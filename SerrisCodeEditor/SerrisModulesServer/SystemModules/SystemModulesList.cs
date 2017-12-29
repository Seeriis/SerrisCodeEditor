using SerrisModulesServer.Items;
using SerrisModulesServer.Type;
using System.Collections.Generic;

namespace SerrisModulesServer.SystemModules
{
    public class SystemModulesList
    {
        public List<InfosModule> Modules = new List<InfosModule>
        {
            new InfosModule
            {
                ID = 0,
                ModuleType = ModuleTypesList.Theme,
                ContainMonacoTheme = true,
                ModuleName = "SCE - Light theme",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Default light theme of SCE !",
                CanBePinnedToToolBar = false,
                IsEnabled = true
            },
            new InfosModule
            {
                ID = 1,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "[TEST] Youtube flyout",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Test addon",
                CanBePinnedToToolBar = true,
                IsEnabled = true,
                JSFilesPathList = new List<string>()
            },
            new InfosModule
            {
                ID = 2,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "[TEST] Notification",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Test addon B",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                JSFilesPathList = new List<string>
                {
                    "libs/test.js"
                }
            },
            new InfosModule
            {
                ID = 3,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "Undo'n Redo module",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Module for undo and redo in the editor",
                CanBePinnedToToolBar = true,
                IsEnabled = true,
                JSFilesPathList = new List<string>
                { }
            },
            new InfosModule
            {
                ID = 4,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "Save module",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Module for save tabs in the editor",
                CanBePinnedToToolBar = true,
                IsEnabled = true,
                JSFilesPathList = new List<string>
                { }
            },
            new InfosModule
            {
                ID = 5,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "Clipboard module",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Module for copy, cut or paste content in the editor",
                CanBePinnedToToolBar = true,
                IsEnabled = true,
                JSFilesPathList = new List<string>
                { }
            },
            new InfosModule
            {
                ID = 6,
                ModuleType = ModuleTypesList.Theme,
                ContainMonacoTheme = true,
                ModuleName = "SCE - Dark theme",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Background by Last Sentencer",
                CanBePinnedToToolBar = false,
                IsEnabled = true
            }
        };
    }
}
