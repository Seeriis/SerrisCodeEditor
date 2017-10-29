using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type;
using SerrisModulesServer.Type.Addon;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisCodeEditor.Xaml.Views
{
    public class ModuleInfosShow
    {
        public BitmapImage Thumbnail { get; set; }
        public InfosModule Module { get; set; }
    }

    public sealed partial class ModulesManager : Page
    {
        public ModulesManager()
        {
            InitializeComponent();
        }

        private void ModulesManagerUI_Loaded(object sender, RoutedEventArgs e)
        => ChangeSelectedButton(0);



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private async void ChangeSelectedButton(int newSelectedButton)
        {
            if (currentSelectedButton != newSelectedButton)
            {
                currentSelectedButton = newSelectedButton;
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

        }

        private async void ListModules_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListModules.SelectedItem != null)
            {
                var module = (ModuleInfosShow)ListModules.SelectedItem;
                switch (currentSelectedButton)
                {
                    case 0:
                        var osef = new Flyout();
                        var osef_b = new Frame();
                        var executor = new AddonExecutor(module.Module.ID, AddonExecutorFuncTypes.main, ref osef, ref osef_b);
                        break;

                    case 1:
                        await Modules_manager_writer.SetCurrentThemeIDAsync(module.Module.ID);
                        break;
                }
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

        private void ModuleOptions_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
