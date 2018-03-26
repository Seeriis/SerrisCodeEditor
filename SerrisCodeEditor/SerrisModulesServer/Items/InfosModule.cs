using SerrisModulesServer.Type;
using System.Collections.Generic;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisModulesServer.Items
{

    public struct ModuleVersion
    {
        public int Major { get; set; } //Major revision (new UI, lots of new features, conceptual change, etc.)
        public int Minor { get; set; } //Minor revision (maybe a change to a search box, 1 feature added, collection of bug fixes)
        public int Revision { get; set; } //Bug fix release

        public string GetVersionInString()
        {
            return string.Format("{0}.{1}.{2}", Major, Minor, Revision);
        }
    }

    public class InfosModule
    {
        public int ID { get; set; }

        public ModuleVersion ModuleVersion { get; set; }
        public string ModuleName { get; set; }
        public ModuleTypesList ModuleType { get; set; }
        public string ModuleAuthor { get; set; }
        public string ModuleWebsiteLink { get; set; }
        public string ModuleDescription { get; set; }

        //"Addon" module
        public List<string> JSFilesPathList { get; set; }

        //"Theme" module
        public bool ContainMonacoTheme { get; set; }
        public string ModuleMonacoThemeName { get; set; }

        //"ProgrammingLanguage" module
        public bool ProgrammingLanguageMonacoCompletionAvailable { get; set; }
        public string ProgrammingLanguageMonacoDefinitionName { get; set; }
        public List<string> ProgrammingLanguageFilesExtensions { get; set; }

        //"ProjectType" module
        public List<string> ProjectTypeDefaultFilesPath { get; set; }

        public float SceMinimalVersionRequired { get; set; }
        public bool ModuleSystem { get; set; }
        public bool CanBePinnedToToolBar { get; set; }
        public bool IsEnabled { get; set; }
    }

    public sealed class PinnedModule
    {
        public int ID { get; set; }

        public string ModuleName { get; set; }
        public ModuleTypesList ModuleType { get; set; }

        public BitmapImage Image { get; set; }
    }
}
