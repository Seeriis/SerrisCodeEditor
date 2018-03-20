using System.Collections.Generic;

namespace SerrisTabsServer.Manager
{
    public static class FileTypesManager
    {
        public static List<string> List_Type = new List<string> { "AS", "ASM", "BAT", "CSS", "C", "CPP", "CC", "CXX", "CLASS", "CS", "CSHTML", "CBL", "COB", "CPY", "COFFEE", "CFM", "CFC", "E", "EX", "EXS", "GROOVY", "HTML", "HX", "HXML", "HAML", "H", "HPP", "HXX", "INC", "JS", "JSON", "JSP", "LUA", "LITCOFFEE", "M", "MM", "ML", "MLI", "PP", "PAS", "PHP", "PY", "PYC", "PYD", "PYO", "PYW", "PYZ", "PL", "PM", "POD", "PS1", "RB", "RBW", "SASS", "SH", "SCSS", "SQL", "SWIFT", "T", "TS", "TEX", "TXT", "VB", "VBHTML", "VBS", "VBE", "WSF", "WSC", "XML", "YAML", "YML", "INI", "FS" };
        public static List<string> List_Type_extensions = new List<string> { ".html", ".htm", ".css", ".php", ".js", ".scss", ".xml", ".cshtml", ".vbhtml", ".py", ".pyc", ".pyd", ".pyo", ".pyw", ".pyz", ".as", ".class", ".cs", ".lua", ".json", ".c", ".h", ".cpp", ".cc", ".hpp", ".hxx", ".cxx", ".asm", ".m", ".mm", ".coffee", ".litcoffee", ".pl", ".pm", ".t", ".pod", ".pp", ".pas", ".inc", ".rb", ".rbw", ".ts", ".yaml", ".yml", ".swift", ".vbs", ".vbe", ".wsf", ".wsc", ".hx", ".hxml", ".haml", ".cbl", ".cob", ".cpy", ".sass", ".ml", ".mli", ".tex", ".groovy", ".sh", ".ps1", ".ex", ".exs", ".cfm", ".cfc", ".jsp", ".bat", ".e", ".sql", ".txt", ".vb", ".ini", ".fs" };

        //TYPES
        public static List<string> Type_HTML = new List<string> { ".html", ".htm" };
        public static List<string> Type_ASPNET = new List<string> { ".cshtml", ".vbhtml" };
        public static List<string> Type_PYTHON = new List<string> { ".py", ".pyc", ".pyd", ".pyo", ".pyw", ".pyz" };
        public static List<string> Type_C_CPP = new List<string> { ".c", ".h", ".cpp", ".cc", ".hpp", ".hxx", ".cxx" };
        public static List<string> Type_OBJ_C = new List<string> { ".m", ".mm" };
        public static List<string> Type_COFFEE = new List<string> { ".coffee", ".litcoffee" };
        public static List<string> Type_PERL = new List<string> { ".pl", ".pm", ".t", ".pod" };
        public static List<string> Type_PASCAL = new List<string> { ".pp", ".pas", ".inc" };
        public static List<string> Type_RUBY = new List<string> { ".rb", ".rbw" };
        public static List<string> Type_YAML = new List<string> { ".yaml", ".yml" };
        public static List<string> Type_VBSCRIPT = new List<string> { ".vbs", ".vbe", ".wsf", ".wsc" };
        public static List<string> Type_HAXE = new List<string> { ".hx", ".hxml" };
        public static List<string> Type_COBOL = new List<string> { ".cbl", ".cob", ".cpy" };
        public static List<string> Type_OCAML = new List<string> { ".ml", ".mli" };
        public static List<string> Type_ELIXIR = new List<string> { ".ex", ".exs" };
        public static List<string> Type_CFML = new List<string> { ".cfm", ".cfc" };

