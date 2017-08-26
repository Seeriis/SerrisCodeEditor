using GalaSoft.MvvmLight.Messaging;
using SerrisCodeEditor.Items;
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
        TabID IDs = new TabID();

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

        public void PushCodeViaIDs()
        {
            Messenger.Default.Send(new ContactSCEE { IDs = IDs, Code = ContentViewer.Code, ContactType = ContactTypeSCEE.GetCodeForTab });
        }

        public async void lel()
        {
            Messenger.Default.Register<STSNotification>(this, (notification) =>
            {
                try
                {
                    if (IDs.ID_TabsList == notification.ID.ID_TabsList)
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

            Messenger.Default.Register<ContactSCEE>(this, (notification) =>
            {
                try
                {
                    if (notification.ContactType == ContactTypeSCEE.SetCodeForEditor)
                    {
                        IDs = notification.IDs;
                        ContentViewer.CodeLanguage = notification.TypeCode; ContentViewer.Code = notification.Code;
                    }
                }
                catch { }
            });

            var sts_initialize = await manager_access.GetTabsListIDAsync();
            
            if(sts_initialize.Count == 0)
            {
                IDs.ID_TabsList = await manager_writer.CreateTabsListAsync("Liste des onglets - test");
                List<int> list_ids = await manager_access.GetTabsIDAsync(IDs.ID_TabsList);
                AddTabs(list_ids);
            }
            else
            {
                IDs.ID_TabsList = sts_initialize[0];
                List<int> list_ids = await manager_access.GetTabsIDAsync(IDs.ID_TabsList);
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
            await manager_writer.CreateTabAsync(new InfosTab { }, IDs.ID_TabsList);
        }

        private async void DeleteTab_Click(object sender, RoutedEventArgs e)
        {
            await manager_writer.DeleteTabAsync(new TabID { ID_Tab = int.Parse(id_box.Text), ID_TabsList = IDs.ID_TabsList });
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
            await creator.OpenFilesAndCreateNewTabsFiles(IDs.ID_TabsList, StorageListTypes.LocalStorage);
        }

        private async void list_ids_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(list_ids.SelectedItem != null)
            {
                IDs.ID_Tab = (int)list_ids.SelectedItem;
                InfosTab tab = await manager_access.GetTabViaIDAsync(IDs);
                TabName.Text = "Nom: " + tab.TabName;
                TabEncoding.Text = "Encoding: " + Encoding.GetEncoding(tab.TabEncoding).EncodingName;

                ContentViewer.CodeLanguage = tab.TabType.ToUpper();
                ContentViewer.Code = await manager_access.GetTabContentViaIDAsync(IDs);
            }
        }

        private async void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.CreateNewTab(IDs.ID_TabsList, "test.json", Encoding.ASCII, StorageListTypes.LocalStorage, "Je suis une patate !");
        }

        private async void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            await manager_writer.DeleteTabsListAsync(IDs.ID_TabsList);
        }

        private async void NewList_Click(object sender, RoutedEventArgs e)
        {
            await manager_writer.CreateTabsListAsync(name_box.Text);
        }

        private async void list_ids_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_ids_list.SelectedItem != null)
            {
                IDs.ID_TabsList = (int)list_ids_list.SelectedItem; TabsList tabslist = await manager_access.GetTabsListViaIDAsync(IDs.ID_TabsList);

                if(tabslist != null)
                    name_box.Text = tabslist.name;

                List<int> list_ids = await manager_access.GetTabsIDAsync(IDs.ID_TabsList);
                AddTabs(list_ids);
            }

        }

        private async void CreateFileViaTab_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.CreateNewFileViaTab(IDs);
        }
    }

}
