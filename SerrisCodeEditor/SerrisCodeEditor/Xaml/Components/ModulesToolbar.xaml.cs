using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisCodeEditor.Xaml.Views;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Addon;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    public sealed partial class ModulesToolbar : UserControl
    {
        ModulesAccessManager Modules_manager_access = new ModulesAccessManager(); ModulesWriteManager Modules_manager_writer = new ModulesWriteManager();
        TempContent temp_variables = new TempContent();

        public ModulesToolbar()
        {
            this.InitializeComponent();
        }

        private void Toolbar_Loaded(object sender, RoutedEventArgs e)
        { SetMessenger(); }

        private async void ToolbarContent_Loaded(object sender, RoutedEventArgs e)
        {
            var sms_initialize = await Modules_manager_access.GetModulesAsync(true);
            foreach (InfosModule module in sms_initialize)
            {
                if (module.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon)
                {
                    AddModule(module.ID);
                }
            }

        }

        private void Module_button_Click(object sender, RoutedEventArgs e)
        {
            PinnedModule module = (sender as Button).DataContext as PinnedModule;
            Flyout osef = new Flyout(); Frame osef_b = new Frame();
            AddonExecutor executor = new AddonExecutor(module.ID, AddonExecutorFuncTypes.main, ref osef, ref osef_b);
        }



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private void SetMessenger()
        {
            Messenger.Default.Register<SMSNotification>(this, (notification) =>
            {
                try
                {
                    switch (notification.Type)
                    {
                        case TypeUpdateModule.ModuleDeleted:
                            RemoveModule(notification.ID);
                            break;

                        case TypeUpdateModule.NewModule:
                            AddModule(notification.ID);
                            break;

                        case TypeUpdateModule.UpdateModule:
                            break;
                    }
                }
                catch { }
            });

            Messenger.Default.Register<EditorViewNotification>(this, (notification_ui) =>
            {
                try
                {
                    
                }
                catch { }
            });
        }

        private async void AddModule(int ID)
        {
            var module = await Modules_manager_access.GetModuleViaIDAsync(ID);

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
            }

        }

        private void RemoveModule(int ID)
        { ToolbarContent.Children.Remove((UIElement)ToolbarContent.FindName("" + ID)); }

        private void ButtonListModules_Click(object sender, RoutedEventArgs e)
        { FrameListModules.Navigate(typeof(ModulesManager)); }
    }
}
