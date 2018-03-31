using SerrisModulesServer.Manager;
using System.Collections.Generic;
using System.IO;

namespace SerrisModulesServer.Type.ProgrammingLanguage
{
    public static class LanguagesHelper
    {

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

    }
}
