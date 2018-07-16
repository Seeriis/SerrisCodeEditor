using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public sealed partial class TabsList : UserControl
    {
        bool ComponentLoaded = false;
        public TabID CurrentSelectedIDs;
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        public event EventHandler ListTabSelectionChanged, ListNewTab, ListChanged;
        public event EventHandler<TabID> ListTabDeleted;

        public TabsList()
        {
            this.InitializeComponent();
        }


        /* =============
         * = FUNCTIONS =
         * =============
         */

        private void ListTabs_Loaded(object sender, RoutedEventArgs e)
        {
            if (!ComponentLoaded)
            {
                SetMessenger();
                ComponentLoaded = true;
            }
        }

        private async void ListTabs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListTabs.SelectedItem != null)
            {
                if (((TabID)ListTabs.SelectedItem).ID_Tab != GlobalVariables.CurrentIDs.ID_Tab)
                {
                    CurrentSelectedIDs = (TabID)ListTabs.SelectedItem;
                    var tab = TabsAccessManager.GetTabViaID(CurrentSelectedIDs);

                    if (tab.TabContentType == ContentType.File)
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

                        AppSettings.Values["Tabs_tab-selected-index"] = ((TabID)ListTabs.SelectedItem).ID_Tab;
                        AppSettings.Values["Tabs_list-selected-index"] = ((TabID)ListTabs.SelectedItem).ID_TabsList;

                        ListTabSelectionChanged?.Invoke(this, new EventArgs());
                    }

                }
            }
        }

        private void SetMessenger()
        {
            Messenger.Default.Register<STSNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        if (CurrentSelectedIDs.ID_TabsList == notification.ID.ID_TabsList)
                        {

                            switch (notification.Type)
                            {

                                case TypeUpdateTab.TabDeleted:
                                    object FindItem = ListTabs.Items.SingleOrDefault(o => o.Equals(notification.ID));

                                    if (FindItem != null)
                                    {
                                        ListTabs.Items.Remove(FindItem);

                                        //Auto selection
                                        if (CurrentSelectedIDs.ID_Tab == notification.ID.ID_Tab && ListTabs.Items.Count - 1 >= 0)
                                        {
                                            ListTabs.SelectedIndex = ListTabs.Items.Count - 1;
                                        }
                                    }

                                    ListTabDeleted?.Invoke(this, notification.ID);

                                    break;
                            }

                        }

                    }
                    catch { }

                });

            });

        }



        /* ==============
         * = PARAMETERS =
         * ==============
         */



        public int ListID
        {
            get { return (int)GetValue(ListIDProperty); }
            set
            {
                SetValue(ListIDProperty, value);
                CurrentSelectedIDs.ID_TabsList = value;
            }
        }

        public static readonly DependencyProperty ListIDProperty = DependencyProperty.Register("ListID", typeof(int), typeof(TabsList), null);


    }
}
