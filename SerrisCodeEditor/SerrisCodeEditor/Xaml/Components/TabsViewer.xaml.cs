using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
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
        { SetMessenger(); }



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
                foreach (int id in await access_manager.GetTabsListIDAsync())
                {
                    var list = await access_manager.GetTabsListViaIDAsync(id);
                    Lists.Items.Add(new ListItem { ListID = list.ID, ListName = list.name });
                }

                if (Lists.Items.Count == 0)
                {
                    await write_manager.CreateTabsListAsync("Default list");
                }
                else
                    Lists.SelectedIndex = 0;

                isLoaded = true;
            }

        }

        private async void Tabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Tabs.SelectedItem != null)
            {
                CurrentSelectedIDs = (TabID)Tabs.SelectedItem;
                var tab = await access_manager.GetTabViaIDAsync(CurrentSelectedIDs);
                Messenger.Default.Send(new TabSelectedNotification { tabID = CurrentSelectedIDs.ID_Tab, tabsListID = CurrentSelectedIDs.ID_TabsList, code = await access_manager.GetTabContentViaIDAsync(CurrentSelectedIDs), contactType = ContactTypeSCEE.SetCodeForEditor, typeLanguage = tab.TabType.ToUpper(), typeCode = Encoding.GetEncoding(tab.TabEncoding).EncodingName });
            }

        }

        private void SetMessenger()
        {
            Messenger.Default.Register<STSNotification>(this, async (notification) =>
            {
                try
                {
                    if (CurrentSelectedIDs.ID_TabsList == notification.ID.ID_TabsList)
                    {

                        switch (notification.Type)
                        {
                            case TypeUpdateTab.NewTab:
                                Tabs.Items.Add(notification.ID);
                                break;

                            case TypeUpdateTab.TabDeleted:
                                int i = 0;
                                foreach(TabID item in Tabs.Items)
                                {
                                    if(item.ID_Tab == notification.ID.ID_Tab)
                                    {
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

        }

        private async void ChangeSelectedList(int id_list)
        {
            Tabs.Items.Clear();
            CurrentSelectedIDs.ID_TabsList = id_list;
            List<int> list_ids = await access_manager.GetTabsIDAsync(id_list);
            
            foreach(int id in list_ids)
            {
                Tabs.Items.Add(new TabID { ID_Tab = id, ID_TabsList = id_list });
            }

        }

        private void Box_Search_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private async void OpenButton_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.OpenFilesAndCreateNewTabsFiles(CurrentSelectedIDs.ID_TabsList, StorageListTypes.LocalStorage);
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        { await write_manager.DeleteTabsListAsync(CurrentSelectedIDs.ID_TabsList); FlyoutDeleteList.Hide(); }

        private async void NewList_Click(object sender, RoutedEventArgs e)
        { await write_manager.CreateTabsListAsync(TextBoxNewList.Text); TextBoxNewList.Text = ""; FlyoutNewList.Hide(); }



        /* =============
         * = VARIABLES =
         * =============
         */



        public TabID CurrentSelectedIDs; bool isLoaded = false;
        TabsAccessManager access_manager = new TabsAccessManager(); TabsWriteManager write_manager = new TabsWriteManager();

    }
}
