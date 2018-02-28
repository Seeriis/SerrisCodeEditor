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
        public int StrokeThickness { get; set; }

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
            get { return GlobalVariables.CurrentTheme.MainColorFont; }
        }

    }

    public sealed partial class ModulesManager : Page
    {

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
            BackgroundList.Fill = GlobalVariables.CurrentTheme.MainColor;
            MenuButtons.Background = GlobalVariables.CurrentTheme.SecondaryColor;

            ButtonsSeparator.Fill = GlobalVariables.CurrentTheme.SecondaryColorFont;

            AddonsText.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            AddonsIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            ThemesText.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            ThemesIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            InstallButton.Background = GlobalVariables.CurrentTheme.MainColor;
            IconInstallButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            TextInstallButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
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
            int IDThemeMonaco = await ModulesAccessManager.GetCurrentThemeMonacoID(), IDTheme = await ModulesAccessManager.GetCurrentThemeIDAsync();

            foreach (InfosModule module in await ModulesAccessManager.GetModulesAsync(true))
            {
                var module_infos = new ModuleInfosShow { Module = module, StrokeThickness = 0 };
                var reader = new AddonReader(module_infos.Module.ID);
                module_infos.Thumbnail = await reader.GetAddonIconViaIDAsync();

                switch (module.ModuleType)
                {
                    case ModuleTypesList.Addon when currentSelectedButton == 0:
                        ListModules.Items.Add(module_infos);
                        break;

                    case ModuleTypesList.Theme when currentSelectedButton == 1:
                        if (IDTheme == module_infos.Module.ID || IDThemeMonaco == module_infos.Module.ID)
                            module_infos.StrokeThickness = 2;

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
                        await ModulesWriteManager.SetCurrentThemeIDAsync(module.Module.ID);

                        if(module.Module.ContainMonacoTheme)
                            await ModulesWriteManager.SetCurrentMonacoThemeIDAsync(module.Module.ID);

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

            List<int> list = await ModulesPinned.GetModulesPinned();

            if (list.Contains(module.Module.ID))
                ModulesPinned.RemoveModule(module.Module.ID);
            else
                ModulesPinned.AddNewModule(module.Module.ID);

        }

        private void InstallButton_Click(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Send(new ModuleSheetNotification { id = -1, sheetName = "Module installer", type = ModuleSheetNotificationType.NewSheet, sheetContent = new ModulesInstaller(), sheetIcon = new BitmapImage(new Uri(this.BaseUri, "/Assets/Icons/modules_installer.png")), sheetSystem = false });
        }

        private async void DeleteAcceptButton_Click(object sender, RoutedEventArgs e)
        {
            ModuleInfosShow element = (ModuleInfosShow)((Button)sender).DataContext;

            if(await ModulesWriteManager.DeleteModuleViaIDAsync(element.Module.ID))
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



        int currentSelectedButton = -1;

    }
}
