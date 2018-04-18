using SerrisModulesServer.Items;
using SerrisModulesServer.Type;
using SerrisModulesServer.Type.Templates;
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
            },

            new InfosModule
            {
                ID = 16,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Coffee script",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Coffee script language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "coffee",
                ProgrammingLanguageFilesExtensions = new List<string> { ".coffee" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 17,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "C / C++",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "C / C++ language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "cpp",
                ProgrammingLanguageFilesExtensions = new List<string> { ".cpp", ".c", ".h", ".hh" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 18,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "CSP",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "CSP file",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "csp",
                ProgrammingLanguageFilesExtensions = new List<string> { ".csp" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 19,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Docker file",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Docker file",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "dockerfile",
                ProgrammingLanguageFilesExtensions = new List<string> { ".dockerfile" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 20,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "F#",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "F# language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "fsharp",
                ProgrammingLanguageFilesExtensions = new List<string> { ".fs", ".fsi", ".fsx", ".fsscript" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 21,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Go",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Go language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "go",
                ProgrammingLanguageFilesExtensions = new List<string> { ".go" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 22,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Handlebars (HTML templating)",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Handlebars.js: HTML templating",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "handlebars",
                ProgrammingLanguageFilesExtensions = new List<string> { },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 23,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "INI file",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "INI file type",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "ini",
                ProgrammingLanguageFilesExtensions = new List<string> { ".ini" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 24,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Less",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Less language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "less",
                ProgrammingLanguageFilesExtensions = new List<string> { ".less" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 25,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Lua",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Lua language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "lua",
                ProgrammingLanguageFilesExtensions = new List<string> { ".lua" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 26,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Markdown",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Markdown language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "markdown",
                ProgrammingLanguageFilesExtensions = new List<string> { ".md", ".markdown" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 27,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "MsDAX",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Microsoft Dynamics AX language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "msdax",
                ProgrammingLanguageFilesExtensions = new List<string> { ".msdax" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 28,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "MySQL",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "MySQL language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "mysql",
                ProgrammingLanguageFilesExtensions = new List<string> { ".sql", ".frm" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 29,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Objective-C",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Objective-C language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "objc",
                ProgrammingLanguageFilesExtensions = new List<string> { ".m", ".mm" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 30,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "PostgreSQL",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "PostgreSQL language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "pgsql",
                ProgrammingLanguageFilesExtensions = new List<string> { ".sql" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 31,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "PHP",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "PHP language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "php",
                ProgrammingLanguageFilesExtensions = new List<string> { ".php" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 32,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "ATS",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "ATS (Applied Type System) language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "ats",
                ProgrammingLanguageFilesExtensions = new List<string> { },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 33,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "PowerShell",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "PowerShell language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "powershell",
                ProgrammingLanguageFilesExtensions = new List<string> { ".ps1", ".psc1" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 34,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Pug (HTML templating)",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Pug templating language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "pug",
                ProgrammingLanguageFilesExtensions = new List<string> { ".pug" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 35,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "R",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "R language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "r",
                ProgrammingLanguageFilesExtensions = new List<string> { ".r", ".R" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 36,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "ASP.NET Razor",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "ASP.NET Razor language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "razor",
                ProgrammingLanguageFilesExtensions = new List<string> { ".cshtml", ".vbhtml" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 37,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Redis",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Redis language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "redis",
                ProgrammingLanguageFilesExtensions = new List<string> { ".rdb" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 38,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Amazon Redshift",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Amazon Redshift language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "redshift",
                ProgrammingLanguageFilesExtensions = new List<string> { ".sql" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 39,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Scratch",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Scratch language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "scratch",
                ProgrammingLanguageFilesExtensions = new List<string> { ".sb", ".sb2" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 40,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Sass / Scss",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Sass / Scss language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "sass",
                ProgrammingLanguageFilesExtensions = new List<string> { ".sass", ".scss" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 41,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Solidity",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Solidity language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "solidity",
                ProgrammingLanguageFilesExtensions = new List<string> { ".sol" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 42,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "SQL",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "SQL language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "sql",
                ProgrammingLanguageFilesExtensions = new List<string> { ".sql" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 43,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Swift",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Swift language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "swift",
                ProgrammingLanguageFilesExtensions = new List<string> { ".swift" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 44,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "VB(.net)",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "VB(.net) language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "vb",
                ProgrammingLanguageFilesExtensions = new List<string> { ".vb" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 45,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "XML",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "XML language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "xml",
                ProgrammingLanguageFilesExtensions = new List<string> { ".xml" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 46,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "YAML",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "YAML language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "yaml",
                ProgrammingLanguageFilesExtensions = new List<string> { ".yaml" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            new InfosModule
            {
                ID = 47,
                ModuleType = ModuleTypesList.ProgrammingLanguage,
                ModuleName = "Text tab (default)",
                ModuleSystem = true,
                ModuleAuthor = "Microsoft",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "Default tab language",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                ProgrammingLanguageMonacoDefinitionName = "txt",
                ProgrammingLanguageFilesExtensions = new List<string> { ".txt" },
                ProgrammingLanguageMonacoCompletionAvailable = false
            },

            /* =============
             * = TEMPLATES =
             * =============
            */
            
            new InfosModule
            {
                ID = 48,
                ModuleType = ModuleTypesList.Templates,
                ModuleName = "Default list",
                ModuleSystem = true,
                ModuleAuthor = "[SP] DeerisLeGris",
                ModuleVersion = new ModuleVersion
                {
                    Major = 1,
                    Minor = 0,
                    Revision = 0
                },
                ModuleDescription = "The default list",
                CanBePinnedToToolBar = false,
                IsEnabled = true,
                JSFilesPathList = new List<string>(),
                TemplateContainProjectTemplate = false,
                TemplateContainTemplateFiles = true,
                TemplateProjectTypeName = "Default",
                TemplateFilesInfos = new List<TemplatesFileInfos>
                {
                    new TemplatesFileInfos { Name = "C# class", Description = "An default C# class", SuggestedTemplateName = "sample.cs", TemplateFileModulePath = "class.cs"  }
                }
            }
        };
    }
}
