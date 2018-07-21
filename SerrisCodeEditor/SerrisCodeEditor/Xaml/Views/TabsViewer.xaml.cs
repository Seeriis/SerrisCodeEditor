using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Templates;
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
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Views
{
    public class ListItem
    {
        public string ListName { get; set; }
        public BitmapImage ListIcon { get; set; }
        public int ListID { get; set; }
    }

    public sealed partial class TabsViewer : UserControl
    {
        /* =============
         * = VARIABLES =
         * =============
         */

        public TabID CurrentSelectedIDs; bool isLoaded = false, LastTabLoaded = false, StorageListIsLoaded = false, DefaultFunctionsLoaded = false;
        int CurrentCreationType = -1;
        string TabTemplateContent = "";
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;


        public TabsViewer()
        { this.InitializeComponent(); }

        private void TabsView_Loaded(object sender, RoutedEventArgs e)
        {
            if(!DefaultFunctionsLoaded)
            {
                SetMessenger();
                SetTheme();
                DefaultFunctionsLoaded = true;
            }
        }



        /* =============
         * = FUNCTIONS =
         * =============
         */


        private void CreateDefaultTab()
        => TabsCreatorAssistant.CreateNewTab(CurrentSelectedIDs.ID_TabsList, "New tab", Encoding.UTF8, StorageListTypes.LocalStorage, "");

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

                foreach (int id in TabsAccessManager.GetTabsListID())
                {
                    var list = TabsAccessManager.GetTabsListViaID(id);
                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name, ListIcon = await ModulesAccessManager.GetModuleIconViaIDAsync(list.TabsListProjectTypeID, true) });

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
                    var tab = TabsAccessManager.GetTabViaID(CurrentSelectedIDs);

                    if(tab.TabContentType == ContentType.File)
                    {
                        int EncodingType = tab.TabEncoding;
                        string TabType = "";

                        if (EncodingType == 0)
                            EncodingType = Encoding.UTF8.CodePage;

                        if (string.IsNullOrEmpty(tab.TabType))
                            TabType = "TXT";
                        else
                            TabType = tab.TabType.ToUpper();

                        if (tab != null)
                            Messenger.Default.Send(new TabSelectedNotification { tabID = CurrentSelectedIDs.ID_Tab, tabsListID = CurrentSelectedIDs.ID_TabsList, code = await TabsAccessManager.GetTabContentViaIDAsync(CurrentSelectedIDs), contactType = ContactTypeSCEE.SetCodeForEditor, typeLanguage = TabType, typeCode = Encoding.GetEncoding(EncodingType).EncodingName, cursorPositionColumn = tab.TabCursorPosition.column, cursorPositionLineNumber = tab.TabCursorPosition.row, tabName = tab.TabName });

                        AppSettings.Values["Tabs_tab-selected-index"] = ((TabID)Tabs.SelectedItem).ID_Tab;
                        AppSettings.Values["Tabs_list-selected-index"] = ((TabID)Tabs.SelectedItem).ID_TabsList;
                    }
                }
            }

        }

        private void SetTheme()
        {
            //SeparatorA.Fill = GlobalVariables.CurrentTheme.SecondaryColor;

            Lists.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

            Box_Search.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(200, GlobalVariables.CurrentTheme.SecondaryColor.Color.R, GlobalVariables.CurrentTheme.SecondaryColor.Color.G, GlobalVariables.CurrentTheme.SecondaryColor.Color.B));
            Box_Search.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            NewListFlyout.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

            /*DeleteButtonFlyout.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;
            DeleteButtonFlyout.Background = GlobalVariables.CurrentTheme.SecondaryColorFont;
            DeleteButtonFlyout.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColor;*/

            CreateButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
            CreateButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            CreateIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            TextBoxNewFileProject.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(200, GlobalVariables.CurrentTheme.SecondaryColor.Color.R, GlobalVariables.CurrentTheme.SecondaryColor.Color.G, GlobalVariables.CurrentTheme.SecondaryColor.Color.B));
            TextBoxNewFileProject.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            TextBoxNewFileProject.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;

            NewTabAcceptButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
            NewTabAcceptButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            IconCreateTab.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            BlankTabRadioButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            TabTemplateRadioButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            DefaultListRadioButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            NewProjectRadioButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

            OpenFilesButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
            OpenFilesButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            OpenFilesButton.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            OpenFolderButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
            OpenFolderButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            OpenFolderButton.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            OpenProjectButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
            OpenProjectButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            OpenProjectButton.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            TabTemplatesListView.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            //Update buttons colors
            ChangeCreationType(0);
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
                                ShowCreatorGrid(false);
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
                                    if(!TabsAccessManager.GetTabViaID(notification.ID).TabInvisibleByDefault)
                                    {
                                        Tabs.Items.Add(notification.ID);

                                        //Auto selection
                                        Tabs.SelectedIndex = Tabs.Items.Count - 1;
                                    }
                                    break;

                                case TypeUpdateTab.SelectTab:
                                    int Position = 0;
                                    foreach(TabID CurrId in Tabs.Items)
                                    {
                                        if(CurrId.ID_Tab == notification.ID.ID_Tab && CurrId.ID_TabsList == notification.ID.ID_TabsList)
                                        {
                                            Tabs.SelectedIndex = Position;
                                            break;
                                        }

                                        Position++;
                                    }
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
                                    var list = TabsAccessManager.GetTabsListViaID(notification.ID.ID_TabsList);
                                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name, ListIcon = await ModulesAccessManager.GetModuleIconViaIDAsync(list.TabsListProjectTypeID, true) });
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
                                    var list = TabsAccessManager.GetTabsListViaID(notification.ID.ID_TabsList);
                                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name, ListIcon = await ModulesAccessManager.GetModuleIconViaIDAsync(list.TabsListProjectTypeID, true) });
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

        private void ChangeSelectedList(int id_list)
        {
            Tabs.Items.Clear();
            CurrentSelectedIDs.ID_TabsList = id_list;
            List<int> list_ids = TabsAccessManager.GetTabsID(id_list);
            
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

        private void ChangeCreationType(int Type)
        {
            if(Type != CurrentCreationType)
            {
                CurrentCreationType = Type;

                switch (CurrentCreationType)
                {
                    case 0:
                        TextBoxNewFileProject.PlaceholderText = "Example: toothless.js";
                        CreatorGridTitle.Text = "Create new tab";

                        SelectTabButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
                        SelectTabButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
                        SelectTabButton.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

                        SelectListButton.Background = new SolidColorBrush(Colors.Transparent);
                        SelectListButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        SelectListButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                        CreationTabOptions.Visibility = Visibility.Visible;
                        CreationListOptions.Visibility = Visibility.Collapsed;
                        break;

                    case 1:
                        TextBoxNewFileProject.PlaceholderText = "Example: project csharp";
                        CreatorGridTitle.Text = "Create new list / project";

                        SelectListButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
                        SelectListButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
                        SelectListButton.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

                        SelectTabButton.Background = new SolidColorBrush(Colors.Transparent);
                        SelectTabButton.BorderBrush = new SolidColorBrush(Colors.Transparent);
                        SelectTabButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                        CreationTabOptions.Visibility = Visibility.Collapsed;
                        CreationListOptions.Visibility = Visibility.Visible;

                        break;
                }

            }

        }

        private void CreateTab()
        {
            if(CurrentSelectedIDs.ID_Tab != 0)
            {
                StorageListTypes SelectedType = ((StorageTypeDefinition)TabStorageType.SelectedValue).Type;

                switch (TabsAccessManager.GetTabViaID(CurrentSelectedIDs).TabContentType)
                {
                    case ContentType.File:
                        TabsCreatorAssistant.CreateNewTab(CurrentSelectedIDs.ID_TabsList, TextBoxNewFileProject.Text, Encoding.UTF8, SelectedType, TabTemplateContent);
                        break;

                    //Create file in the selected folder !
                    case ContentType.Folder:
                        TabsCreatorAssistant.CreateNewTabInFolder(CurrentSelectedIDs.ID_TabsList, CurrentSelectedIDs, TextBoxNewFileProject.Text, Encoding.UTF8, SelectedType, TabTemplateContent);
                        break;
                }
            }
            else
            {
                TabsCreatorAssistant.CreateNewTab(CurrentSelectedIDs.ID_TabsList, TextBoxNewFileProject.Text, Encoding.UTF8, StorageListTypes.LocalStorage, TabTemplateContent);
            }


            TextBoxNewFileProject.Text = "";
        }

        private async void CreateList()
        {
            await TabsWriteManager.CreateTabsListAsync(TextBoxNewFileProject.Text);
            TextBoxNewFileProject.Text = "";
        }

        private void CreateListOrTab()
        {
            switch (CurrentCreationType)
            {
                case 0:
                    CreateTab();
                    break;

                case 1:
                    CreateList();
                    break;
            }

            ShowCreatorGrid(false);
        }

        private void Box_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void OpenFilesButton_Click(object sender, RoutedEventArgs e)
        {
            StorageListTypes SelectedType = ((StorageTypeDefinition)TabStorageType.SelectedValue).Type;

            LoadingGrid.IsLoading = true;
            await TabsCreatorAssistant.OpenFilesAndCreateNewTabsFiles(CurrentSelectedIDs.ID_TabsList, SelectedType);
            LoadingGrid.IsLoading = false;
            ShowCreatorGrid(false);
        }

        private async void OpenFolderButton_Click(object sender, RoutedEventArgs e)
        {
            LoadingGrid.IsLoading = true;
            await TabsCreatorAssistant.OpenFolder(CurrentSelectedIDs.ID_TabsList, StorageListTypes.LocalStorage);
            LoadingGrid.IsLoading = false;
            ShowCreatorGrid(false);
        }

        private void TextBoxNewFileProject_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.KeyStatus.RepeatCount == 1)
            {
                if (e.Key == Windows.System.VirtualKey.Enter)
                {
                    CreateListOrTab();
                }
            }
        }

        private void ShowCreatorGrid(bool ShowIt)
        {
            if (ShowIt)
            {
                //Open CreatorGrid...
                CreateIcon.Text = "";
                Tabs.Visibility = Visibility.Collapsed;
                SeparatorA.Visibility = Visibility.Collapsed;
                CreatorGrid.Visibility = Visibility.Visible;
            }
            else
            {
                //Close CreatorGrid and show Tabs...
                CreateIcon.Text = "";
                CreatorGrid.Visibility = Visibility.Collapsed;
                Tabs.Visibility = Visibility.Visible;
                SeparatorA.Visibility = Visibility.Visible;
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            if (CreatorGrid.Visibility == Visibility.Collapsed)
            {
                ShowCreatorGrid(true);
            }
            else 
            {
                ShowCreatorGrid(false);
            }
        }

        private async void TabTemplatesListView_Loaded(object sender, RoutedEventArgs e)
        {
            List<TemplatesFileInfos> Templates = new List<TemplatesFileInfos>();
            foreach(InfosModule Module in ModulesAccessManager.GetSpecificModules(true, SerrisModulesServer.Type.ModuleTypesList.Templates))
            {
                TemplatesReader Reader = new TemplatesReader(Module.ID);
                Templates.AddRange(await Reader.GetTemplatesFilesContentAsync());
            }

            var TemplatesGrouping = from c in Templates
                                    group c by c.Type;

            TabTemplatesList.Source = TemplatesGrouping;
            TabTemplatesListView.SelectedIndex = -1;
        }

        private void TabTemplatesListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(TabTemplatesListView.SelectedItem != null)
            if(TabTemplateRadioButton.IsChecked == true)
            {
                TemplatesFileInfos TemplateInfos = TabTemplatesListView.SelectedItem as TemplatesFileInfos;
                TextBoxNewFileProject.Text = TemplateInfos.SuggestedTemplateName;
                TabTemplateContent = TemplateInfos.Content;
            }
        }

        private void TabStorageType_Loaded(object sender, RoutedEventArgs e)
        {
            if (!StorageListIsLoaded)
            {
                foreach (StorageTypeDefinition StorageType in StorageTypesAvailable.GetStorageTypesAvailable())
                {
                    TabStorageType.Items.Add(StorageType);
                }

                TabStorageType.SelectedIndex = 0;
                StorageListIsLoaded = true;
            }
            
        }

        private void BlankTabRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            TabTemplateContent = "";

            if(TabTemplatesListView != null)
            {
                TabTemplatesListView.Visibility = Visibility.Collapsed;
                TabTemplatesListView.SelectedIndex = -1;
            }
        }

        private void TabTemplateRadioButton_Checked(object sender, RoutedEventArgs e)
        => TabTemplatesListView.Visibility = Visibility.Visible;

        private void NewTabAcceptButton_Click(object sender, RoutedEventArgs e)
        => CreateListOrTab();

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        { await TabsWriteManager.DeleteTabsListAsync(CurrentSelectedIDs.ID_TabsList); /*FlyoutDeleteList.Hide();*/ }

        private void SelectTabButton_Click(object sender, RoutedEventArgs e)
        => ChangeCreationType(0);

        private void SelectListButton_Click(object sender, RoutedEventArgs e)
        => ChangeCreationType(1);

    }
}
