using System.Collections.Generic;

namespace SerrisModulesServer.Items
{
    public class ModulesList
    {
        public string CurrentThemeID { get; set; }
        public string CurrentThemeMonacoID { get; set; }
        public List<InfosModule> Modules { get; set; }
    }
}
