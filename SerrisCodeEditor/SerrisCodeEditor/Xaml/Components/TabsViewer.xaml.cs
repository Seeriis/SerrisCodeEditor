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


        private async void CreateDefaultTab()
        => await TabsCreatorAssistant.CreateNewTab(CurrentSelectedIDs.ID_TabsList, "New tab", Encoding.UTF8, StorageListTypes.LocalStorage, "");

        private async void Lists_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListItem selected_list = Lists.SelectedItem as ListItem;

            if(selected_list != null)
                ChangeSelectedList(selected_list.ListID);
            else if(Lists.Items.Count > 0)
                Lists.SelectedIndex = 0;
            else
                await TabsWriteManager.CreateTabsListAsync("Default list");

        }

        private async void Lists_Loaded(object sender, RoutedEventArgs e)
        {
            if(!isLoaded)
            {
                TabsViewerControls.Visibility = Visibility.Collapsed;

                foreach (int id in await TabsAccessManager.GetTabsListIDAsync())
                {
                    var list = await TabsAccessManager.GetTabsListViaIDAsync(id);
                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name });

                    if (AppSettings.Values.ContainsKey("Tabs_list-selected-index"))
                    {
                        if((int)AppSettings.Values["Tabs_list-selected-index"] == id)
                            Lists.SelectedIndex = Lists.Items.Count - 1;
                    }

                }

                if (Lists.Items.Count == 0)
                {
                    await TabsWriteManager.CreateTabsListAsync("Default list");
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
                if(((TabID)Tabs.SelectedItem).ID_Tab != GlobalVariables.CurrentIDs.ID_Tab)
                {
                    CurrentSelectedIDs = (TabID)Tabs.SelectedItem;
                    var tab = await TabsAccessManager.GetTabViaIDAsync(CurrentSelectedIDs);
                    int EncodingType = tab.TabEncoding;
                    string TabType = "";

                    if (EncodingType == 0)
                        EncodingType = Encoding.UTF8.CodePage;

                    if (string.IsNullOrEmpty(tab.TabType))
                        TabType = "TXT";
                    else
                        TabType = tab.TabType.ToUpper();

                    if (tab != null)
                        Messenger.Default.Send(new TabSelectedNotification { tabID = CurrentSelectedIDs.ID_Tab, tabsListID = CurrentSelectedIDs.ID_TabsList, code = await TabsAccessManager.GetTabContentViaIDAsync(CurrentSelectedIDs), contactType = ContactTypeSCEE.SetCodeForEditor, typeLanguage = TabType, typeCode = Encoding.GetEncoding(EncodingType).EncodingName });

                    AppSettings.Values["Tabs_tab-selected-index"] = ((TabID)Tabs.SelectedItem).ID_Tab;
                    AppSettings.Values["Tabs_list-selected-index"] = ((TabID)Tabs.SelectedItem).ID_TabsList;
                }
            }

        }

        private void SetTheme()
        {
            SeparatorA.Fill = GlobalVariables.CurrentTheme.SecondaryColor;
            SeparatorB.Fill = GlobalVariables.CurrentTheme.SecondaryColor;

            Lists.Background = GlobalVariables.CurrentTheme.SecondaryColorFont;
            Lists.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
            Lists.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColor;

            Box_Search.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            Box_Search.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;

            NewListFlyout.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
            NewListFlyout.Background = GlobalVariables.CurrentTheme.SecondaryColorFont;
            NewListFlyout.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColor;

            DeleteButtonFlyout.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
            DeleteButtonFlyout.Background = GlobalVariables.CurrentTheme.SecondaryColorFont;
            DeleteButtonFlyout.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColor;

            OpenButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColor;
            OpenButton.Background = GlobalVariables.CurrentTheme.SecondaryColorFont;
            OpenIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
            OpenText.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;

            CreateButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColor;
            CreateButton.Background = GlobalVariables.CurrentTheme.SecondaryColorFont;
            CreateIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
            CreateText.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;

            TextBoxNewTab.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            TextBoxNewTab.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            NewTabAcceptButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColor;
            NewTabAcceptButton.Background = GlobalVariables.CurrentTheme.SecondaryColorFont;

            IconCreateTab.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
            TextCreateTab.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
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
                                    Tabs.SelectedIndex = Tabs.Items.Count - 1;
                                    break;

                                case TypeUpdateTab.TabDeleted:
                                    if(await TabsWriteManager.DeleteTabAsync(notification.ID))
                                    {
                                        object FindItem = Tabs.Items.SingleOrDefault(o => o.Equals(notification.ID));

                                        if (FindItem != null)
                                        {
                                            Tabs.Items.Remove(FindItem);

                                            //Auto selection
                                            if (CurrentSelectedIDs.ID_Tab == notification.ID.ID_Tab && Tabs.Items.Count - 1 >= 0)
                                            {
                                                Tabs.SelectedIndex = Tabs.Items.Count - 1;
                                            }
                                        }

                                        if (Tabs.Items.Count == 0)
                                            CreateDefaultTab();
                                    }

                                    break;

                                case TypeUpdateTab.NewList:
                                    var list = await TabsAccessManager.GetTabsListViaIDAsync(notification.ID.ID_TabsList);
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
                                    var list = await TabsAccessManager.GetTabsListViaIDAsync(notification.ID.ID_TabsList);
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
            List<int> list_ids = await TabsAccessManager.GetTabsIDAsync(id_list);
            
            if(list_ids.Count == 0)
            {
                CreateDefaultTab();
            }
            else
            {
                foreach (int id in list_ids)
                {
                    Tabs.Items.Add(new TabID { ID_Tab = id, ID_TabsList = id_list });

                    if (GlobalVariables.CurrentIDs.ID_TabsList == CurrentSelectedIDs.ID_TabsList && GlobalVariables.CurrentIDs.ID_Tab == id)
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

        }

        private async void CreateTab()
        {
            await TabsCreatorAssistant.CreateNewTab(CurrentSelectedIDs.ID_TabsList, TextBoxNewTab.Text, Encoding.UTF8, StorageListTypes.LocalStorage, "");
            NewTabCreatorGrid.Visibility = Visibility.Collapsed;
            TextBoxNewTab.Text = "";
        }

        private void Box_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            await TabsCreatorAssistant.OpenFilesAndCreateNewTabsFiles(CurrentSelectedIDs.ID_TabsList, StorageListTypes.LocalStorage);
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
        { await TabsWriteManager.DeleteTabsListAsync(CurrentSelectedIDs.ID_TabsList); FlyoutDeleteList.Hide(); }

        private async void NewList_Click(object sender, RoutedEventArgs e)
        { await TabsWriteManager.CreateTabsListAsync(TextBoxNewList.Text); TextBoxNewList.Text = ""; FlyoutNewList.Hide(); }



        /* =============
         * = VARIABLES =
         * =============
         */



        public TabID CurrentSelectedIDs; bool isLoaded = false, LastTabLoaded = false;
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

    }
}
