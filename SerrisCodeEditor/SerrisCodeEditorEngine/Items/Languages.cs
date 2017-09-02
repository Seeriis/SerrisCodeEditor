using System;
using Windows.UI.Xaml.Controls;

namespace SerrisCodeEditorEngine.Items
{
    public class Languages
    {
        /*
        *       =========
        *       LANGUAGES
        *       =========
        */

        string[] plain_text = { "monaco.editor.setModelLanguage(editor.getModel(), 'plaintext');" };
        string[] html = { "monaco.editor.setModelLanguage(editor.getModel(), 'html');" };
        string[] php = { "monaco.editor.setModelLanguage(editor.getModel(), 'php');" };
        string[] css = { "monaco.editor.setModelLanguage(editor.getModel(), 'css');" };
        string[] js = { "monaco.editor.setModelLanguage(editor.getModel(), 'javascript');" };
        string[] scss = { "monaco.editor.setModelLanguage(editor.getModel(), 'scss');" };
        string[] xml = { "monaco.editor.setModelLanguage(editor.getModel(), 'xml');" };
        string[] aspnet = { "monaco.editor.setModelLanguage(editor.getModel(), 'razor');" };
        string[] python = { "monaco.editor.setModelLanguage(editor.getModel(), 'python');" };
        string[] actionscript = { "editor.getSession().setMode('ace/mode/actionscript'); change = 0;" };
        string[] java = { "monaco.editor.setModelLanguage(editor.getModel(), 'java');" };
        string[] csharp = { "monaco.editor.setModelLanguage(editor.getModel(), 'csharp');" };
        string[] lua = { "monaco.editor.setModelLanguage(editor.getModel(), 'lua');" };
        string[] json = { "monaco.editor.setModelLanguage(editor.getModel(), 'json');" };
        string[] c_cpp = { "monaco.editor.setModelLanguage(editor.getModel(), 'cpp');" };
        string[] assembly = { "editor.getSession().setMode('ace/mode/assembly_x86'); change = 0;" };
        string[] objective_c = { "monaco.editor.setModelLanguage(editor.getModel(), 'objective-c');" };
        string[] coffee = { "monaco.editor.setModelLanguage(editor.getModel(), 'coffee');" };
        string[] perl = { "monaco.editor.setModelLanguage(editor.getModel(), 'json');" };
        string[] pascal = { "editor.getSession().setMode('ace/mode/pascal'); change = 0;" };
        string[] ruby = { "editor.getSession().setMode('ace/mode/ruby'); change = 0;" };
        string[] typescript = { "editor.getSession().setMode('ace/mode/typescript'); change = 0;" };
        string[] yaml = { "monaco.editor.setModelLanguage(editor.getModel(), 'yaml');" };
        string[] swift = { "monaco.editor.setModelLanguage(editor.getModel(), 'swift');" };
        string[] vbscript = { "editor.getSession().setMode('ace/mode/vbscript'); change = 0;" };
        string[] haxe = { "editor.getSession().setMode('ace/mode/haxe'); change = 0;" };
        string[] haml = { "editor.getSession().setMode('ace/mode/haml'); change = 0;" };
        string[] cobol = { "editor.getSession().setMode('ace/mode/cobol'); change = 0;" };
        string[] sass = { "editor.getSession().setMode('ace/mode/sass'); change = 0;" };
        string[] ocaml = { "editor.getSession().setMode('ace/mode/ocaml'); change = 0;" };
        string[] latex = { "editor.getSession().setMode('ace/mode/latex'); change = 0;" };
        string[] groovy = { "editor.getSession().setMode('ace/mode/groovy'); change = 0;" };
        string[] shell = { "editor.getSession().setMode('ace/mode/sh'); change = 0;" };
        string[] powershell = { "monaco.editor.setModelLanguage(editor.getModel(), 'powershell');" };
        string[] elixir = { "editor.getSession().setMode('ace/mode/elixir'); change = 0;" };
        string[] cfml = { "editor.getSession().setMode('ace/mode/coldfusion'); change = 0;" };
        string[] jsp = { "editor.getSession().setMode('ace/mode/jsp'); change = 0;" };
        string[] batch = { "monaco.editor.setModelLanguage(editor.getModel(), 'bat');" };
        string[] eiffel = { "editor.getSession().setMode('ace/mode/eiffel'); change = 0;" };
        string[] sql = { "monaco.editor.setModelLanguage(editor.getModel(), 'sql');" };



