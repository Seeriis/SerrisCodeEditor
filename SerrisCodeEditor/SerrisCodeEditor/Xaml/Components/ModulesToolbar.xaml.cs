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
        ModulesAccessManager Modules_manager_access = new ModulesAccessManager(); ModulesWriteManager Modules_manager_writer = new ModulesWriteManager();
        TempContent temp_variables = new TempContent();

        public ModulesToolbar()
        {
            InitializeComponent();
            temp_variables.CurrentModulesToolbar = ToolbarContent;
        }

        private void Toolbar_Loaded(object sender, RoutedEventArgs e)
        { SetMessenger(); }

        private async void ToolbarContent_Loaded(object sender, RoutedEventArgs e)
        {
            //List<InfosModule> sms_initialize = await Modules_manager_access.GetModulesAsync(true);
            foreach(int id in await new ModulesPinned().GetModulesPinned())
            {
                await AddModule(id);
            }

            /*foreach (InfosModule module in sms_initialize)
            {
                if (module.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon)
                {
                    AddModule(module.ID);
                }
            }*/

        }

        private void Module_button_Click(object sender, RoutedEventArgs e)
        {
            PinnedModule module = (sender as Button).DataContext as PinnedModule;
            //AddonExecutor executor = new AddonExecutor(module.ID, AddonExecutorFuncTypes.main, new SCEELibs.SCEELibs(module.ID));
        }



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
                                new ModulesPinned().RemoveModule(notification.ID);
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

            Messenger.Default.Register<EditorViewNotification>(this, (notification_ui) =>
            {
                try
                {

                }
                catch { }
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
                                    HTMLView.Height = 250; HTMLView.Width = 250;

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

        private async Task AddModule(int ID)
        {
            /*var module = await Modules_manager_access.GetModuleViaIDAsync(ID);

            if(module != null)
            {
                PinnedModule pinned = new PinnedModule { ID = module.ID, ModuleName = module.ModuleName, ModuleType = module.ModuleType };
                pinned.Image = await new AddonReader(module.ID).GetAddonIconViaIDAsync();

                Button module_button = new Button();
                module_button.DataContext = pinned; module_button.Name = "" + pinned.ID;
                module_button.Height = 30; module_button.Width = 30; module_button.Margin = new Thickness(2, 0, 2, 0);
                var image_brush = new ImageBrush(); image_brush.Stretch = Stretch.Uniform; image_brush.ImageSource = pinned.Image;
                module_button.Background = image_brush;
                module_button.Click += Module_button_Click;


                ToolbarContent.Children.Add(module_button);
            }*/
            //new AddonExecutor(ID, new SCEELibs.SCEELibs(ID)).ExecuteDefaultFunction(AddonExecutorFuncTypes.whenModuleIsPinned);

            ToolbarContent.Children.Add(await new AddonReader(ID).GetAddonWidgetViaIDAsync(new SCEELibs.SCEELibs(ID)));
        }

        private void RemoveModule(int ID)
        { ToolbarContent.Children.Remove((UIElement)ToolbarContent.FindName("" + ID)); }

        private void ButtonListModules_Click(object sender, RoutedEventArgs e)
        { FrameListModules.Navigate(typeof(ModulesManager)); }

        private void ScrollViewer_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            //ScrollMaster.Scroll
            ScrollMaster.ChangeView(ScrollMaster.HorizontalOffset + (e.GetCurrentPoint(ScrollMaster).Properties.MouseWheelDelta * 2), ScrollMaster.VerticalOffset, ScrollMaster.ZoomFactor);
            //ScrollMaster.ScrollToHorizontalOffset(ScrollMaster.HorizontalOffset + e.GetCurrentPoint(ScrollMaster).Properties.MouseWheelDelta);
        }
    }
}
