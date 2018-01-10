using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
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
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public sealed partial class Tab : UserControl
    {
        InfosTab current_tab = new InfosTab(); int current_list; bool infos_opened = false, enable_selection = false;
        TabsAccessManager access_manager = new TabsAccessManager(); TabsWriteManager write_manager = new TabsWriteManager();

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

                if (current_tab == null)
                    current_tab = new InfosTab();

                current_tab.ID = ids.ID_Tab; current_list = ids.ID_TabsList;
                UpdateTabInformations();
            }
        }

        private void TabComponent_Loaded(object sender, RoutedEventArgs e)
        {
            SetMessenger();
            //UpdateTabInformations();
        }

        private void TabComponent_PointerEntered(object sender, PointerRoutedEventArgs e)
        { ShowPath.Begin(); }

        private void TabComponent_PointerExited(object sender, PointerRoutedEventArgs e)
        { ShowName.Begin(); }



        /*
         * =============
         * = FUNCTIONS =
         * =============
         */



        private void SetMessenger()
        {
            Messenger.Default.Register<STSNotification>(this, async (nm) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
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
                                    await write_manager.PushTabContentViaIDAsync(new TabID { ID_Tab = current_tab.ID, ID_TabsList = current_list }, current_tab.TabContentTemporary, true);
                                    break;
                            }
                        }

                    }
                    catch { }

                });

            });
        }

        private async void UpdateTabInformations()
        {
            //Set temp tab + tabs list ID
            try
            {
                current_tab = await access_manager.GetTabViaIDAsync(new TabID { ID_Tab = current_tab.ID, ID_TabsList = current_list });

                foreach(CoreApplicationView view in CoreApplication.Views)
                {
                    Extension_tab.Text = current_tab.TabType.ToUpper();
                    name_tab.Text = current_tab.TabName;
                    path_tab.Text = current_tab.PathContent;
                    encoding_file.Text = Encoding.GetEncoding(current_tab.TabEncoding).EncodingName;
                }
                //image_tab.Source = current_tab.;

                //SET INFOS
                //default_type = infos_list.GetExtension(current_tab.type).ToUpper(); list_types.SelectedItem = infos_list.GetExtension(current_tab.type).ToUpper();
            }
            catch { }
        }

        private void Close_Tab_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new STSNotification { ID = new TabID { ID_Tab = current_tab.ID, ID_TabsList = current_list }, Type = TypeUpdateTab.TabDeleted });
        }

        private void list_types_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void More_Tab_Click(object sender, RoutedEventArgs e)
        {
            if (infos_opened)
            {
                RemoveInfos.Begin(); infos_opened = false;
            }
            else
            {
                enable_selection = false;
                try
                {
                    StorageFile file = await StorageFile.GetFileFromPathAsync(current_tab.PathContent);
                    BasicProperties properties = await file.GetBasicPropertiesAsync();

                    if (properties.Size != 0)
                    {

                        if (properties.Size > 1024f) //Ko
                        {
                            size_file.Text = String.Format("{0:0.00}", (properties.Size / 1024f)) + " Ko";

                            if ((properties.Size / 1024f) > 1024f) //Mo
                            {
                                size_file.Text = String.Format("{0:0.00}", ((properties.Size / 1024f) / 1024f)) + " Mo";
                            }
                        }
                        else //Octect
                        {
                            size_file.Text = properties.Size + " Octect(s)";
                        }

                    }

                    modified_file.Text = properties.DateModified.ToString();
                    created_file.Text = file.DateCreated.ToString();
                }
                catch { }

                ShowInfos.Begin(); infos_opened = true; enable_selection = true;
            }

        }
    }
}
