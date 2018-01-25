using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace SCEELibs.Modules
{
    [AllowForWeb]
    public sealed class ModuleInfoVersion
    {
        public int major { get; set; }
        public int minor { get; set; }
        public int revision { get; set; }
    }

    [AllowForWeb]
    public sealed class ModuleInfo
    {
        public int ID { get; set; }
        public bool moduleSystem { get; set; }

        public ModuleInfoVersion moduleVersion { get; set; }
        public string moduleName { get; set; }
        public string moduleAuthor { get; set; }
        public string moduleDescription { get; set; }
        public string moduleWebsiteLink { get; set; }

        public bool isEnabled { get; set; }
        public bool isPinnedToToolbar { get; set; }
        public bool containMonacoTheme { get; set; }

    }

    [AllowForWeb]
    public sealed class Manager
    {

        public IList<ModuleInfo> getThemesAvailable(bool system_themes)
        {
            IList<ModuleInfo> list_themes_final = new List<ModuleInfo>();
            List<InfosModule> list_themes = ModulesAccessManager.GetModules(system_themes);

            foreach (InfosModule theme in list_themes.Where(n => n.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Theme).ToList())
            {
                list_themes_final.Add(new ModuleInfo { ID = theme.ID, moduleSystem = theme.ModuleSystem, moduleName = theme.ModuleName, moduleAuthor = theme.ModuleAuthor, moduleDescription = theme.ModuleDescription, moduleWebsiteLink = theme.ModuleWebsiteLink, containMonacoTheme = theme.ContainMonacoTheme, isEnabled = theme.IsEnabled, isPinnedToToolbar = theme.CanBePinnedToToolBar, moduleVersion = new ModuleInfoVersion { major = theme.ModuleVersion.Major, minor = theme.ModuleVersion.Minor, revision = theme.ModuleVersion.Revision } });
            }

            return list_themes_final;
        }

        public IList<ModuleInfo> getAddonsAvailable(bool system_addons)
        {
            IList<ModuleInfo> list_addons_final = new List<ModuleInfo>();
            List<InfosModule> list_addons = ModulesAccessManager.GetModules(system_addons);

            foreach (InfosModule theme in list_addons.Where(n => n.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon).ToList())
            {
                list_addons_final.Add(new ModuleInfo { ID = theme.ID, moduleSystem = theme.ModuleSystem, moduleName = theme.ModuleName, moduleAuthor = theme.ModuleAuthor, moduleDescription = theme.ModuleDescription, moduleWebsiteLink = theme.ModuleWebsiteLink, containMonacoTheme = theme.ContainMonacoTheme, isEnabled = theme.IsEnabled, isPinnedToToolbar = theme.CanBePinnedToToolBar, moduleVersion = new ModuleInfoVersion { major = theme.ModuleVersion.Major, minor = theme.ModuleVersion.Minor, revision = theme.ModuleVersion.Revision } });
            }

            return list_addons_final;
        }

        public ModuleInfo getModuleInfosViaID(int ID)
        {
            var module = ModulesAccessManager.GetModuleViaID(ID);
            return new ModuleInfo { ID = module.ID, moduleSystem = module.ModuleSystem, moduleName = module.ModuleName, moduleAuthor = module.ModuleAuthor, moduleDescription = module.ModuleDescription, moduleWebsiteLink = module.ModuleWebsiteLink, containMonacoTheme = module.ContainMonacoTheme, isEnabled = module.IsEnabled, isPinnedToToolbar = module.CanBePinnedToToolBar, moduleVersion = new ModuleInfoVersion { major = module.ModuleVersion.Major, minor = module.ModuleVersion.Minor, revision = module.ModuleVersion.Revision } };
        }

        public async void installModule(string zip_path)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                MessageDialog dialog_warning = new MessageDialog("");
                dialog_warning.Title = "An module want to install a new module on the editor";
                dialog_warning.Content = "Are you sure to accept the module to install a new module on the editor ?";

                dialog_warning.Commands.Add(new UICommand { Label = "Yes", Invoked = async (e) => 
                {
                    StorageFile file = await StorageFile.GetFileFromPathAsync(zip_path);
                    var result_verify = await new ModulesVerifyAssistant(file).VerifyPackageAsync();

                    if(result_verify == PackageVerificationCode.Passed)
                        await ModulesWriteManager.AddModuleAsync(file);

                }
                });

                dialog_warning.Commands.Add(new UICommand { Label = "No", Invoked = (e) => 
                {}
                });

                await dialog_warning.ShowAsync();
            });

        }

        public async void deleteModule(int ID)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                var infos = getModuleInfosViaID(ID);
                MessageDialog dialog_warning = new MessageDialog("");
                dialog_warning.Title = "An module want to uninstall \"" + infos.moduleName + "\" on the editor";
                dialog_warning.Content = "Are you sure to accept the module to uninstall \"" + infos.moduleName + "\" on the editor ?";

                dialog_warning.Commands.Add(new UICommand
                {
                    Label = "Yes",
                    Invoked = async (e) =>
                    {
                        await ModulesWriteManager.DeleteModuleViaIDAsync(ID);
                    }
                });

                dialog_warning.Commands.Add(new UICommand
                {
                    Label = "No",
                    Invoked = (e) =>
                    { }
                });

                await dialog_warning.ShowAsync();
            });

        }

    }

}
