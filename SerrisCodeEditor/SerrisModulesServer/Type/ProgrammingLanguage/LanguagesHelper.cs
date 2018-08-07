using SerrisModulesServer.Manager;
using System.Collections.Generic;
using System.IO;

namespace SerrisModulesServer.Type.ProgrammingLanguage
{
    public static class LanguagesHelper
    {
        public static readonly int DefaultLanguageModuleID = 47;

        public static string GetLanguageType(string Filename)
        {
            string Extension = Path.GetExtension(Filename);

            foreach(var Module in ModulesAccessManager.GetSpecificModules(true, ModuleTypesList.ProgrammingLanguage))
            {
                if(Module.ProgrammingLanguageFilesExtensions.Contains(Extension))
                {
                    return Module.ProgrammingLanguageMonacoDefinitionName;
                }
                else
                {
                    continue;
                }
            }

            return "txt";
        }

        public static bool IsFileLanguageIsCompatible(string Filename)
        {
            string Extension = Path.GetExtension(Filename);

            foreach (var Module in ModulesAccessManager.GetSpecificModules(true, ModuleTypesList.ProgrammingLanguage))
            {
                if (Module.ProgrammingLanguageFilesExtensions.Contains(Extension))
                {
                    return true;
                }
                else
                {
                    continue;
                }
            }

            return false;
        }

        public static int GetModuleIDOfLangageType(string LangType)
        {
            string Type = LangType.ToLower();

            foreach (var Module in ModulesAccessManager.GetSpecificModules(true, ModuleTypesList.ProgrammingLanguage))
            {
                if (Module.ProgrammingLanguageMonacoDefinitionName == Type)
                {
                    return Module.ID;
                }
                else
                {
                    continue;
                }
            }

            return DefaultLanguageModuleID;
        }

        public static List<string> GetLanguageExtensions(string Filetype)
        {
            foreach (var Module in ModulesAccessManager.GetSpecificModules(true, ModuleTypesList.ProgrammingLanguage))
            {
                if (Module.ProgrammingLanguageMonacoDefinitionName == Filetype)
                {
                    return Module.ProgrammingLanguageFilesExtensions;
                }
                else
                {
                    continue;
                }
            }

            return new List<string> { ".txt" };
        }

        public static List<string> GetLanguagesNames()
        {
            List<string> LanguagesAvailable = new List<string>();

            foreach (var Module in ModulesAccessManager.GetSpecificModules(true, ModuleTypesList.ProgrammingLanguage))
            {
                LanguagesAvailable.Add(Module.ModuleName);
            }

            return LanguagesAvailable;
        }

        public static string GetLanguageTypeViaName(string LanguageName)
        {
            foreach (var Module in ModulesAccessManager.GetSpecificModules(true, ModuleTypesList.ProgrammingLanguage))
            {
                if (Module.ModuleName == LanguageName)
                {
                    return Module.ProgrammingLanguageMonacoDefinitionName;
                }
                else
                {
                    continue;
                }
            }

            return "txt";
        }

        public static string GetLanguageNameViaType(string Type)
        {
            foreach (var Module in ModulesAccessManager.GetSpecificModules(true, ModuleTypesList.ProgrammingLanguage))
            {
                if (Module.ProgrammingLanguageMonacoDefinitionName == Type)
                {
                    return Module.ModuleName;
                }
                else
                {
                    continue;
                }
            }

            return "txt";
        }
    }
}
