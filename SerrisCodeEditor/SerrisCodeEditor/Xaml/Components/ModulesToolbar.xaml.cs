using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Components;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisCodeEditor.Xaml.Views;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Addon;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace SerrisCodeEditor.Xaml.Components
{
    public sealed partial class ModulesToolbar : UserControl
    {

        public ModulesToolbar()
        {
            InitializeComponent();
            //GlobalVariables.CurrentModulesToolbar = ToolbarContent;
        }

        private void Toolbar_Loaded(object sender, RoutedEventArgs e)
        {
            SetMessenger();
            SetTheme();
        }

        private async void ToolbarContent_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Run(async () => 
            {
                foreach (string id in await ModulesPinned.GetModulesPinned())
                {
                    await AddModule(id);
                }
            });
        }

        private void Module_button_Click(object sender, RoutedEventArgs e)
        {
            PinnedModule module = (sender as Button).DataContext as PinnedModule;
            //AddonExecutor executor = new AddonExecutor(module.ID, AddonExecutorFuncTypes.main, new SCEELibs.SCEELibs(module.ID));
        }



        public bool IsScrollable
        {
            get
            {
                if(ToolbarContent.ActualWidth > ScrollMaster.ActualWidth)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public static readonly DependencyProperty IsScrollableProperty = DependencyProperty.Register("IsScrollable", typeof(bool), typeof(ModulesToolbar), new PropertyMetadata(0));



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private void SetMessenger()
        {
            Messenger.Default.Register<SMSNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () => 
                {
                    try
                    {
                        switch (notification.Type)
                        {
                            case TypeUpdateModule.ModuleDeleted:
                                RemoveModule(notification.ID);
                                ModulesPinned.RemoveModule(notification.ID);
                                break;

                            case TypeUpdateModule.NewModule:
                                await AddModule(notification.ID);
                                break;

                            case TypeUpdateModule.UpdateModule:
                                break;
                        }
                    }
                    catch { }

                });

            });

            Messenger.Default.Register<ModulesPinnedNotification>(this, async (notification) => 
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    try
                    {
                        switch (notification.Modification)
                        {
                            case ModulesPinedModification.Added:
                                await AddModule(notification.ID);
                                break;

                            case ModulesPinedModification.Removed:
                                RemoveModule(notification.ID);
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

            Messenger.Default.Register<ToolbarNotification>(this, async (notification_toolbar) =>
            {

                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        StackPanel widget = (StackPanel)ToolbarContent.FindName("" + notification_toolbar.id);

                        switch (notification_toolbar.propertie)
                        {
                            case ToolbarProperties.ButtonEnabled:
                                ((Button)widget.FindName(notification_toolbar.uiElementName + notification_toolbar.id)).IsEnabled = (bool)notification_toolbar.content;
                                break;

                            case ToolbarProperties.IsButtonEnabled when !notification_toolbar.answerNotification:
                                Button element = (Button)widget.FindName(notification_toolbar.uiElementName + notification_toolbar.id);
                                Messenger.Default.Send(new ToolbarNotification { id = notification_toolbar.id, uiElementName = notification_toolbar.uiElementName, propertie = ToolbarProperties.IsButtonEnabled, answerNotification = true, content = element.IsEnabled });
                                break;

                            case ToolbarProperties.SetTextBoxContent:
                                ((TextBox)widget.FindName(notification_toolbar.uiElementName + notification_toolbar.id)).Text = (string)notification_toolbar.content;
                                break;

                            case ToolbarProperties.GetTextBoxContent when !notification_toolbar.answerNotification:
                                TextBox elementb = (TextBox)widget.FindName(notification_toolbar.uiElementName + notification_toolbar.id);
                                Messenger.Default.Send(new ToolbarNotification { id = notification_toolbar.id, guid = notification_toolbar.guid, uiElementName = notification_toolbar.uiElementName, propertie = ToolbarProperties.GetTextBoxContent, answerNotification = true, content = elementb.Text });
                                break;

                            case ToolbarProperties.OpenFlyout:
                                UIElement UIElement = (UIElement)widget.FindName(notification_toolbar.uiElementName + notification_toolbar.id);
                                Flyout FlyoutElement;

                                if (UIElement.ContextFlyout == null)
                                {
                                    FlyoutElement = new Flyout();
                                    ModuleHTMLView HTMLView = new ModuleHTMLView();
                                    HTMLView.Height = 300; HTMLView.Width = 300;

                                    HTMLView.LoadPage((string)notification_toolbar.content, notification_toolbar.id);
                                    FlyoutElement.Content = HTMLView; FlyoutElement.FlyoutPresenterStyle = (Style)Application.Current.Resources["FlyoutBorderless"];

                                    UIElement.ContextFlyout = FlyoutElement;
                                    UIElement.ContextFlyout.ShowAt(UIElement as FrameworkElement);
                                }
                                else
                                {
                                    if((string)notification_toolbar.content != "")
                                    {
                                        FlyoutElement = (UIElement.ContextFlyout as Flyout);
                                        (FlyoutElement.Content as ModuleHTMLView).LoadPage((string)notification_toolbar.content, notification_toolbar.id);
                                    }

                                    UIElement.ContextFlyout.ShowAt(UIElement as FrameworkElement);
                                }

                                break;
                        }
                    }
                    catch { }

                });

            });
        }

        private void SetTheme()
        {
            ButtonListModules.Background = GlobalVariables.CurrentTheme.ToolbarRoundButtonColor;
            ButtonListModules.Foreground = GlobalVariables.CurrentTheme.ToolbarRoundButtonColorFont;
            ButtonListModules.BorderBrush = GlobalVariables.CurrentTheme.ToolbarRoundButtonColorFont;
        }

        private async Task AddModule(string ID)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                StackPanel Widget = await new AddonReader(ID).GetAddonWidgetViaIDAsync(new SCEELibs.SCEELibs(ID), GlobalVariables.CurrentTheme);
                Widget.PointerReleased += ((e, f) =>
                {
                    MenuFlyout Flyout = new MenuFlyout();
                    MenuFlyoutItem UnpinButton = new MenuFlyoutItem { Text = "Unpin this widget" };
                    UnpinButton.Click += ((a, c) => 
                    {
                        ModulesPinned.RemoveModule(ID);
                    });

                    Flyout.Items.Add(UnpinButton);
                    Flyout.ShowAt((FrameworkElement)e);
                });

                ToolbarContent.Children.Add(Widget);
            });

            SCEELibs.SCEELibs Libs = new SCEELibs.SCEELibs(ID);
            await Task.Run(() => new AddonExecutor(ID, Libs).ExecuteDefaultFunction(AddonExecutorFuncTypes.whenModuleIsPinned));

        }

        private void RemoveModule(string ID)
        => ToolbarContent.Children.Remove((UIElement)ToolbarContent.FindName("" + ID));

        private void ButtonListModules_Click(object sender, RoutedEventArgs e)
        => FrameListModules.Navigate(typeof(WindowFlyout), new WindowFlyoutContent { Content = typeof(ModulesManager), WindowIcon = "", WindowTitle = GlobalVariables.GlobalizationRessources.GetString("modulesmanager-titleflyout") });

        private void ScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            //ScrollMaster.Scroll
            ScrollMaster.ChangeView(ScrollMaster.HorizontalOffset + (e.GetCurrentPoint(ScrollMaster).Properties.MouseWheelDelta * 2), ScrollMaster.VerticalOffset, ScrollMaster.ZoomFactor);
            //ScrollMaster.ScrollToHorizontalOffset(ScrollMaster.HorizontalOffset + e.GetCurrentPoint(ScrollMaster).Properties.MouseWheelDelta);
        }
    }
}
