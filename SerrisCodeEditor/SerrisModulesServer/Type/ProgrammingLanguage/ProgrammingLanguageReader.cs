using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SerrisModulesServer.Type.ProgrammingLanguage
{
    public class ProgrammingLanguageReader : ModuleReader
    {
        public ProgrammingLanguageReader(int ID) : base(ID) { }

        public async Task<string> GetLanguageDefinitionContent()
        {
            try
            {
                StorageFile LanguageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModuleFolderPath + "language.js"));
                return "var content = function() { " + await FileIO.ReadTextAsync(LanguageFile) + " }; monaco.languages.setMonarchTokensProvider('" + ModulesAccessManager.GetModuleViaID(ModuleID).ProgrammingLanguageMonacoDefinitionName + "', content());";
            }
            catch { return ""; }
            
        }

        public async Task<string> GetLanguageCompletionContent()
        {
            try
            {
                StorageFile LanguageFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModuleFolderPath + "completion.js"));
                return await FileIO.ReadTextAsync(LanguageFile);
            }
            catch { return ""; }

        }
    }
}
