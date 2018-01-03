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
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.Helpers;
using SerrisCodeEditor.Functions.Settings;
using Windows.Storage;
using SerrisModulesServer.Items;
using SerrisModulesServer.Type.Addon;
using System.Text;

namespace SerrisCodeEditor.Xaml.Views
{

    public sealed partial class EditorView : Page
    {
        public EditorView()
        {
            InitializeComponent();
        }

        private void EditorViewUI_Loaded(object sender, RoutedEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            SetMessenger();
            SetTheme();
            SetInterface();
        }

        private void EditorViewUI_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (temp_variables.CurrentDevice != CurrentDevice.WindowsMobile)
            {
                if (Toolbar.ActualWidth > FirstBar.ActualWidth - 272)
                {
                    ToolbarWidth.Width = new GridLength(1, GridUnitType.Star);
                    FocusTitlebarWidth.Width = new GridLength(272, GridUnitType.Pixel);
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
            Messenger.Default.Register<TabSelectedNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
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
            });

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

            Messenger.Default.Register<SettingsNotification>(this, async (notification_settings) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        if(notification_settings.SettingsUpdatedName == new DefaultSettings().DefaultSettingsMenuList[0].Name && EditorIsLoaded) //If settings updated for Editor, then...
                        {
                            LoadSettings();
                        }
                    }
                    catch { }

                });

            });

            Messenger.Default.Register<SCEENotification>(this, async (notification_scee) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    try
                    {
                        switch (notification_scee.type)
                        {
                            case SCEENotifType.Injection:
                                ContentViewer.SendAndExecuteJavaScript((string)notification_scee.content);
                                break;

                            case SCEENotifType.SaveCurrentTab when !notification_scee.answerNotification:
                                string content = await ContentViewer.GetCode();
                                await Tabs_manager_writer.PushTabContentViaIDAsync(temp_variables.CurrentIDs, content, false);
                                Messenger.Default.Send(new SCEENotification { type = SCEENotifType.SaveCurrentTab, answerNotification = true });
                                break;

                            case SCEENotifType.InjectionAndReturn when !notification_scee.answerNotification:
                                Messenger.Default.Send(new SCEENotification { type = SCEENotifType.InjectionAndReturn, answerNotification = true, content = await ContentViewer.SendAndExecuteJavaScriptWithReturn((string)notification_scee.content) });
                                break;
                        }
                    }
                    catch { }
                });

            });
        }

        private void DeployUIDetector_PointerEntered(object sender, PointerRoutedEventArgs e)
        => UpdateUI(true);

        private void ContentViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
        => UpdateUI(false);

        private void UpdateUI(bool isDeployed)
        {
            isUIDeployed = isDeployed;

            if (isDeployed)
            {
                SheetViewSeparatorLine.Width = 2;
                PrincipalUI.Visibility = Visibility.Visible; SheetsManager.Visibility = Visibility.Visible;
                SheetViewSplit.DisplayMode = SplitViewDisplayMode.Inline; SheetViewSplit.IsPaneOpen = true;

                Messenger.Default.Send(SheetViewMode.Deployed);
            }
            else
            {
                SheetViewSeparatorLine.Width = 0;
                PrincipalUI.Visibility = Visibility.Collapsed; SheetsManager.Visibility = Visibility.Collapsed;
                SheetViewSplit.DisplayMode = SplitViewDisplayMode.CompactOverlay; SheetViewSplit.IsPaneOpen = false;
                SheetsManager.SelectTabsListSheet();

                Messenger.Default.Send(SheetViewMode.Minimized);
            }
        }

        private void SetTheme()
        {
            DeployUIDetector.Background = temp_variables.CurrentTheme.MainColor;
            DeployUIIcon.Foreground = temp_variables.CurrentTheme.MainColorFont;

            BackgroundPrinciapalUI.ImageSource = temp_variables.CurrentTheme.BackgroundImage;
            ColorPrincipalUI.Fill = temp_variables.CurrentTheme.MainColor;

            BackgroundSheetView.ImageSource = temp_variables.CurrentTheme.BackgroundImage;
            ColorSheetView.Fill = temp_variables.CurrentTheme.MainColor;

            if (temp_variables.CurrentDevice == CurrentDevice.Desktop)
            {
                ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
                TitleBar.ButtonBackgroundColor = Colors.Transparent;
                TitleBar.ButtonForegroundColor = temp_variables.CurrentTheme.MainColorFont.Color;
            }
        }

        private void LoadSettings()
        {
            ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

            //LINE NUMBERS
            if (AppSettings.Values.ContainsKey("editor_linenumbers"))
            {
                if((bool)AppSettings.Values["editor_linenumbers"])
                {
                    ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ lineNumbers: true });");
                }
                else
                {
                    ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ lineNumbers: false });");
                }
            }

            //WRAPPING CODE
            if (AppSettings.Values.ContainsKey("editor_wordwrap"))
            {
                if ((bool)AppSettings.Values["editor_wordwrap"])
                {
                    ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ wordWrap: 'wordWrapColumn', wordWrapMinified: true });");
                }
                else
                {
                    ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ wordWrap: 'none', wordWrapMinified: false });");
                }
            }

        }

        private async void ExecuteModulesFunction()
        {
            foreach(InfosModule Module in await Modules_manager_access.GetModulesAsync(true))
            {
                if(Module.IsEnabled && Module.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon)
                {
                    SCEELibs.SCEELibs Libs = new SCEELibs.SCEELibs(Module.ID);
                    await Task.Run(() => new AddonExecutor(Module.ID, Libs).ExecuteDefaultFunction(AddonExecutorFuncTypes.onEditorViewReady));
                }
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
            {
                UpdateUI(false);
            }
        }

        private void ModuleSheetView_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed)
            {
                SheetViewSplit.IsPaneOpen = true;
            }
        }

        private void ModuleSheetView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed)
            {
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength - 15) || e.GetCurrentPoint(MasterGrid).Position.Y <= 70 || e.GetCurrentPoint(MasterGrid).Position.X <= 0)
                {
                    SheetViewSplit.IsPaneOpen = false;
                }
            }

            //Debug.WriteLine(e.GetCurrentPoint(MasterGrid).Position.Y);
        }

        private void ContentViewerGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed)
            {
                SheetViewSplit.IsPaneOpen = false;
            }
        }

        private void EditorViewUI_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed)
            {
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength - 15) || e.GetCurrentPoint(MasterGrid).Position.Y <= 62 || e.GetCurrentPoint(MasterGrid).Position.X <= 10)
                {
                    SheetViewSplit.IsPaneOpen = false;
                }
            }
        }

        private void ContentViewer_EditorLoaded(object sender, EventArgs e)
        {
            if(!EditorIsLoaded)
            {
                LoadSettings();
                ExecuteModulesFunction();
                EditorIsLoaded = true;
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            FrameSettings.Navigate(typeof(SettingsManager));
        }

        //For manage tabs content
        List<TabSelectedNotification> Queue_Tabs = new List<TabSelectedNotification>();
        bool CanManageQueue = true;
        public async void ManageQueueTabs()
        {
            while (!CanManageQueue)
            {
                await Task.Delay(20);
            }

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
                    if (Dispatcher != view.Dispatcher)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabUpdated, ID = temp_variables.CurrentIDs });
                        });
                    }
                }

                temp_variables.CurrentIDs = new TabID { ID_Tab = Queue_Tabs[0].tabID, ID_TabsList = Queue_Tabs[0].tabsListID };
                ContentViewer.CodeLanguage = Queue_Tabs[0].typeLanguage;
                ContentViewer.Code = Queue_Tabs[0].code;

                Queue_Tabs.RemoveAt(0);
                CanManageQueue = true;
            }
        }



        /* =============
         * = VARIABLES =
         * =============
         */



        TabsAccessManager Tabs_manager_access = new TabsAccessManager();
        TabsWriteManager Tabs_manager_writer = new TabsWriteManager();
        ModulesAccessManager Modules_manager_access = new ModulesAccessManager();
        ModulesWriteManager Modules_manager_writer = new ModulesWriteManager();
        TempContent temp_variables = new TempContent();
        bool isUIDeployed = false, EditorIsLoaded = false;

    }

}
