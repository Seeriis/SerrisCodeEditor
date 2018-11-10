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
using SerrisCodeEditorEngine.Items;
using Windows.UI.Xaml.Navigation;
using SerrisCodeEditor.Functions.News;

namespace SerrisCodeEditor.Xaml.Views
{

    public sealed partial class EditorView : Page
    {
        /* =============
         * = VARIABLES =
         * =============
         */

        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        bool isUIDeployed = false, EditorIsLoaded = false, ClosePanelAuto = true, SeparatorClicked = false, EditorStartModulesEventsLaunched = false, ChangePushed = false, FilesWasOpened = false, AutoDeployerEnabled = true, SheetViewerPinned = false, EditorIsReady = false;
        double OpenPaneLengthOriginal = 0;
        string TitlebarText = $"Serris Code Editor ( {SCEELibs.SCEInfos.versionName} )";
        IReadOnlyList<IStorageItem> OpenedFiles;



        public EditorView()
        {
            //FOR TESTING ONLY !
            //Windows.Globalization.ApplicationLanguages.PrimaryLanguageOverride = "en-US";

            InitializeComponent();
            Application.Current.Suspending += Current_Suspending;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var args = e.Parameter as Windows.ApplicationModel.Activation.IActivatedEventArgs;

            if (args != null)
            {

                if (args.Kind == Windows.ApplicationModel.Activation.ActivationKind.File)
                {
                    var fileArgs = args as Windows.ApplicationModel.Activation.FileActivatedEventArgs;

                    if(EditorIsReady)
                    {
                        await TabsCreatorAssistant.OpenFilesAlreadyOpenedAndCreateNewTabsFiles(GlobalVariables.CurrentIDs.ID_TabsList, fileArgs.Files);
                    }
                    else
                    {
                        OpenedFiles = fileArgs.Files;
                        FilesWasOpened = true;
                    }

                }

            }
        }

        private void EditorViewUI_Loaded(object sender, RoutedEventArgs e)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            SetMessenger();
            SetInterface();
            SetTheme();

            //Show bonjour view ?
            if(!SCEELibs.SCEInfos.preReleaseVersion)
            {
                if (AppSettings.Values.ContainsKey("version_sce"))
                {
                    if ((string)AppSettings.Values["version_sce"] != SCEELibs.SCEInfos.versionNumber)
                    {
                        ShowBonjourView();
                    }
                }
                else
                {
                    ShowBonjourView();
                }
            }
            
            PointerMoved += EditorView_PointerMoved;
            PointerReleased += SeparatorLinePointerReleased;
        }

        private async void Current_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();

