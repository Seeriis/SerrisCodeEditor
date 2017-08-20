using GalaSoft.MvvmLight.Messaging;
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
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor
{

    public sealed partial class MainPage : Page
    {
        TabsAccessManager manager_access = new TabsAccessManager(); TabsWriteManager manager_writer = new TabsWriteManager();
        int current_list, current_tab;

        /*protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                Tuple<TabsAccessManager, TabsWriteManager> tuple = e.Parameter as Tuple<TabsAccessManager, TabsWriteManager>;
                if(tuple != null)
                {
                    manager_access = tuple.Item1; manager_writer = tuple.Item2;
                    lel();
                }
                else { lel(); }
            }
            else
            {
                lel();
            }
        }*/

        public MainPage()
        {
            this.InitializeComponent();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            lel();
        }

        public async void lel()
        {
            Messenger.Default.Register<STSNotification>(this, (notification) =>
            {
                try
                {
                    if (current_list == notification.ID.ID_TabsList)
                    {

                        switch (notification.Type)
                        {
                            case TypeUpdate.NewTab:
                                list_ids.Items.Add(notification.ID.ID_Tab);
                                break;

                            case TypeUpdate.TabDeleted:
                                list_ids.Items.Remove(notification.ID.ID_Tab);
                                break;

                            case TypeUpdate.NewList:
                                list_ids_list.Items.Add(notification.ID.ID_TabsList);
                                break;

                            case TypeUpdate.ListDeleted:
                                list_ids_list.Items.Remove(notification.ID.ID_TabsList);
                                break;
                        }

                    }
                    else
                        switch (notification.Type)
                        {
                            case TypeUpdate.NewList:
                                list_ids_list.Items.Add(notification.ID.ID_TabsList);
                                break;

                            case TypeUpdate.ListDeleted:
                                list_ids_list.Items.Remove(notification.ID.ID_TabsList);
                                break;
                        }
                }
                catch { }
            });

            var sts_initialize = await manager_access.GetTabsListIDAsync();
            
            if(sts_initialize.Count == 0)
            {
                current_list = await manager_writer.CreateTabsListAsync("Liste des onglets - test");
                List<int> list_ids = await manager_access.GetTabsIDAsync(current_list);
                AddTabs(list_ids);
            }
            else
            {
                current_list = sts_initialize[0];
                List<int> list_ids = await manager_access.GetTabsIDAsync(current_list);
                AddTabs(list_ids);
            }

            list_ids_list.Items.Clear();
            foreach(int id in sts_initialize)
            {
                list_ids_list.Items.Add(id);
            }
        }

        public void AddTabs(List<int> tabs)
        {
            list_ids.Items.Clear();
            foreach(int nmb in tabs)
            {
                list_ids.Items.Add(nmb);
            }
        }

        private async void NewTab_Click(object sender, RoutedEventArgs e)
        {
            await manager_writer.CreateTabAsync(new InfosTab { }, current_list);
        }

        private async void DeleteTab_Click(object sender, RoutedEventArgs e)
        {
            await manager_writer.DeleteTabAsync(new TabID { ID_Tab = int.Parse(id_box.Text), ID_TabsList = current_list });
        }

        private async void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                Frame frame = new Frame();
                frame.Navigate(typeof(MainPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void OpenFiles_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.OpenFilesAndCreateNewTabsFiles(current_list, StorageListTypes.LocalStorage);
        }

        private async void list_ids_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(list_ids.SelectedItem != null)
            {
                current_tab = (int)list_ids.SelectedItem;
                InfosTab tab = await manager_access.GetTabViaIDAsync(new TabID { ID_Tab = current_tab, ID_TabsList = current_list });
                TabName.Text = "Nom: " + tab.TabName;
                TabEncoding.Text = "Encoding: " + Encoding.GetEncoding(tab.TabEncoding).EncodingName;

                ContentViewer.CodeLanguage = tab.TabType.ToUpper();
                ContentViewer.Code = await manager_access.GetTabContentViaIDAsync(new TabID { ID_Tab = current_tab, ID_TabsList = current_list });
            }
        }

        private async void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.CreateNewTab(current_list, "test.json", Encoding.ASCII, StorageListTypes.LocalStorage, "Je suis une patate !");
        }

        private async void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            await manager_writer.DeleteTabsListAsync(current_list);
        }

        private async void NewList_Click(object sender, RoutedEventArgs e)
        {
            await manager_writer.CreateTabsListAsync(name_box.Text);
        }

        private async void list_ids_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_ids_list.SelectedItem != null)
            {
                current_list = (int)list_ids_list.SelectedItem; TabsList tabslist = await manager_access.GetTabsListViaIDAsync(current_list);

                if(tabslist != null)
                    name_box.Text = tabslist.name;

                List<int> list_ids = await manager_access.GetTabsIDAsync(current_list);
                AddTabs(list_ids);
            }

        }

        private async void CreateFileViaTab_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.CreateNewFileViaTab(new TabID { ID_Tab = current_tab, ID_TabsList = current_list });
        }
    }

}
