using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public class ListItem
    {
        public string ListName { get; set; }
        public int ListID { get; set; }
    }

    public sealed partial class TabsViewer : UserControl
    {
        public TabsViewer()
        { this.InitializeComponent(); }

        private void TabsView_Loaded(object sender, RoutedEventArgs e)
        {
            SetMessenger();
            SetTheme();
        }



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private async void Lists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListItem selected_list = Lists.SelectedItem as ListItem;

            if(selected_list != null)
                ChangeSelectedList(selected_list.ListID);
            else if(Lists.Items.Count > 0)
                Lists.SelectedIndex = 0;
            else
                await write_manager.CreateTabsListAsync("Default list");

        }

        private async void Lists_Loaded(object sender, RoutedEventArgs e)
        {
            if(!isLoaded)
            {
                TabsViewerControls.Visibility = Visibility.Collapsed;

                foreach (int id in await access_manager.GetTabsListIDAsync())
                {
                    var list = await access_manager.GetTabsListViaIDAsync(id);
                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name });

                    if (AppSettings.Values.ContainsKey("Tabs_list-selected-index"))
                    {
                        if((int)AppSettings.Values["Tabs_list-selected-index"] == id)
                            Lists.SelectedIndex = Lists.Items.Count - 1;
                    }

                }

                if (Lists.Items.Count == 0)
                {
                    await write_manager.CreateTabsListAsync("Default list");
                }
                else
                {
                    if (!AppSettings.Values.ContainsKey("Tabs_list-selected-index"))
                        Lists.SelectedIndex = 0;
                }


                isLoaded = true;
            }

        }

        private async void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedItem != null)
            {
                if(((TabID)Tabs.SelectedItem).ID_Tab != temp_variables.CurrentIDs.ID_Tab)
                {
                    CurrentSelectedIDs = (TabID)Tabs.SelectedItem;
                    var tab = await access_manager.GetTabViaIDAsync(CurrentSelectedIDs);
                    Messenger.Default.Send(new TabSelectedNotification { tabID = CurrentSelectedIDs.ID_Tab, tabsListID = CurrentSelectedIDs.ID_TabsList, code = await access_manager.GetTabContentViaIDAsync(CurrentSelectedIDs), contactType = ContactTypeSCEE.SetCodeForEditor, typeLanguage = tab.TabType.ToUpper(), typeCode = Encoding.GetEncoding(tab.TabEncoding).EncodingName });
                    AppSettings.Values["Tabs_tab-selected-index"] = ((TabID)Tabs.SelectedItem).ID_Tab;
                }
            }

        }

        private void SetTheme()
        {
            TabsViewerControls.Background = temp_variables.CurrentTheme.SecondaryColor;

            SeparatorA.Fill = temp_variables.CurrentTheme.SecondaryColorFont;
            SeparatorB.Fill = temp_variables.CurrentTheme.SecondaryColorFont;

            Lists.Background = temp_variables.CurrentTheme.SecondaryColor;
            Lists.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            Lists.BorderBrush = temp_variables.CurrentTheme.SecondaryColorFont;

            Box_Search.Background = temp_variables.CurrentTheme.SecondaryColorFont;

            NewListFlyout.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            NewListFlyout.BorderBrush = temp_variables.CurrentTheme.SecondaryColorFont;

            DeleteButtonFlyout.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            DeleteButtonFlyout.BorderBrush = temp_variables.CurrentTheme.SecondaryColorFont;

            OpenButton.BorderBrush = temp_variables.CurrentTheme.SecondaryColorFont;
            OpenIcon.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            OpenText.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;

            CreateButton.BorderBrush = temp_variables.CurrentTheme.SecondaryColorFont;
            CreateIcon.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            CreateText.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;

            TextBoxNewTab.Background = temp_variables.CurrentTheme.MainColor;
            TextBoxNewTab.Foreground = temp_variables.CurrentTheme.MainColorFont;

            NewTabAcceptButton.BorderBrush = temp_variables.CurrentTheme.SecondaryColorFont;
            IconCreateTab.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            TextCreateTab.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
        }

        private void SetMessenger()
        {
            Messenger.Default.Register<EditorViewNotification>(this, async (notification_ui) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        SetTheme();
                    }
                    catch { }

                });

            });

            Messenger.Default.Register<SheetViewMode>(this, async (notification_sheetview) => 
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        switch(notification_sheetview)
                        {
                            case SheetViewMode.Deployed:
                                TabsViewerControls.Visibility = Visibility.Visible;
                                break;

                            case SheetViewMode.Minimized:
                                TabsViewerControls.Visibility = Visibility.Collapsed;
                                break;
                        }
                    }
                    catch { }

                });
            });

            Messenger.Default.Register<STSNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    try
                    {
                        if (CurrentSelectedIDs.ID_TabsList == notification.ID.ID_TabsList)
                        {

                            switch (notification.Type)
                            {
                                case TypeUpdateTab.NewTab:
                                    Tabs.Items.Add(notification.ID);

                                    //Auto selection
                                    //Tabs.SelectedIndex = Tabs.Items.Count - 1;
                                    break;

                                case TypeUpdateTab.TabDeleted:
                                    int i = 0;
                                    foreach (TabID item in Tabs.Items)
                                    {
                                        if (item.ID_Tab == notification.ID.ID_Tab)
                                        {
                                            //Auto selection
                                            if (CurrentSelectedIDs.ID_Tab == notification.ID.ID_Tab && Tabs.Items.Count - 2 >= 0)
                                            {
                                                Tabs.SelectedIndex = Tabs.Items.Count - 2;
                                            }

                                            Tabs.Items.RemoveAt(i);
                                            break;
                                        }

                                        i++;
                                    }
                                    await write_manager.DeleteTabAsync(notification.ID);
                                    break;

                                case TypeUpdateTab.NewList:
                                    var list = await access_manager.GetTabsListViaIDAsync(notification.ID.ID_TabsList);
                                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name });
                                    Lists.SelectedIndex = Lists.Items.Count - 1;
                                    break;

                                case TypeUpdateTab.ListDeleted:
                                    Lists.Items.RemoveAt(Lists.SelectedIndex);
                                    break;
                            }

                        }
                        else
                            switch (notification.Type)
                            {
                                case TypeUpdateTab.NewList:
                                    var list = await access_manager.GetTabsListViaIDAsync(notification.ID.ID_TabsList);
                                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name });
                                    Lists.SelectedIndex = Lists.Items.Count - 1;
                                    break;

                                case TypeUpdateTab.ListDeleted:
                                    Lists.Items.RemoveAt(Lists.SelectedIndex);
                                    break;
                            }
                    }
                    catch { }

                });

            });

        }

        private async void ChangeSelectedList(int id_list)
        {
            Tabs.Items.Clear();
            CurrentSelectedIDs.ID_TabsList = id_list;
            AppSettings.Values["Tabs_list-selected-index"] = id_list;
            List<int> list_ids = await access_manager.GetTabsIDAsync(id_list);
            
            foreach(int id in list_ids)
            {
                Tabs.Items.Add(new TabID { ID_Tab = id, ID_TabsList = id_list });

                if (temp_variables.CurrentIDs.ID_TabsList == CurrentSelectedIDs.ID_TabsList && temp_variables.CurrentIDs.ID_Tab == id)
                {
                    Tabs.SelectedIndex = Tabs.Items.Count - 1;
                }

                //Select the last selected tab when TabsViewer is initialized
                if (AppSettings.Values.ContainsKey("Tabs_tab-selected-index"))
                {
                    if (!LastTabLoaded && (int)AppSettings.Values["Tabs_tab-selected-index"] == id && (int)AppSettings.Values["Tabs_list-selected-index"] == id_list)
                    {
                        Tabs.SelectedIndex = Tabs.Items.Count - 1;
                        LastTabLoaded = true;
                    }
                }
                else
                {
                    LastTabLoaded = true;
                }

            }

        }

        private async void CreateTab()
        {
            await CreatorAssistant.CreateNewTab(CurrentSelectedIDs.ID_TabsList, TextBoxNewTab.Text, Encoding.UTF8, StorageListTypes.LocalStorage, "");
            NewTabCreatorGrid.Visibility = Visibility.Collapsed;
            TextBoxNewTab.Text = "";
        }

        private void Box_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            await CreatorAssistant.OpenFilesAndCreateNewTabsFiles(CurrentSelectedIDs.ID_TabsList, StorageListTypes.LocalStorage);
        }

        private void TextBoxNewTab_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.KeyStatus.RepeatCount == 1)
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    CreateTab();
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (NewTabCreatorGrid.Visibility == Visibility.Collapsed)
                NewTabCreatorGrid.Visibility = Visibility.Visible;
            else
                NewTabCreatorGrid.Visibility = Visibility.Collapsed;
        }

        private void NewTabAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            CreateTab();
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        { await write_manager.DeleteTabsListAsync(CurrentSelectedIDs.ID_TabsList); FlyoutDeleteList.Hide(); }

        private async void NewList_Click(object sender, RoutedEventArgs e)
        { await write_manager.CreateTabsListAsync(TextBoxNewList.Text); TextBoxNewList.Text = ""; FlyoutNewList.Hide(); }



        /* =============
         * = VARIABLES =
         * =============
         */



        public TabID CurrentSelectedIDs; bool isLoaded = false, LastTabLoaded = false;
        TabsAccessManager access_manager = new TabsAccessManager(); TabsWriteManager write_manager = new TabsWriteManager(); TabsCreatorAssistant CreatorAssistant = new TabsCreatorAssistant();
        TempContent temp_variables = new TempContent();
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

    }
}
