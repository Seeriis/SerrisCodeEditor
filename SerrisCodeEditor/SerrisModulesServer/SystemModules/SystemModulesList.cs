using SerrisModulesServer.Items;
using SerrisModulesServer.Type;
using System.Collections.Generic;

namespace SerrisModulesServer.SystemModules
{
    public class SystemModulesList
    {
        public List<InfosModule> Modules = new List<InfosModule>
        {
            new InfosModule { ID = 0, ModuleType = ModuleTypesList.Theme, ContainMonacoTheme = true, ModuleName = "SCE - Light theme", ModuleSystem = true, ModuleAuthor = "[SP] DeerisLeGris", ModuleVersion = new ModuleVersion { Major = 1, Minor = 0, Revision = 0 }, ModuleDescription = "Default light theme of SCE !", IsPinnedToToolBar = false, IsEnabled = true  }
        };
    }
}