        public static string GetExtensionType(string fileextension)
        {
            string extension = fileextension.Replace(".", "");

            if (Type_HTML.Contains(fileextension))
            {
                extension = "html";
            }
            else if (Type_ASPNET.Contains(fileextension))
            {
                extension = "aspnet";
            }
            else if (Type_PYTHON.Contains(fileextension))
            {
                extension = "python";
            }
            else if (Type_C_CPP.Contains(fileextension))
            {
                extension = "c_cpp";
            }
            else if (Type_OBJ_C.Contains(fileextension))
            {
                extension = "obj_c";
            }
            else if (Type_COFFEE.Contains(fileextension))
            {
                extension = "coffee";
            }
            else if (Type_PERL.Contains(fileextension))
            {
                extension = "perl";
            }
            else if (Type_PASCAL.Contains(fileextension))
            {
                extension = "pascal";
            }
            else if (Type_RUBY.Contains(fileextension))
            {
                extension = "ruby";
            }
            else if (Type_YAML.Contains(fileextension))
            {
                extension = "yaml";
            }
            else if (Type_VBSCRIPT.Contains(fileextension))
            {
                extension = "vbscript";
            }
            else if (Type_HAXE.Contains(fileextension))
            {
                extension = "haxe";
            }
            else if (Type_COBOL.Contains(fileextension))
            {
                extension = "cobol";
            }
            else if (Type_OCAML.Contains(fileextension))
            {
                extension = "ocaml";
            }
            else if (Type_ELIXIR.Contains(fileextension))
            {
                extension = "elixir";
            }
            else if (Type_CFML.Contains(fileextension))
            {
                extension = "cfml";
            }

            return extension;
        }

        public static string GetExtension(string filetype)
        {
            string filetype_ = filetype.ToLower();

            if (filetype_.Contains("html"))
            {
                filetype_ = Type_HTML[0].Replace(".", "");
            }
            else if (filetype_.Contains("aspnet"))
            {
                filetype_ = Type_ASPNET[0].Replace(".", "");
            }
            else if (filetype_.Contains("python"))
            {
                filetype_ = Type_PYTHON[0].Replace(".", "");
            }
            else if (filetype_.Contains("c_cpp"))
            {
                filetype_ = Type_C_CPP[0].Replace(".", "");
            }
            else if (filetype_.Contains("obj_c"))
            {
                filetype_ = Type_OBJ_C[0].Replace(".", "");
            }
            else if (filetype_.Contains("coffee"))
            {
                filetype_ = Type_COFFEE[0].Replace(".", "");
            }
            else if (filetype_.Contains("perl"))
            {
                filetype_ = Type_PERL[0].Replace(".", "");
            }
            else if (filetype_.Contains("pascal"))
            {
                filetype_ = Type_PASCAL[0].Replace(".", "");
            }
            else if (filetype_.Contains("ruby"))
            {
                filetype_ = Type_RUBY[0].Replace(".", "");
            }
            else if (filetype_.Contains("yaml"))
            {
                filetype_ = Type_YAML[0].Replace(".", "");
            }
            else if (filetype_.Contains("vbscript"))
            {
                filetype_ = Type_VBSCRIPT[0].Replace(".", "");
            }
            else if (filetype_.Contains("haxe"))
            {
                filetype_ = Type_HAXE[0].Replace(".", "");
            }
            else if (filetype_.Contains("cobol"))
            {
                filetype_ = Type_COBOL[0].Replace(".", "");
            }
            else if (filetype_.Contains("ocaml"))
            {
                filetype_ = Type_OCAML[0].Replace(".", "");
            }
            else if (filetype_.Contains("elixir"))
            {
                filetype_ = Type_ELIXIR[0].Replace(".", "");
            }
            else if (filetype_.Contains("cfml"))
            {
                filetype_ = Type_CFML[0].Replace(".", "");
            }

            return filetype_;
        }

        public static bool FileIsSupported(string extension)
        => List_Type_extensions.Contains(extension);

    }
}
