using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisModulesServer.Manager;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Views
{

    public sealed partial class EditorView : Page
    {
        public EditorView()
        {
            this.InitializeComponent();
        }

        private void EditorViewUI_Loaded(object sender, RoutedEventArgs e)
        { SetMessenger(); SetInterface(); }

        private void EditorViewUI_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (temp_variables.CurrentDevice != CurrentDevice.WindowsMobile)
            {
                if(Toolbar.ActualWidth > FirstBar.ActualWidth - 272)
                {
                    ToolbarWidth.Width = new GridLength(1, GridUnitType.Star);
                    FocusTitlebarWidth.Width = new GridLength(100, GridUnitType.Pixel);
                }
                else
                {
                    ToolbarWidth.Width = new GridLength(1, GridUnitType.Auto);
                    FocusTitlebarWidth.Width = new GridLength(1, GridUnitType.Star);
                }
            }
        }



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private void SetMessenger()
        {
            Messenger.Default.Register<TabSelectedNotification>(this, (notification) =>
            {
                try
                {
                    switch (notification.contactType)
                    {
                        case ContactTypeSCEE.SetCodeForEditor:
                            Queue_Tabs.Add(notification);
                            ManageQueueTabs();
                            break;

                        case ContactTypeSCEE.SetCodeForEditorWithoutUpdate:
                            ContentViewer.CodeLanguage = notification.typeCode; ContentViewer.Code = notification.code;
                            break;
                    }
                }
                catch { }
            });
        }

        private void DeployUIDetector_PointerEntered(object sender, PointerRoutedEventArgs e)
        { UpdateUI(true); }

        private void ContentViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
        { UpdateUI(false); }

        private void UpdateUI(bool isDeployed)
        {
            isUIDeployed = isDeployed;

            if(isUIDeployed)
            {
                PrincipalUI.Visibility = Visibility.Visible; SheetsManager.Visibility = Visibility.Visible;
                SheetViewSplit.DisplayMode = SplitViewDisplayMode.Inline; SheetViewSplit.IsPaneOpen = true;
            }
            else
            {
                PrincipalUI.Visibility = Visibility.Collapsed; SheetsManager.Visibility = Visibility.Collapsed;
                SheetViewSplit.DisplayMode = SplitViewDisplayMode.CompactOverlay; SheetViewSplit.IsPaneOpen = false;

                SheetsManager.SelectTabsListSheet();
            }
        }

        private void SetInterface()
        {
            if (temp_variables.CurrentDevice == CurrentDevice.WindowsMobile)
            { }
            else
            {
                CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

                Window.Current.SetTitleBar(focus_titlebar);
            }

            UpdateUI(false);
        }

        private void ContentViewer_EditorCommands(object sender, SerrisCodeEditorEngine.Items.EventSCEE e)
        {
            if (e.message == "click")
                UpdateUI(false);
        }

        private void ModuleSheetView_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if(!isUIDeployed)
                SheetViewSplit.IsPaneOpen = true;
        }

        private void ModuleSheetView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed)
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength - 15) || e.GetCurrentPoint(MasterGrid).Position.Y <= 62 || e.GetCurrentPoint(MasterGrid).Position.X <= 0)
                    SheetViewSplit.IsPaneOpen = false;

            Debug.WriteLine(e.GetCurrentPoint(MasterGrid).Position.X);
        }

        private void EditorViewUI_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed)
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength - 15) || e.GetCurrentPoint(MasterGrid).Position.Y <= 62 || e.GetCurrentPoint(MasterGrid).Position.X <= 10)
                    SheetViewSplit.IsPaneOpen = false;

        }

        //For manage tabs content
        List<TabSelectedNotification> Queue_Tabs = new List<TabSelectedNotification>(); bool CanManageQueue = true;
        public async void ManageQueueTabs()
        {
            while (!CanManageQueue)
                await Task.Delay(20);

            if (CanManageQueue)
            {
                CanManageQueue = false;

                try
                {
                    if (temp_variables.CurrentIDs.ID_Tab != 0)
                    {
                        string content = await ContentViewer.GetCode();
                        SerrisModulesServer.Manager.AsyncHelpers.RunSync(() => Tabs_manager_writer.PushTabContentViaIDAsync(temp_variables.CurrentIDs, content, false));
                    }
                }
                catch { }

                foreach (CoreApplicationView view in CoreApplication.Views)
                {
                    if (this.Dispatcher != view.Dispatcher)
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabUpdated, ID = temp_variables.CurrentIDs });
                        });
                }

                temp_variables.CurrentIDs = new TabID { ID_Tab = Queue_Tabs[0].tabID, ID_TabsList = Queue_Tabs[0].tabsListID };
                ContentViewer.CodeLanguage = Queue_Tabs[0].typeLanguage; ContentViewer.Code = Queue_Tabs[0].code;

                Queue_Tabs.RemoveAt(0);
                CanManageQueue = true;
            }
        }



        /* =============
         * = VARIABLES =
         * =============
         */



        TabsAccessManager Tabs_manager_access = new TabsAccessManager(); TabsWriteManager Tabs_manager_writer = new TabsWriteManager();
        ModulesAccessManager Modules_manager_access = new ModulesAccessManager(); ModulesWriteManager Modules_manager_writer = new ModulesWriteManager();
        TempContent temp_variables = new TempContent();
        bool isUIDeployed = false;

    }

}
