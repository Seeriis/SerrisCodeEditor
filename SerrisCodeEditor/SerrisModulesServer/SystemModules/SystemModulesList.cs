using SerrisModulesServer.Items;
using SerrisModulesServer.Type;
using System.Collections.Generic;

namespace SerrisModulesServer.SystemModules
{
    public static class SystemModulesList
    {
        public static InfosModule[] Modules =
        {
            new InfosModule
            {
                ID = 0,
                ModuleType = ModuleTypesList.Theme,
                ContainMonacoTheme = true,
                ModuleMonacoThemeName = "lightTheme",
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
                ModuleName = "[TEST] Soundcloud flyout",
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
                ModuleMonacoThemeName = "darkTheme",
                ModuleName = "SCE - Dark theme",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Background by Last Sentencer (deviantart)",
                CanBePinnedToToolBar = false,
                IsEnabled = true
            },
            new InfosModule
            {
                ID = 7,
                ModuleType = ModuleTypesList.Addon,
                ContainMonacoTheme = false,
                ModuleName = "SCE Marne-la-Vallée informations",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Informations about the SCE project !",
                CanBePinnedToToolBar = true,
                IsEnabled = true,
                JSFilesPathList = new List<string>
                {
                    "libs/notification.js"
                }
            },


            /* =============
             * = LANGUAGES =
             * =============
            */

            new InfosModule
            {
                ID = 8,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "CSS",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "CSS language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "css",
                ProgrammingLanguageFilesExtensions = new List<string> { ".css" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 9,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "C#",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "C#(.NET) language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "csharp",
                ProgrammingLanguageFilesExtensions = new List<string> { ".cs" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            }
        };
    }
}
