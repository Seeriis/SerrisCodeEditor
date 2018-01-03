using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type;
using SerrisModulesServer.Type.Addon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisCodeEditor.Xaml.Views
{
    public class ModuleInfosShow
    {
        public BitmapImage Thumbnail { get; set; }
        public InfosModule Module { get; set; }
        public Visibility DeleteButtonVisibility
        {
            get
            {
                if(Module.ModuleSystem)
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Visible;
                }
            }
        }

        public SolidColorBrush ForegroundColor
        {
            get { return new TempContent().CurrentTheme.MainColorFont; }
        }
    }

    public sealed partial class ModulesManager : Page
    {
        ModulesPinned Pinned = new ModulesPinned();

        public ModulesManager()
        {
            InitializeComponent();
            SetTheme();
        }

        private void ModulesManagerUI_Loaded(object sender, RoutedEventArgs e)
        => ChangeSelectedButton(0);



        /* =============
         * = FUNCTIONS =
         * =============
         */


        private void SetTheme()
        {
            BackgroundList.Fill = temp_variables.CurrentTheme.MainColor;
            MenuButtons.Background = temp_variables.CurrentTheme.SecondaryColor;

            ButtonsSeparator.Fill = temp_variables.CurrentTheme.SecondaryColorFont;

            AddonsText.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            AddonsIcon.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;

            ThemesText.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;
            ThemesIcon.Foreground = temp_variables.CurrentTheme.SecondaryColorFont;

            InstallButton.Background = temp_variables.CurrentTheme.MainColor;
            IconInstallButton.Foreground = temp_variables.CurrentTheme.MainColorFont;
            TextInstallButton.Foreground = temp_variables.CurrentTheme.MainColorFont;
        }

        private void ChangeSelectedButton(int newSelectedButton)
        {
            if (currentSelectedButton != newSelectedButton)
            {
                currentSelectedButton = newSelectedButton;
                LoadModules();
            }

        }

        private async void LoadModules()
        {
            ListModules.Items.Clear();

            foreach (InfosModule module in await Modules_manager_access.GetModulesAsync(true))
            {
                var module_infos = new ModuleInfosShow { Module = module };
                var reader = new AddonReader(module_infos.Module.ID);
                module_infos.Thumbnail = await reader.GetAddonIconViaIDAsync();

                switch (module.ModuleType)
                {
                    case ModuleTypesList.Addon when currentSelectedButton == 0:
                        ListModules.Items.Add(module_infos);
                        break;

                    case ModuleTypesList.Theme when currentSelectedButton == 1:
                        ListModules.Items.Add(module_infos);
                        break;
                }
            }
        }

        private async void ListModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListModules.SelectedItem != null)
            {
                var module = (ModuleInfosShow)ListModules.SelectedItem;
                switch (currentSelectedButton)
                {
                    case 0:
                        await Task.Run(() => 
                        {
                            new AddonExecutor(module.Module.ID, new SCEELibs.SCEELibs(module.Module.ID)).ExecuteDefaultFunction(AddonExecutorFuncTypes.main);
                        });
                        break;

                    case 1:
                        await Modules_manager_writer.SetCurrentThemeIDAsync(module.Module.ID);
                        break;
                }
            }
        }

        private void ModuleOptions_Click(object sender, RoutedEventArgs e)
        {
        }

        private async void ModulePin_Click(object sender, RoutedEventArgs e)
        {
            ModuleInfosShow module = (ModuleInfosShow)(sender as Button).DataContext;

            List<int> list = await Pinned.GetModulesPinned();

            if (list.Contains(module.Module.ID))
                Pinned.RemoveModule(module.Module.ID);
            else
                Pinned.AddNewModule(module.Module.ID);

        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new ModuleSheetNotification { id = -1, sheetName = "Module installer", type = ModuleSheetNotificationType.NewSheet, sheetContent = new ModulesInstaller(), sheetIcon = new BitmapImage(new Uri(this.BaseUri, "/Assets/Icons/modules_installer.png")), sheetSystem = false });
        }

        private async void DeleteAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            ModuleInfosShow element = (ModuleInfosShow)((Button)sender).DataContext;

            if(await Modules_manager_writer.DeleteModuleViaIDAsync(element.Module.ID))
            {
                LoadModules();
            }
        }

        private void AddonsButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        => ChangeSelectedButton(0);

        private void ThemesButton_PointerPressed(object sender, PointerRoutedEventArgs e)
        => ChangeSelectedButton(1);



        /* =============
         * = VARIABLES =
         * =============
         */



        ModulesAccessManager Modules_manager_access = new ModulesAccessManager(); ModulesWriteManager Modules_manager_writer = new ModulesWriteManager();
        int currentSelectedButton = -1;
        TempContent temp_variables = new TempContent();

    }
}
