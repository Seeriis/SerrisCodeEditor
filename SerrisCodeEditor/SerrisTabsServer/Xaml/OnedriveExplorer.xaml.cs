using GalaSoft.MvvmLight.Messaging;
using Microsoft.OneDrive.Sdk;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisTabsServer.Xaml
{
    public enum OnedriveExplorerMode
    {
        CreateFile,
        SelectFile
    }

    public class OnedriveExplorerItem
    {
        public string IconContent { get; set; }
        public string LastModificationDatetime { get; set; }
        public InfosTab Tab { get; set; }
    }

    public sealed partial class OnedriveExplorer : Page
    {
        List<Tuple<string, string>> OneDriveNavigationHistory = new List<Tuple<string, string>>();
        InfosTab CurrentTab = new InfosTab();
        TabID TabIDRequest;
        OnedriveExplorerMode CurrentExplorerMode;

        public OnedriveExplorer()
        {
            this.InitializeComponent();
        }

        private async void Tabs_Loaded(object sender, RoutedEventArgs e)
        {
            if(await OneDriveAuthHelper.OneDriveAuthentification())
            {
                if (OneDriveNavigationHistory.Count == 0)
                {
                    OneDriveLoadStart();
                    Tabs.Items.Clear(); var root = await OneDriveGetRootFolder();
                    OneDriveNavigationHistory.Add(new Tuple<string, string>(root.Item1, root.Item2));
                    foreach (var item in root.Item3)
                    {
                        Tabs.Items.Add(item);
                    }
                    OnedriveLoadFinished();
                }
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var Infos = e.Parameter as Tuple<OnedriveExplorerMode, TabID>;
            CurrentExplorerMode = Infos.Item1;
            TabIDRequest = Infos.Item2;
        }

        private async void back_onedrive_Click(object sender, RoutedEventArgs e)
        {
            if (OneDriveNavigationHistory.Count > 0 && OneDriveNavigationHistory.Count - 2 >= 0)
            {
                OneDriveNavigationHistory.Remove(OneDriveNavigationHistory[OneDriveNavigationHistory.Count - 1]);

                var tuple = OneDriveNavigationHistory[OneDriveNavigationHistory.Count - 1];
                Tabs.Items.Clear(); od_foldername.Text = tuple.Item1;
                OneDriveLoadStart();
                foreach (var item in await OneDriveNavigate(tuple.Item2))
                {
                    Tabs.Items.Add(item);
                }

                OnedriveLoadFinished();
            }
        }

        private async void home_onedrive_Click(object sender, RoutedEventArgs e)
        {
            OneDriveLoadStart();
            Tabs.Items.Clear(); var root = await OneDriveGetRootFolder();
            OneDriveNavigationHistory.Add(new Tuple<string, string>(root.Item1, root.Item2)); od_foldername.Text = "root";
            foreach (var item in root.Item3)
            {
                Tabs.Items.Add(item);
            }
            OnedriveLoadFinished();
        }

        private void reload_onedrive_Click(object sender, RoutedEventArgs e)
        {
            ReloadOnedrive();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window.Current.Close();
            }
            catch { }
        }

        private async void Accept_Click(object sender, RoutedEventArgs e)
        {
            OneDriveLoadStart();
            InfosTab RequestedTab = TabsAccessManager.GetTabViaID(TabIDRequest);

            switch(CurrentExplorerMode)
            {
                case OnedriveExplorerMode.CreateFile:
                    MemoryStream stream = new MemoryStream();
                    var file = await TabsDataCache.OneDriveClient.Drive.Items[CurrentTab.TabOriginalPathContent].ItemWithPath(RequestedTab.TabName).Content.Request().PutAsync<Item>(stream);

                    RequestedTab.TabOriginalPathContent = file.Id;
                    RequestedTab.TabDateModified = file.LastModifiedDateTime.ToString();
                    await TabsWriteManager.PushUpdateTabAsync(RequestedTab, TabIDRequest.ID_TabsList, false);
                    break;

                case OnedriveExplorerMode.SelectFile:
                    var tab = new InfosTab { TabName = CurrentTab.TabName, TabStorageMode = Storage.StorageListTypes.OneDrive, TabContentType = ContentType.File, CanBeDeleted = true, CanBeModified = true, TabOriginalPathContent = CurrentTab.TabOriginalPathContent, TabInvisibleByDefault = false, TabType = LanguagesHelper.GetLanguageType(CurrentTab.TabName) };

                    int id_tab = await TabsWriteManager.CreateTabAsync(tab, TabIDRequest.ID_TabsList, false);
                    if (await new StorageRouter(TabsAccessManager.GetTabViaID(new TabID { ID_Tab = id_tab, ID_TabsList = TabIDRequest.ID_TabsList }), TabIDRequest.ID_TabsList).ReadFile(true))
                    {
                        Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewTab, ID = new TabID { ID_Tab = id_tab, ID_TabsList = TabIDRequest.ID_TabsList } });
                    }
                    break;
            }

            OnedriveLoadFinished();
            try { Window.Current.Close(); } catch { }
        }

        private async void NewFolder_Create_Click(object sender, RoutedEventArgs e)
        {
            Item Folder = new Item { Name = NewFolder_Name.Text, Folder = new Folder() };
            await TabsDataCache.OneDriveClient.Drive.Items[OneDriveNavigationHistory[OneDriveNavigationHistory.Count - 1].Item2].Children.Request().AddAsync(Folder);
            ReloadOnedrive();
        }

        private async void tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            OnedriveExplorerItem Tab = (sender as ListView).SelectedItem as OnedriveExplorerItem;
            if (Tabs.SelectedIndex != -1)
            {
                Tabs.SelectedIndex = -1;

                switch (Tab.Tab.TabContentType)
                {
                    case ContentType.Folder:
                        Tabs.Items.Clear(); od_foldername.Text = Tab.Tab.TabName; OneDriveNavigationHistory.Add(new Tuple<string, string>(Tab.Tab.TabName, Tab.Tab.TabOriginalPathContent));

                        OneDriveLoadStart();
                        foreach (var item in await OneDriveNavigate(Tab.Tab.TabOriginalPathContent))
                        {
                            Tabs.Items.Add(item);
                        }
                        OnedriveLoadFinished();

                        if (CurrentExplorerMode == OnedriveExplorerMode.CreateFile)
                        {
                            CurrentTab = Tab.Tab;
                            folder_name.Text = $"Create the new file in {Tab.Tab.TabName} ?";
                            accept.IsEnabled = true;
                        }
                        break;

                    case ContentType.File:
                        if (CurrentExplorerMode == OnedriveExplorerMode.SelectFile)
                        {
                            CurrentTab = Tab.Tab;
                            folder_name.Text = $"Open the file {Tab.Tab.TabName} ?";
                            accept.IsEnabled = true;
                        }
                        break;
                }

            }
        }

        private void OneDriveLoadStart()
        {
            back_onedrive.IsEnabled = false; reload_onedrive.IsEnabled = false;
            LoadingGrid.Visibility = Visibility.Visible; LoadingGridRing.IsActive = true;
        }

        private void OnedriveLoadFinished()
        {
            back_onedrive.IsEnabled = true; reload_onedrive.IsEnabled = true;
            LoadingGrid.Visibility = Visibility.Collapsed; LoadingGridRing.IsActive = false;
        }

        private async void ReloadOnedrive()
        {
            if (OneDriveNavigationHistory.Count > 0)
            {
                Tabs.Items.Clear();

                var tuple = OneDriveNavigationHistory[OneDriveNavigationHistory.Count - 1]; Tabs.Items.Clear();
                OneDriveLoadStart();
                foreach (var item in await OneDriveNavigate(tuple.Item2))
                {
                    Tabs.Items.Add(item);
                }
                OnedriveLoadFinished();
            }
        }

        /*
         * ONEDRIVE TOOLS
         */

        public async Task<Tuple<string, string, List<OnedriveExplorerItem>>> OneDriveGetRootFolder() //Name, ID, List<InfosTab>
        {
            try
            {

                List<InfosTab> ListTabs = new List<InfosTab>();
                var item = await TabsDataCache.OneDriveClient.Drive.Root.Request().Expand("children").GetAsync();

                return new Tuple<string, string, List<OnedriveExplorerItem>>(item.Name, item.Id, ConvertItemToListTab(item));
            }
            catch { return new Tuple<string, string, List<OnedriveExplorerItem>>("", "", new List<OnedriveExplorerItem>()); }
        }

        public async Task<List<OnedriveExplorerItem>> OneDriveNavigate(string ID)
        {
            try
            {
                List<InfosTab> ListTabs = new List<InfosTab>();
                var item = await TabsDataCache.OneDriveClient.Drive.Items[ID].Request().Expand("children").GetAsync();

                return ConvertItemToListTab(item);
            }
            catch { return new List<OnedriveExplorerItem>(); }
        }

        private List<OnedriveExplorerItem> ConvertItemToListTab(Item ListItem)
        {
            List<OnedriveExplorerItem> ListTabs = new List<OnedriveExplorerItem>();

            foreach (var OnedriveItem in ListItem.Children)
            {

                if (OnedriveItem.Folder == null)
                {
                    //FILE
                    if (OnedriveItem.File != null)
                        if (OnedriveItem.File.MimeType.Contains("text") || OnedriveItem.File.MimeType.Contains("application"))
                        {
                            ListTabs.Add(new OnedriveExplorerItem {
                            IconContent = "",
                            LastModificationDatetime = OnedriveItem.LastModifiedDateTime.Value.ToString(@"MM\/dd\/yyyy HH:mm"),
                            Tab = new InfosTab
                            {
                                TabName = OnedriveItem.Name,
                                TabContentType = ContentType.File,
                                TabOriginalPathContent = OnedriveItem.Id,
                                TabType = LanguagesHelper.GetLanguageType(OnedriveItem.Name),
                                TabDateModified = OnedriveItem.LastModifiedDateTime.ToString(),
                                DateTabContentUpdated = OnedriveItem.LastModifiedDateTime.Value
                            }
                            });
                        }
                }
                else
                {
                    //FOLDER
                    ListTabs.Add(new OnedriveExplorerItem
                    {
                        IconContent = "",
                        LastModificationDatetime = "",
                        Tab = new InfosTab
                        {
                            TabName = OnedriveItem.Name,
                            TabContentType = ContentType.Folder,
                            TabOriginalPathContent = OnedriveItem.Id,
                            TabType = LanguagesHelper.GetLanguageType(OnedriveItem.Name),
                            TabDateModified = OnedriveItem.LastModifiedDateTime.ToString(),
                            DateTabContentUpdated = OnedriveItem.LastModifiedDateTime.Value
                        }
                    });
                }

            }

            return ListTabs;

        }

    }
}
