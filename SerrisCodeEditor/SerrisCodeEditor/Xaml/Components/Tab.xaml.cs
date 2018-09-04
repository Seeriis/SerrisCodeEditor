using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.ProgrammingLanguage;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public sealed partial class Tab : UserControl
    {
        InfosTab current_tab = new InfosTab(); int current_list; bool infos_opened = false, enable_selection = false, loaded = false;
        TabID CurrentIDs;
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

        public Tab()
        {
            this.InitializeComponent();

            DataContextChanged += Tab_DataContextChanged;
        }

        private void Tab_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            if (DataContext != null)
            {
                TabID ids = (TabID)DataContext;

                if(CurrentIDs.ID_Tab != ids.ID_Tab && CurrentIDs.ID_TabsList != ids.ID_TabsList)
                {
                    if (current_tab == null)
                        current_tab = new InfosTab();

                    if (AppSettings.Values.ContainsKey("ui_leftpanelength"))
                    {
                        GridInfoLeft.Width = (int)AppSettings.Values["ui_leftpanelength"];
                        StackInfos.Margin = new Thickness((int)AppSettings.Values["ui_leftpanelength"], 0, 0, 0);
                    }

                    current_tab.ID = ids.ID_Tab; current_list = ids.ID_TabsList;
                    UpdateTabInformations();
                }

            }
        }

        private void TabComponent_Loaded(object sender, RoutedEventArgs e)
        {
            if(!loaded)
            {
                SetMessenger();
                SetTheme();

                foreach (string Language in LanguagesHelper.GetLanguagesNames())
                {
                    list_types.Items.Add(Language);
                }

                loaded = true;
            }

            //UpdateTabInformations();
        }

        private void TabComponent_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(current_tab.TabOriginalPathContent) && current_tab.TabContentType == ContentType.File)
                ShowPath.Begin();
        }

        private void TabComponent_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(current_tab.TabOriginalPathContent) && current_tab.TabContentType == ContentType.File)
                ShowName.Begin();
        }



        /*
         * =============
         * = FUNCTIONS =
         * =============
         */



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

            Messenger.Default.Register<STSNotification>(this, async (nm) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    try
                    {

                        if (nm.ID.ID_Tab == current_tab.ID && nm.ID.ID_TabsList == current_list)
                        {
                            switch (nm.Type)
                            {
                                case TypeUpdateTab.TabUpdated:
                                    UpdateTabInformations();
                                    break;

                                case TypeUpdateTab.TabNewModifications:
                                    current_tab.TabNewModifications = true;
                                    await TabsWriteManager.PushUpdateTabAsync(current_tab, current_list, false);
                                    UpdateTabInformations();
                                    break;
                            }
                        }

                    }
                    catch { }
                });

            });

            Messenger.Default.Register<TabSelectedNotification>(this, async (nm) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    try
                    {

                        if (nm.tabID == current_tab.ID && nm.tabsListID == current_list)
                        {
                            switch (nm.contactType)
                            {
                                case ContactTypeSCEE.GetCodeForTab:
                                    await TabsWriteManager.PushTabContentViaIDAsync(new TabID { ID_Tab = current_tab.ID, ID_TabsList = current_list }, current_tab.TabContentTemporary, true);
                                    break;
                            }
                        }

                    }
                    catch { }

                });

            });
        }

        private void SetTheme()
        {
            TextBoxRename.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            TextBoxRename.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            TextBoxRename.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;

            RenameAcceptButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
            RenameAcceptButton.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            RenameAcceptButtonIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
        }

        int TempTabID = 0;
        private async void UpdateTabInformations()
        {
            //Set temp tab + tabs list ID
            try
            {
                current_tab = TabsAccessManager.GetTabViaID(new TabID { ID_Tab = current_tab.ID, ID_TabsList = current_list });

                name_tab.Text = current_tab.TabName;

                //Tooltip name
                ToolTip ButtonTooltip = new ToolTip();
                ButtonTooltip.Content = current_tab.TabName;
                ToolTipService.SetToolTip(IconAndTabNameGrid, ButtonTooltip);

                switch (current_tab.TabContentType)
                {
                    case ContentType.File:
                        string ModuleIDIcon = LanguagesHelper.GetModuleIDOfLangageType(current_tab.TabType);
                        TabIcon.Source = await ModulesAccessManager.GetModuleIconViaIDAsync(ModuleIDIcon, ModulesAccessManager.GetModuleViaID(ModuleIDIcon).ModuleSystem);

                        encoding_file.Text = Encoding.GetEncoding(current_tab.TabEncoding).EncodingName;

                        if (!string.IsNullOrEmpty(current_tab.TabOriginalPathContent))
                        {
                            switch(current_tab.TabStorageMode)
                            {
                                case StorageListTypes.LocalStorage:
                                    path_tab.Text = current_tab.TabOriginalPathContent;
                                    break;

                                case StorageListTypes.OneDrive:
                                    path_tab.Text = "OneDrive file";
                                    break;
                            }
                            
                            Size_Stackpanel.Visibility = Visibility.Visible;
                            Modified_Stackpanel.Visibility = Visibility.Visible;
                            Created_Stackpanel.Visibility = Visibility.Visible;

                            Rename_Tab.IsEnabled = false;
                        }
                        else
                        {
                            Size_Stackpanel.Visibility = Visibility.Collapsed;
                            Modified_Stackpanel.Visibility = Visibility.Collapsed;
                            Created_Stackpanel.Visibility = Visibility.Collapsed;

                            Rename_Tab.IsEnabled = true;
                        }

                        Notification.ShowBadge = current_tab.TabNewModifications;

                        TabsListGrid.Visibility = Visibility.Collapsed;
                        TabIcon.Visibility = Visibility.Visible;
                        FolderIcon.Visibility = Visibility.Collapsed;
                        StackInfos.Visibility = Visibility.Visible;

                        MaxHeightAnimShow.Value = 200;
                        MaxHeightAnimRemove.Value = 200;
                        break;

                    case ContentType.Folder:
                        Notification.ShowBadge = false;
                        Rename_Tab.IsEnabled = false;

                        More_Tab.Visibility = Visibility.Visible;
                        TabsListGrid.Visibility = Visibility.Visible;
                        StackInfos.Visibility = Visibility.Collapsed;

                        TabIcon.Visibility = Visibility.Collapsed;
                        FolderIcon.Visibility = Visibility.Visible;

                        if (TempTabID != current_tab.ID && TabsList.ListID != current_list)
                        {
                            if(current_tab.FolderOpened)
                            {
                                ShowInfos.Begin(); infos_opened = true;
                            }
                            else
                            {
                                infos_opened = false;
                            }

                            TabsList.ListTabs.Items.Clear();
                            TempTabID = current_tab.ID;
                            TabsList.ListID = current_list;
                            foreach (int ID in current_tab.FolderContent)
                            {
                                try
                                {
                                    if (TabsAccessManager.GetTabViaID(new TabID { ID_Tab = ID, ID_TabsList = current_list }) != null)
                                    {
                                        TabsList.ListTabs.Items.Add(new TabID { ID_Tab = ID, ID_TabsList = current_list });
                                        if ((int)AppSettings.Values["Tabs_tab-selected-index"] == ID && (int)AppSettings.Values["Tabs_list-selected-index"] == current_list)
                                        {
                                            TabsList.ListTabs.SelectedIndex = TabsList.ListTabs.Items.Count - 1;
                                        }
                                    }
                                }
                                catch { }
                            }
                        }

                        MaxHeightAnimShow.Value = 1500;
                        MaxHeightAnimRemove.Value = 1500;
                        break;
                }


            }
            catch { }
        }

        private async void TabsList_ListTabDeleted(object sender, TabID e)
        {
            if(current_tab != null)
            {
                current_tab.FolderContent.Remove(e.ID_Tab);
                await TabsWriteManager.PushUpdateTabAsync(current_tab, current_list, false);
            }
        }

        private void Close_Tab_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Messenger.Default.Send(new STSNotification { ID = new TabID { ID_Tab = current_tab.ID, ID_TabsList = current_list }, Type = TypeUpdateTab.TabDeleted });
            }
            catch { }
        }

        private async void list_types_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(list_types.SelectedIndex != -1 && (string)list_types.SelectedItem != current_tab.TabType)
            {
                current_tab.TabType = LanguagesHelper.GetLanguageTypeViaName((string)list_types.SelectedItem);
                await TabsWriteManager.PushUpdateTabAsync(current_tab, current_list, true);
            }
        }

        private void Rename_Tab_Click(object sender, RoutedEventArgs e)
        {
            if(RenameGrid.Visibility == Visibility.Collapsed)
            {
                TextBoxRename.Text = current_tab.TabName;
                name_tab.Visibility = Visibility.Collapsed;
                RenameGrid.Visibility = Visibility.Visible;
            }
            else
            {
                name_tab.Visibility = Visibility.Visible;
                RenameGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void TextBoxRename_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.KeyStatus.RepeatCount == 1)
            {
                if (e.Key == Windows.System.VirtualKey.Enter && !string.IsNullOrWhiteSpace(TextBoxRename.Text))
                {
                    RenameTab();
                }
            }
        }

        private void TextBoxRename_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TextBoxRename.Text))
            {
                RenameAcceptButton.IsEnabled = false;
            }
            else
            {
                RenameAcceptButton.IsEnabled = true;
            }
        }

        private void RenameAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            RenameTab();
        }

        private void Grid_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);
        }

        private async void RenameTab()
        {
            current_tab.TabName = TextBoxRename.Text;
            await TabsWriteManager.PushUpdateTabAsync(current_tab, current_list, false);
            RenameGrid.Visibility = Visibility.Collapsed;
            name_tab.Visibility = Visibility.Visible;
        }

        private async void More_Tab_Click(object sender, RoutedEventArgs e)
        {
            if (infos_opened)
            {
                RemoveInfos.Begin(); infos_opened = false;

                if(current_tab.TabContentType == ContentType.Folder)
                {
                    current_tab.FolderOpened = false;
                    await TabsWriteManager.PushUpdateTabAsync(current_tab, current_list, false);
                }
            }
            else
            {
                enable_selection = false;

                switch(current_tab.TabContentType)
                {
                    case ContentType.File:
                        try
                        {
                            list_types.SelectedItem = LanguagesHelper.GetLanguageNameViaType(current_tab.TabType);

                            switch (current_tab.TabStorageMode)
                            {
                                case StorageListTypes.LocalStorage:
                                    StorageFile file = await StorageFile.GetFileFromPathAsync(current_tab.TabOriginalPathContent);
                                    BasicProperties properties = await file.GetBasicPropertiesAsync();

                                    if (properties.Size != 0)
                                    {

                                        if (properties.Size > 1024f) //Ko
                                        {
                                            size_file.Text = string.Format("{0:0.00}", (properties.Size / 1024f)) + " Ko";

                                            if ((properties.Size / 1024f) > 1024f) //Mo
                                            {
                                                size_file.Text = string.Format("{0:0.00}", ((properties.Size / 1024f) / 1024f)) + " Mo";
                                            }
                                        }
                                        else //Octect
                                        {
                                            size_file.Text = properties.Size + " Octect(s)";
                                        }

                                    }

                                    modified_file.Text = properties.DateModified.ToString();
                                    created_file.Text = file.DateCreated.ToString();
                                    break;

                                case StorageListTypes.OneDrive:
                                    if(await OneDriveAuthHelper.OneDriveAuthentification())
                                    {
                                        var Item = await TabsDataCache.OneDriveClient.Drive.Items[current_tab.TabOriginalPathContent].Request().GetAsync();

                                        if (Item.Size != 0)
                                        {

                                            if (Item.Size > 1024f) //Ko
                                            {
                                                size_file.Text = string.Format("{0:0.00}", (Item.Size / 1024f)) + " Ko";

                                                if ((Item.Size / 1024f) > 1024f) //Mo
                                                {
                                                    size_file.Text = string.Format("{0:0.00}", ((Item.Size / 1024f) / 1024f)) + " Mo";
                                                }
                                            }
                                            else //Octect
                                            {
                                                size_file.Text = Item.Size + " Octect(s)";
                                            }

                                        }

                                        modified_file.Text = Item.LastModifiedDateTime.ToString();
                                        created_file.Text = Item.CreatedDateTime.ToString();
                                        
                                        //path_tab.Text = System.Net.WebUtility.HtmlDecode(Item.ParentReference.Path);
                                    }
                                    break;
                            }

                        }
                        catch { }

                        break;

                    case ContentType.Folder:
                        current_tab.FolderOpened = true;
                        await TabsWriteManager.PushUpdateTabAsync(current_tab, current_list, false);
                        break;
                }

                ShowInfos.Begin(); infos_opened = true; enable_selection = true;
            }

        }
    }
}
