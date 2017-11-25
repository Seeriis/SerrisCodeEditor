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
                IsPinnedToToolBar = false,
                IsEnabled = true
            },
            new InfosModule
            {
                ID = 1,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "Test addon c:",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Test addon",
                IsPinnedToToolBar = true,
                IsEnabled = true,
                JSFilesPathList = new List<string>()
            },
            new InfosModule
            {
                ID = 2,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "Lool",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Test addon B",
                IsPinnedToToolBar = true,
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
                IsPinnedToToolBar = true,
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
                IsPinnedToToolBar = true,
                IsEnabled = true,
                JSFilesPathList = new List<string>
                { }
            }
        };
    }
}