        /*
        *       ========
        *       FUNCTION
        *       ========
        */

        public async void GetActualLanguage(string CodeLanguage, WebView editor)
        {
            switch (CodeLanguage)
            {

                case "HTML":
                    await editor.InvokeScriptAsync("eval", html);
                    break;

                case "HTM":
                    await editor.InvokeScriptAsync("eval", html);
                    break;

                case "PHP":
                    await editor.InvokeScriptAsync("eval", php);
                    break;

                case "CSS":
                    await editor.InvokeScriptAsync("eval", css);
                    break;

                case "JS":
                    await editor.InvokeScriptAsync("eval", js);
                    break;

                case "XML":
                    await editor.InvokeScriptAsync("eval", xml);
                    break;

                case "ASPNET":
                    await editor.InvokeScriptAsync("eval", aspnet);
                    break;

                case "PYTHON":
                    await editor.InvokeScriptAsync("eval", python);
                    break;

                case "SCSS":
                    await editor.InvokeScriptAsync("eval", scss);
                    break;

                case "AS":
                    await editor.InvokeScriptAsync("eval", actionscript);
                    break;

                case "CLASS":
                    await editor.InvokeScriptAsync("eval", java);
                    break;

                case "CS":
                    await editor.InvokeScriptAsync("eval", csharp);
                    break;

                case "LUA":
                    await editor.InvokeScriptAsync("eval", lua);
                    break;

                case "JSON":
                    await editor.InvokeScriptAsync("eval", json);
                    break;

                case "C_CPP":
                    await editor.InvokeScriptAsync("eval", c_cpp);
                    break;

                case "ASM":
                    await editor.InvokeScriptAsync("eval", assembly);
                    break;

                case "OBJ_C":
                    await editor.InvokeScriptAsync("eval", objective_c);
                    break;

                case "COFFEE":
                    await editor.InvokeScriptAsync("eval", coffee);
                    break;

                case "PERL":
                    await editor.InvokeScriptAsync("eval", perl);
                    break;

                case "PASCAL":
                    await editor.InvokeScriptAsync("eval", pascal);
                    break;

                case "RUBY":
                    await editor.InvokeScriptAsync("eval", ruby);
                    break;

                case "TS":
                    await editor.InvokeScriptAsync("eval", typescript);
                    break;

                case "YAML":
                    await editor.InvokeScriptAsync("eval", yaml);
                    break;

                case "SWIFT":
                    await editor.InvokeScriptAsync("eval", swift);
                    break;

                case "VBSCRIPT":
                    await editor.InvokeScriptAsync("eval", vbscript);
                    break;

                case "HAXE":
                    await editor.InvokeScriptAsync("eval", haxe);
                    break;

                case "HAML":
                    await editor.InvokeScriptAsync("eval", haml);
                    break;

                case "COBOL":
                    await editor.InvokeScriptAsync("eval", cobol);
                    break;

                case "SASS":
                    await editor.InvokeScriptAsync("eval", sass);
                    break;

                case "OCAML":
                    await editor.InvokeScriptAsync("eval", ocaml);
                    break;

                case "TEX":
                    await editor.InvokeScriptAsync("eval", latex);
                    break;

                case "GROOVY":
                    await editor.InvokeScriptAsync("eval", groovy);
                    break;

                case "SH":
                    await editor.InvokeScriptAsync("eval", shell);
                    break;

                case "PS1":
                    await editor.InvokeScriptAsync("eval", powershell);
                    break;

                case "ELIXIR":
                    await editor.InvokeScriptAsync("eval", elixir);
                    break;

                case "CFML":
                    await editor.InvokeScriptAsync("eval", cfml);
                    break;

                case "JSP":
                    await editor.InvokeScriptAsync("eval", jsp);
                    break;

                case "BAT":
                    await editor.InvokeScriptAsync("eval", batch);
                    break;

                case "E":
                    await editor.InvokeScriptAsync("eval", eiffel);
                    break;

                case "SQL":
                    await editor.InvokeScriptAsync("eval", sql);
                    break;

                case "VB":
                    await editor.InvokeScriptAsync("eval", new string[] { "monaco.editor.setModelLanguage(editor.getModel(), 'vb');" });
                    break;

                default:
                    await editor.InvokeScriptAsync("eval", plain_text);
                    break;
            }

        }

    }
}
