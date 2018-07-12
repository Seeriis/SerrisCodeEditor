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
using Windows.UI.Input;
using SerrisModulesServer.Type.Theme;
using Windows.UI.Xaml.Media;

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

            PointerMoved += EditorView_PointerMoved;
            PointerReleased += SeparatorLinePointerReleased;
        }

        private void EditorViewUI_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (GlobalVariables.CurrentDevice != CurrentDevice.WindowsMobile)
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
                        SetMonacoTheme();
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
                        if(notification_settings.SettingsUpdatedName == DefaultSettings.DefaultSettingsMenuList[0].Name && EditorIsLoaded) //If settings updated for Editor, then...
                        {
                            LoadSettings();
                        } else if (notification_settings.SettingsUpdatedName == DefaultSettings.DefaultSettingsMenuList[1].Name)
                        {
                            SetInterface();
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
                                await TabsWriteManager.PushTabContentViaIDAsync(GlobalVariables.CurrentIDs, content, false);
                                Messenger.Default.Send(new SCEENotification { type = SCEENotifType.SaveCurrentTab, answerNotification = true });
                                ChangePushed = false;
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
        => UpdateUI(!isUIDeployed, false);

        private void ContentViewer_PointerPressed(object sender, PointerRoutedEventArgs e)
        => UpdateUI(false, false);

        private void UpdateUI(bool isDeployed, bool ForceUpdate)
        {
            if(isUIDeployed != isDeployed || ForceUpdate)
            {
                isUIDeployed = isDeployed;

                if (isDeployed)
                {
                    SheetViewSeparatorLine.Width = 8;

                    switch (GlobalVariables.CurrentDevice)
                    {
                        case CurrentDevice.Desktop:
                            if (AppSettings.Values.ContainsKey("ui_extendedview"))
                            {
                                if (!(bool)AppSettings.Values["ui_extendedview"])
                                {
                                    DeployUIIconDeploying.Begin();
                                }
                            }
                            else
                            {
                                DeployUIIconDeploying.Begin();
                            }

                            PrincipalUI.Visibility = Visibility.Visible;
                            break;
                    }

                    SheetViewSplit.DisplayMode = SplitViewDisplayMode.Inline; SheetViewSplit.IsPaneOpen = true;
                    SheetsManager.Visibility = Visibility.Visible;
                    Messenger.Default.Send(SheetViewMode.Deployed);
                }
                else
                {
                    SheetViewSeparatorLine.Width = 0;

                    switch (GlobalVariables.CurrentDevice)
                    {
                        case CurrentDevice.Desktop:

                            if (AppSettings.Values.ContainsKey("ui_extendedview"))
                            {
                                if (!(bool)AppSettings.Values["ui_extendedview"])
                                {
                                    PrincipalUI.Visibility = Visibility.Visible;
                                    DeployUIIconCollapsing.Begin();
                                }
                                else
                                    PrincipalUI.Visibility = Visibility.Collapsed;
                            }
                            else
                            {
                                PrincipalUI.Visibility = Visibility.Visible;
                                DeployUIIconCollapsing.Begin();
                            }

                            SheetViewSplit.DisplayMode = SplitViewDisplayMode.CompactOverlay;
                            break;

                        case CurrentDevice.WindowsMobile:
                            SheetViewSplit.DisplayMode = SplitViewDisplayMode.Inline;
                            break;
                    }

                    SheetsManager.Visibility = Visibility.Collapsed;
                    SheetViewSplit.IsPaneOpen = false;
                    SheetsManager.SelectTabsListSheet();

                    Messenger.Default.Send(SheetViewMode.Minimized);
                }

            }
        }

        private void SetTheme()
        {
            DeployUIDetectorBG.Fill = GlobalVariables.CurrentTheme.SecondaryColor;
            DeployUIDetectorBG.Stroke = GlobalVariables.CurrentTheme.SecondaryColorFont;
            DeployUIIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            DeployUIDetectorB.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            DeployUIIconB.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            BackgroundPrinciapalUI.ImageSource = GlobalVariables.CurrentTheme.BackgroundImage;
            ColorPrincipalUI.Fill = GlobalVariables.CurrentTheme.MainColor;

            BackgroundSheetView.ImageSource = GlobalVariables.CurrentTheme.BackgroundImage;
            ColorSheetView.Fill = GlobalVariables.CurrentTheme.MainColor;

            SheetViewSeparatorLine.Fill = GlobalVariables.CurrentTheme.MainColor;

            SolidColorBrush ColorSettingsButtons = new SolidColorBrush(Windows.UI.Color.FromArgb(150, GlobalVariables.CurrentTheme.SecondaryColor.Color.R, GlobalVariables.CurrentTheme.SecondaryColor.Color.G, GlobalVariables.CurrentTheme.SecondaryColor.Color.B));

            SettingsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            NotificationsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            DownloadButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

            switch (GlobalVariables.CurrentDevice)
            {
                case CurrentDevice.Desktop:
                    ApplicationViewTitleBar TitleBar = ApplicationView.GetForCurrentView().TitleBar;
                    TitleBar.ButtonBackgroundColor = Colors.Transparent;
                    TitleBar.ButtonForegroundColor = GlobalVariables.CurrentTheme.MainColorFont.Color;
                    TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    TitleBar.ButtonInactiveForegroundColor = GlobalVariables.CurrentTheme.MainColorFont.Color;
                    break;

                case CurrentDevice.WindowsMobile:
                    var StatusBarSettings = StatusBar.GetForCurrentView();
                    StatusBarSettings.ForegroundColor = GlobalVariables.CurrentTheme.MainColorFont.Color;
                    StatusBarSettings.BackgroundColor = GlobalVariables.CurrentTheme.MainColor.Color;
                    StatusBarSettings.BackgroundOpacity = 1;
                    break;
            }

        }

        private async void SetMonacoTheme()
        {
            if (EditorIsLoaded)
            {
                var MonacoTheme = ModulesAccessManager.GetModuleViaID(ModulesAccessManager.GetCurrentThemeMonacoID());
                ContentViewer.SendAndExecuteJavaScript(await new ThemeReader(ModulesAccessManager.GetCurrentThemeMonacoID()).GetThemeJSContentAsync());
                ContentViewer.SendAndExecuteJavaScript("monaco.editor.setTheme('" + MonacoTheme.ModuleMonacoThemeName + "');");

                //Debug.WriteLine(MonacoTheme.ModuleMonacoThemeName);
            }
        }

        private void LoadSettings()
        {

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

            //FONT FAMILY
            if (AppSettings.Values.ContainsKey("editor_fontfamily"))
                ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ fontFamily: '" + (string)AppSettings.Values["editor_fontfamily"] + "' });");

            //FONT SIZE
            if (AppSettings.Values.ContainsKey("editor_fontsize"))
                ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ fontSize: " + (int)AppSettings.Values["editor_fontsize"] + " });");

        }

        private async void ExecuteModulesFunction()
        {
            //onEditorViewReady
            foreach (InfosModule Module in ModulesAccessManager.GetModules(true))
            {
                if(Module.IsEnabled && Module.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon)
                {
                    SCEELibs.SCEELibs Libs = new SCEELibs.SCEELibs(Module.ID);
                    await Task.Run(() => new AddonExecutor(Module.ID, Libs).ExecuteDefaultFunction(AddonExecutorFuncTypes.onEditorViewReady));
                }
            }
        }

        private async void SetInterface()
        {

            switch(GlobalVariables.CurrentDevice)
            {
                case CurrentDevice.WindowsMobile:
                    //UI modification for mobile
                    PrincipalUI.VerticalAlignment = VerticalAlignment.Bottom;
                    ColorPrincipalUI.VerticalAlignment = VerticalAlignment.Bottom;
                    BackgroundPrincipalUIControl.VerticalAlignment = VerticalAlignment.Bottom;
                    Console.Height = 30;
                    SheetViewSplit.OpenPaneLength = MasterGrid.ActualWidth - 40;
                    Grid.SetColumnSpan(Toolbar, 1);
                    //Toolbar.HorizontalAlignment = HorizontalAlignment.Stretch;
                    TextInfoTitlebar.Margin = new Thickness(0);
                    ContentViewerGrid.Margin = new Thickness(0, 0, 0, 75);
                    TextInfoTitlebar.Visibility = Visibility.Collapsed;
                    TopSheetViewSplit.Visibility = Visibility.Collapsed;

                    StatusBar.GetForCurrentView().ProgressIndicator.Text = "Serris Code Editor MLV";
                    UpdateUI(true, true);
                    break;

                case CurrentDevice.Desktop:

                    if (AppSettings.Values.ContainsKey("ui_extendedview"))
                    {
                        if (!(bool)AppSettings.Values["ui_extendedview"])
                        {
                            ContentViewerGrid.Margin = new Thickness(60, ColorPrincipalUI.ActualHeight, 0, 0);
                            DeployUIDetector.Visibility = Visibility.Visible;
                            BackgroundPrincipalUIControl.Color = Colors.Transparent;
                        }
                        else
                        {
                            ContentViewerGrid.Margin = new Thickness(60, 0, 0, 0);
                            DeployUIDetector.Visibility = Visibility.Collapsed;
                            BackgroundPrincipalUIControl.Color = Colors.Black;
                        }
                    }
                    else
                    {
                        ContentViewerGrid.Margin = new Thickness(60, ColorPrincipalUI.ActualHeight, 0, 0);
                        BackgroundPrincipalUIControl.Color = Colors.Transparent;
                    }

                    //CLOSE PANEL AUTOMATICALLY
                    if (AppSettings.Values.ContainsKey("ui_closepanelauto"))
                    {
                        ClosePanelAuto = (bool)AppSettings.Values["ui_closepanelauto"];
                    }

                    if (AppSettings.Values.ContainsKey("ui_leftpanelength"))
                    {
                        TopSheetViewSplit.Width = (int)AppSettings.Values["ui_leftpanelength"];
                        SheetViewSplit.CompactPaneLength = (int)AppSettings.Values["ui_leftpanelength"];
                        ContentViewerGrid.Margin = new Thickness((int)AppSettings.Values["ui_leftpanelength"], ContentViewerGrid.Margin.Top, 0, 0);
                    }


                    TextInfoTitlebar.Text = "Serris Code Editor - " + SCEELibs.SCEInfos.versionName;
                    CoreApplicationViewTitleBar coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
                    CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

                    Window.Current.SetTitleBar(focus_titlebar);
                    UpdateUI(false, true);
                    break;
            }

            if(!EditorStartModulesEventsLaunched)
            {
                //onEditorStart
                foreach (InfosModule Module in ModulesAccessManager.GetModules(true))
                {
                    if (Module.IsEnabled && Module.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon)
                    {
                        SCEELibs.SCEELibs Libs = new SCEELibs.SCEELibs(Module.ID);
                        await Task.Run(() => new AddonExecutor(Module.ID, Libs).ExecuteDefaultFunction(AddonExecutorFuncTypes.onEditorStart));
                    }
                }

                EditorStartModulesEventsLaunched = true;
            }

        }

        private void ContentViewer_EditorCommands(object sender, SerrisCodeEditorEngine.Items.EventSCEE e)
        {
            switch(e.message)
            {
                case "click":
                    UpdateUI(false, false);
                    break;

                case "change":
                    if(!ChangePushed)
                    {
                        Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabNewModifications, ID = GlobalVariables.CurrentIDs });
                        ChangePushed = true;
                    }
                    break;
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
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength - 15) || e.GetCurrentPoint(MasterGrid).Position.Y <= 75 || e.GetCurrentPoint(MasterGrid).Position.X <= 0)
                {
                    SheetViewSplit.IsPaneOpen = false;
                }
            }

            //Debug.WriteLine(e.GetCurrentPoint(MasterGrid).Position.Y);
        }

        private void SheetsManager_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (ClosePanelAuto && isUIDeployed)
            {
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength + 5) || e.GetCurrentPoint(MasterGrid).Position.X <= 0)
                {
                    UpdateUI(false, false);
                }
            }
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
                //SheetsManager.AddTabsListSheet();
                SetMonacoTheme();
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
                    if (GlobalVariables.CurrentIDs.ID_Tab != 0)
                    {
                        string content = await ContentViewer.GetCode();
                        SerrisModulesServer.Manager.AsyncHelpers.RunSync(() => TabsWriteManager.PushTabContentViaIDAsync(GlobalVariables.CurrentIDs, content, false));
                    }
                }
                catch { }

                foreach (CoreApplicationView view in CoreApplication.Views)
                {
                    if (Dispatcher != view.Dispatcher)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabUpdated, ID = GlobalVariables.CurrentIDs });
                        });
                    }
                }

                GlobalVariables.CurrentIDs = new TabID { ID_Tab = Queue_Tabs[0].tabID, ID_TabsList = Queue_Tabs[0].tabsListID };
                ContentViewer.CodeLanguage = Queue_Tabs[0].typeLanguage;
                ContentViewer.Code = Queue_Tabs[0].code;
                ChangePushed = false;

                Queue_Tabs.RemoveAt(0);
                CanManageQueue = true;
            }
        }

        private void SeparatorLinePointerPressed(object sender, PointerRoutedEventArgs e)
        {
            SeparatorClicked = true;

            if(OpenPaneLengthOriginal == 0)
                OpenPaneLengthOriginal = SheetViewSplit.OpenPaneLength;

            //var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
            //SheetViewSplit.OpenPaneLength = pointerPosition.X + 4;

        }
        private void EditorView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (SeparatorClicked)
            {
                var pointerPosition = e.GetCurrentPoint(MasterGrid);

                if(pointerPosition.Position.X + 4 > OpenPaneLengthOriginal)
                    SheetViewSplit.OpenPaneLength = pointerPosition.Position.X + 4;
            }
        }

        private void SeparatorLinePointerReleased(object sender, PointerRoutedEventArgs e)
        => SeparatorClicked = false;



        /* =============
         * = VARIABLES =
         * =============
         */



        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        bool isUIDeployed = false, EditorIsLoaded = false, ClosePanelAuto = false, SeparatorClicked = false, EditorStartModulesEventsLaunched = false, ChangePushed = false;
        double OpenPaneLengthOriginal = 0;

    }

}
