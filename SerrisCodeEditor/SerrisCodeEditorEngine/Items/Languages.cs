using System;
using Windows.UI.Xaml.Controls;

namespace SerrisCodeEditorEngine.Items
{
    public static class Languages
    {
        /*
        *       ========
        *       FUNCTION
        *       ========
        */

        public static async void GetActualLanguage(string CodeLanguage, WebView editor)
        {
            switch (CodeLanguage)
            {

                case "HTM":
                case "HTML":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'html');" });
                    break;

                case "PHP":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'php');" });
                    break;

                case "CSS":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'css');" });
                    break;

                case "JS":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'javascript');" });
                    break;

                case "XML":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'xml');" });
                    break;

                case "ASPNET":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'razor');" });
                    break;

                case "PYTHON":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'python');" });
                    break;

                case "SCSS":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'scss');" });
                    break;

                case "CLASS":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'java');" });
                    break;

                case "CS":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'csharp');" });
                    break;

                case "LUA":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'lua');" });
                    break;

                case "JSON":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'json');" });
                    break;

                case "C_CPP":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'cpp');" });
                    break;

                case "OBJ_C":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'objective-c');" });
                    break;

                case "COFFEE":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'coffee');" });
                    break;

                case "PERL":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'json');" });
                    break;

                case "RUBY":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'ruby');" });
                    break;

                case "TS":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'typescript');" });
                    break;

                case "YAML":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'yaml');" });
                    break;

                case "SWIFT":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'swift');" });
                    break;

                case "VBSCRIPT":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'vb');" });
                    break;

                case "SH":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'shell');" });
                    break;

                case "PS1":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'powershell');" });
                    break;

                case "BAT":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'bat');" });
                    break;

                case "SQL":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'sql');" });
                    break;

                case "VB":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'vb');" });
                    break;

                case "INI":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'ini');" });
                    break;

                case "FS":
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'fsharp');" });
                    break;

                default:
                    await editor.InvokeScriptAsync("eval", new[] { "monaco.editor.setModelLanguage(editor.getModel(), 'plaintext');" });
                    break;
            }

        }

    }
}
