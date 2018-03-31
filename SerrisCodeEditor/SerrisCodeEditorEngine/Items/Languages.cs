using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.ProgrammingLanguage;
using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Controls;

namespace SerrisCodeEditorEngine.Items
{
    public static class Languages
    {
        private static List<string> LanguagesAlreadyLoaded = new List<string>();

        /*
        *       ========
        *       FUNCTION
        *       ========
        */

        private static async void LoadLanguageInTheEditor(string Name, WebView Editor)
        {
            if(!LanguagesAlreadyLoaded.Contains(Name))
            {
                foreach(InfosModule Module in ModulesAccessManager.GetSpecificModules(true, SerrisModulesServer.Type.ModuleTypesList.ProgrammingLanguage))
                {
                    if (Module.ProgrammingLanguageMonacoDefinitionName == Name)
                    {
                        await Editor.InvokeScriptAsync("eval", new[] { "monaco.languages.register({ id:'" + Module.ProgrammingLanguageMonacoDefinitionName + "'});" });

                        await Editor.InvokeScriptAsync("eval", new[] { await new ProgrammingLanguageReader(Module.ID).GetLanguageDefinitionContent() });

                        if(Module.ProgrammingLanguageMonacoCompletionAvailable)
                        {
                            await Editor.InvokeScriptAsync("eval", new[] { await new ProgrammingLanguageReader(Module.ID).GetLanguageCompletionContent() });
                        }

                        break;
                    }
                }

                LanguagesAlreadyLoaded.Add(Name.ToLower());
            }
        }

        public static async void GetActualLanguage(string CodeLanguage, WebView editor)
        {
            System.Diagnostics.Debug.WriteLine(CodeLanguage);
            LoadLanguageInTheEditor(CodeLanguage, editor);
            await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), '" + CodeLanguage + "');" });

        }

    }
}