            try
            {
                if (GlobalVariables.CurrentIDs.ID_Tab != 0)
                {
                    //Code content
                    string content = await ContentViewer.GetCode();
                    await TabsWriteManager.PushTabContentViaIDAsync(GlobalVariables.CurrentIDs, content, false);

                    //Cursor position
                    PositionSCEE CursorPosition = await ContentViewer.GetCursorPosition();
                    InfosTab Tab = TabsAccessManager.GetTabViaID(GlobalVariables.CurrentIDs);
                    Tab.TabCursorPosition = new CursorPosition { column = CursorPosition.column, row = CursorPosition.row };
                    await TabsWriteManager.PushUpdateTabAsync(Tab, GlobalVariables.CurrentIDs.ID_TabsList, false);

                    deferral.Complete();
                }
                else
                {
                    deferral.Complete();
                }
            }
            catch
            {
                deferral.Complete();
            }
        }

        bool ToolbarCanBeResized = true;
        int RightWidth = 200;
        private void EditorViewUI_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (GlobalVariables.CurrentDevice != CurrentDevice.WindowsMobile)
            {
                if(Toolbar.IsScrollable)
                {
                    ToolbarWidth.Width = new GridLength(1, GridUnitType.Star);
                    FocusTitlebarWidth.Width = new GridLength(RightWidth, GridUnitType.Pixel);
                    TextInfoTitlebar.Visibility = Visibility.Collapsed;
                    ToolbarCanBeResized = false;
                }
                else
                {
                    if (Toolbar.ActualWidth > (FirstBar.ActualWidth - RightWidth) && ToolbarCanBeResized)
                    {
                        ToolbarWidth.Width = new GridLength(1, GridUnitType.Star);
                        FocusTitlebarWidth.Width = new GridLength(RightWidth, GridUnitType.Pixel);
                        TextInfoTitlebar.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        ToolbarWidth.Width = new GridLength(1, GridUnitType.Auto);
                        FocusTitlebarWidth.Width = new GridLength(1, GridUnitType.Star);
                        TextInfoTitlebar.Visibility = Visibility.Visible;

                        if (Toolbar.IsScrollable)
                            ToolbarCanBeResized = false;
                        else if(((FirstBar.ActualWidth - RightWidth) - Toolbar.ActualWidth) > 5)
                            ToolbarCanBeResized = true;
                    }
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

                                if (GlobalVariables.CurrentDevice == SCEELibs.Editor.CurrentDevice.Desktop)
                                {
                                    SetTilebarText(notification.tabName);
                                }
                                break;

                            case ContactTypeSCEE.SetCodeForEditorWithoutUpdate:
                                ContentViewer.MonacoModelID = notification.monacoModelID; ContentViewer.CodeLanguage = notification.typeCode; ContentViewer.Code = notification.code;

                                if (GlobalVariables.CurrentDevice == SCEELibs.Editor.CurrentDevice.Desktop)
                                {
                                    SetTilebarText(notification.tabName);
                                }
                                break;

                            case ContactTypeSCEE.ReloadLanguage:
                                if(GlobalVariables.CurrentIDs.ID_Tab == notification.tabID && GlobalVariables.CurrentIDs.ID_TabsList == notification.tabsListID)
                                {
                                    ContentViewer.CodeLanguage = notification.typeLanguage;
                                    ContentViewer.ForceUpdateLanguage();
                                }
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

            Messenger.Default.Register<BonjourViewControl>(this, async (notification_bjrview) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        PopupGrid.Visibility = Visibility.Collapsed;
                        FrameBonjourView.Content = null;
                    }
                    catch { }

                });

            });

            Messenger.Default.Register<SheetViewerNotification>(this, async (notification_ui) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    try
                    {
                        switch(notification_ui)
                        {
                            case SheetViewerNotification.DeployViewer:
                                UpdateUI(true, false);
                                break;

                            case SheetViewerNotification.MinimizeViewer:
                                UpdateUI(false, false);
                                break;

                            case SheetViewerNotification.DisableAutoDeployer:
                                AutoDeployerEnabled = false;
                                break;

                            case SheetViewerNotification.EnableAutoDeployer:
                                AutoDeployerEnabled = true;
                                break;

                            case SheetViewerNotification.PinViewer:
                                SheetViewerPinned = true;
                                SheetViewSplit.DisplayMode = SplitViewDisplayMode.Inline;
                                SheetsManager.Visibility = Visibility.Collapsed;

                                ContentViewerGrid.Margin = new Thickness(SheetViewSplit.OpenPaneLength, BackgroundPrincipalUIControl.ActualHeight, 0, 0);
                                break;

                            case SheetViewerNotification.UnpinViewer:
                                SheetViewerPinned = false;

                                if (AppSettings.Values.ContainsKey("ui_leftpanelength"))
                                {
                                    ContentViewerGrid.Margin = new Thickness((int)AppSettings.Values["ui_leftpanelength"], BackgroundPrincipalUIControl.ActualHeight, 0, 0);
                                }
                                else
                                {
                                    ContentViewerGrid.Margin = new Thickness(60, BackgroundPrincipalUIControl.ActualHeight, 0, 0);
                                }

                                UpdateUI(true, true);
                                break;
                        }
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

        private void ShowBonjourView()
        {
            FrameBonjourView.Navigate(typeof(BonjourView));
            PopupGrid.Visibility = Visibility.Visible;
        }

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
                        case CurrentDevice.Hololens:
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

                        case CurrentDevice.WindowsMobile:
                            DeployUIIconDeploying.Begin();
                            break;
                    }

                    if(!SheetViewerPinned)
                    {
                        SheetViewSplit.DisplayMode = SplitViewDisplayMode.Inline;
                        SheetViewSplit.IsPaneOpen = true;
                        SheetsManager.Visibility = Visibility.Visible;
                        Messenger.Default.Send(SheetViewMode.Deployed);
                    }

                }
                else
                {

                    switch (GlobalVariables.CurrentDevice)
                    {
                        case CurrentDevice.Hololens:
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

                            if (!SheetViewerPinned)
                                SheetViewSplit.DisplayMode = SplitViewDisplayMode.CompactOverlay;

                            break;

                        case CurrentDevice.WindowsMobile:
                            SheetViewSplit.DisplayMode = SplitViewDisplayMode.Inline;
                            DeployUIIconCollapsing.Begin();
                            break;
                    }

                    if(!SheetViewerPinned)
                    {
                        SheetViewSeparatorLine.Width = 0;

                        SheetsManager.Visibility = Visibility.Collapsed;
                        SheetViewSplit.IsPaneOpen = false;
                        SheetsManager.SelectTabsListSheet();
                        Messenger.Default.Send(SheetViewMode.Minimized);
                    }

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
            NewsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
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

        private void RectangleBackgroundPrinciapalUI_Loaded(object sender, RoutedEventArgs e)
        {
            //Background loading bug fixed
            BackgroundPrinciapalUI.ImageSource = GlobalVariables.CurrentTheme.BackgroundImage;
        }

        private void RectangleBackgroundSheetView_Loaded(object sender, RoutedEventArgs e)
        {
            //Background loading bug fixed
            BackgroundSheetView.ImageSource = GlobalVariables.CurrentTheme.BackgroundImage;
        }

        private void LoadSettings()
        {
            //LINE NUMBERS
            if (AppSettings.Values.ContainsKey("editor_linenumbers"))
                ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ lineNumbers: " + ((bool)AppSettings.Values["editor_linenumbers"]).ToString().ToLower() + "});");

            //QUICK SUGGESTIONS
            if (AppSettings.Values.ContainsKey("editor_quicksuggestions"))
                ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ quickSuggestions: " + ((bool)AppSettings.Values["editor_quicksuggestions"]).ToString().ToLower() + "});");

            //WRAPPING CODE
            if (AppSettings.Values.ContainsKey("editor_wordwrap"))
                ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ wordWrap: " + ((bool)AppSettings.Values["editor_wordwrap"]).ToString().ToLower() + "});");

            //MINIMAP
            if (AppSettings.Values.ContainsKey("editor_minimap"))
                ContentViewer.SendAndExecuteJavaScript("editor.updateOptions({ minimap: { enabled: " + ((bool)AppSettings.Values["editor_minimap"]).ToString().ToLower() + "} });");

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
                    Grid.SetColumnSpan(Toolbar, 1);
                    TextInfoTitlebar.Margin = new Thickness(0);
                    ContentViewerGrid.Margin = new Thickness(0, 0, 0, 75);
                    SheetViewSplit.Margin = new Thickness(0, 0, 0, 75);
                    TextInfoTitlebar.Visibility = Visibility.Collapsed;
                    TopSheetViewSplit.Visibility = Visibility.Collapsed;

                    SheetViewSeparatorLine.Visibility = Visibility.Collapsed;
                    SheetViewSplit.OpenPaneLength = MasterGrid.ActualWidth;
                    SheetsManager.Visibility = Visibility.Collapsed;

                    ToolbarWidth.Width = new GridLength(1, GridUnitType.Star);
                    FocusTitlebarWidth.Width = new GridLength(0, GridUnitType.Pixel);

                    StatusBar.GetForCurrentView().ProgressIndicator.Text = "Serris Code Editor MLV";
                    await StatusBar.GetForCurrentView().ShowAsync();
                    UpdateUI(true, true);
                    break;

                case CurrentDevice.Hololens:
                case CurrentDevice.Desktop:

                    if (AppSettings.Values.ContainsKey("ui_extendedview"))
                    {
                        if (!(bool)AppSettings.Values["ui_extendedview"])
                        {
                            ContentViewerGrid.Margin = new Thickness(60, ColorPrincipalUI.ActualHeight, 0, 0);
                            DeployUIDetector.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            ContentViewerGrid.Margin = new Thickness(60, 0, 0, 0);
                            DeployUIDetector.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        ContentViewerGrid.Margin = new Thickness(60, ColorPrincipalUI.ActualHeight, 0, 0);
                    }

                    //CLOSE PANEL AUTOMATICALLY
                    if (AppSettings.Values.ContainsKey("ui_closepanelauto"))
                    {
                        ClosePanelAuto = (bool)AppSettings.Values["ui_closepanelauto"];
                    }

                    //LEFT PANE REDUCED SETTING
                    if (AppSettings.Values.ContainsKey("ui_leftpanelength"))
                    {
                        TopSheetViewSplit.Width = (int)AppSettings.Values["ui_leftpanelength"];
                        SheetViewSplit.CompactPaneLength = (int)AppSettings.Values["ui_leftpanelength"];
                        ContentViewerGrid.Margin = new Thickness((int)AppSettings.Values["ui_leftpanelength"], ContentViewerGrid.Margin.Top, 0, 0);
                    }

                    //LEFT PANE OPENED SETTING
                    if (AppSettings.Values.ContainsKey("ui_leftpaneopenlength"))
                    {
                        SheetViewSplit.OpenPaneLength = (int)AppSettings.Values["ui_leftpaneopenlength"];
                    }

                    SetTilebarText("");
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

        public void SetTilebarText(string Text)
        {
            string NewText = "";

            if (Text != "")
                NewText = Text + " - ";

            if(SCEELibs.SCEInfos.preReleaseVersion)
            {
                TextInfoTitlebar.Text = $"{NewText}Serris Code Editor ({SCEELibs.SCEInfos.versionName})";
            }
            else
            {
                TextInfoTitlebar.Text = $"{NewText}Serris Code Editor";
            }
            
            ApplicationView.GetForCurrentView().Title = Text;
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
            if (!isUIDeployed && !SheetViewerPinned)
            {
                SheetViewSplit.IsPaneOpen = true;
            }
        }

        private void ModuleSheetView_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed && AutoDeployerEnabled && !SheetViewerPinned)
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
            //ClonePanelAuto = settings for enabling the auto closing of the left panel || AutoDeployerEnabled = variable changed if the current sheet selected is not the tabs list
            if (ClosePanelAuto && isUIDeployed && AutoDeployerEnabled)
            {
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength + 5) || e.GetCurrentPoint(MasterGrid).Position.X <= 0)
                {
                    UpdateUI(false, false);
                }
            }
        }

        private async void ContentViewer_EditorCodeLoaded(object sender, EventArgs e)
        {
            if(!EditorIsReady)
                EditorIsReady = true;

            if (FilesWasOpened)
            {
                await TabsCreatorAssistant.OpenFilesAlreadyOpenedAndCreateNewTabsFiles(GlobalVariables.CurrentIDs.ID_TabsList, OpenedFiles);
                FilesWasOpened = false;
            }
        }

        private void ContentViewerGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed && !SheetViewerPinned)
            {
                SheetViewSplit.IsPaneOpen = false;
            }
        }

        private void EditorViewUI_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (!isUIDeployed && !SheetViewerPinned)
            {
                if (e.GetCurrentPoint(MasterGrid).Position.X >= (SheetViewSplit.OpenPaneLength - 15) || e.GetCurrentPoint(MasterGrid).Position.Y <= 62 || e.GetCurrentPoint(MasterGrid).Position.X <= 10)
                {
                    SheetViewSplit.IsPaneOpen = false;
                }
            }
        }

        private async void ContentViewer_EditorLoaded(object sender, EventArgs e)
        {
            if(!EditorIsLoaded)
            {
                LoadSettings();
                ExecuteModulesFunction();
                EditorIsLoaded = true;
                //SheetsManager.AddTabsListSheet();
                SetMonacoTheme();

                if (AppSettings.Values.ContainsKey("news_token"))
                {
                    if ((int)AppSettings.Values["news_token"] != await NewsHelper.GetCurrentNewsToken())
                    {
                        NewsNotification.ShowBadge = true;
                    }
                }
                else
                {
                    NewsNotification.ShowBadge = true;
                }
                NewsHelper.CheckNewsUpdate();
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            FrameSettings.Navigate(typeof(WindowFlyout), new WindowFlyoutContent { Content = typeof(SettingsManager), WindowIcon = "", WindowTitle = GlobalVariables.GlobalizationRessources.GetString("settings-titleflyout") });
        }

        //For manage tabs content
        List<TabSelectedNotification> Queue_Tabs = new List<TabSelectedNotification>();

        bool CanManageQueue = true;

        private void NewsButton_Click(object sender, RoutedEventArgs e)
        {
            NewsNotification.ShowBadge = false;
            FrameNews.Navigate(typeof(WindowFlyout), new WindowFlyoutContent { Content = typeof(NewsViewer), WindowIcon = "", WindowTitle = GlobalVariables.GlobalizationRessources.GetString("news-titleflyout") });
        }

        private void EditorViewUI_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            var pointerPosition = e.GetCurrentPoint(MasterGrid);

            if (pointerPosition.Position.X <= 75 && pointerPosition.Position.Y <= 80)
            {
                if (AppSettings.Values.ContainsKey("ui_extendedview"))
                {
                    if (!(bool)AppSettings.Values["ui_extendedview"])
                    {
                        if (!isUIDeployed && GlobalVariables.CurrentDevice != CurrentDevice.WindowsMobile)
                        {
                            UpdateUI(true, false);
                        }
                    }
                    else if(pointerPosition.Position.X <= 60)
                    {
                        if (!isUIDeployed && GlobalVariables.CurrentDevice != CurrentDevice.WindowsMobile)
                        {
                            UpdateUI(true, false);
                        }
                    }
                }
                else
                {
                    if (!isUIDeployed && GlobalVariables.CurrentDevice != CurrentDevice.WindowsMobile)
                    {
                        UpdateUI(true, false);
                    }
                }
                
            }
        }

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
                        //Code content
                        string content = await ContentViewer.GetCode();
                        SerrisModulesServer.Manager.AsyncHelpers.RunSync(() => TabsWriteManager.PushTabContentViaIDAsync(GlobalVariables.CurrentIDs, content, false));

                        //Cursor position
                        PositionSCEE CursorPosition = await ContentViewer.GetCursorPosition();
                        InfosTab Tab = TabsAccessManager.GetTabViaID(GlobalVariables.CurrentIDs);
                        Tab.TabCursorPosition = new CursorPosition { column = CursorPosition.column, row = CursorPosition.row };
                        await TabsWriteManager.PushUpdateTabAsync(Tab, GlobalVariables.CurrentIDs.ID_TabsList, false);
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
                ContentViewer.MonacoModelID = Queue_Tabs[0].monacoModelID;
                ContentViewer.CursorPositionColumn = Queue_Tabs[0].cursorPositionColumn;
                ContentViewer.CursorPositionRow = Queue_Tabs[0].cursorPositionLineNumber;
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
                OpenPaneLengthOriginal = 320;

            //var pointerPosition = Windows.UI.Core.CoreWindow.GetForCurrentThread().PointerPosition;
            //SheetViewSplit.OpenPaneLength = pointerPosition.X + 4;

        }
        private void EditorView_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (SeparatorClicked)
            {
                var pointerPosition = e.GetCurrentPoint(MasterGrid);

                if(pointerPosition.Position.X + 4 > OpenPaneLengthOriginal)
                {
                    SheetViewSplit.OpenPaneLength = pointerPosition.Position.X + 4;

                    if(SheetViewerPinned)
                    {
                        ContentViewerGrid.Margin = new Thickness(pointerPosition.Position.X + 4, BackgroundPrincipalUIControl.ActualHeight, 0, 0);
                    }
                }
            }
        }

        private void SeparatorLinePointerReleased(object sender, PointerRoutedEventArgs e)
        {
            SeparatorClicked = false;
            AppSettings.Values["ui_leftpaneopenlength"] = Convert.ToInt32(SheetViewSplit.OpenPaneLength);
        }

        private void SheetViewSeparatorLine_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            SheetViewSeparatorLine.Opacity = 0.8;
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.SizeWestEast, 1);
        }

        private void SheetViewSeparatorLine_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            SheetViewSeparatorLine.Opacity = 1;
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);
        }

    }

}
