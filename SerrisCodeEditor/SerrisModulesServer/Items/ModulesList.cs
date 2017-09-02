using System.Collections.Generic;

namespace SerrisModulesServer.Items
{
    public class ModulesList
    {
        public int CurrentThemeID { get; set; }
        public int CurrentThemeAceID { get; set; }
        public List<InfosModule> Modules { get; set; }
    }
}
