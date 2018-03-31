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
                ModuleAuthor = "Microsoft",
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
                ModuleAuthor = "Microsoft",
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
            },

            new InfosModule
            {
                ID = 10,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "HTML",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "HTML language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "html",
                ProgrammingLanguageFilesExtensions = new List<string> { ".html", ".htm" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 11,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Java",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Java language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "java",
                ProgrammingLanguageFilesExtensions = new List<string> { ".class" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 12,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Javascript",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Javascript language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "javascript",
                ProgrammingLanguageFilesExtensions = new List<string> { ".js" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 13,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Python",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Python language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "python",
                ProgrammingLanguageFilesExtensions = new List<string> { ".py", ".pyc", ".pyd" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 14,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Ruby",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Ruby language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "ruby",
                ProgrammingLanguageFilesExtensions = new List<string> { ".rb" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 15,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Batch script (BAT)",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Batch script (.bat files) language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "bat",
                ProgrammingLanguageFilesExtensions = new List<string> { ".bat" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            }
        };
    }
}
