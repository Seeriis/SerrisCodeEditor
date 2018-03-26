using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisModulesServer.Type
{
    public abstract class ModuleReader
    {
        public ModuleTypesList ModuleType;
        public int ModuleID;
        public bool IsSystemModule;
        public string ModuleFolderPath
        {
            get => ModulesAccessManager.GetModuleFolderPath(ModuleID, IsSystemModule);
        }

        protected ModuleReader(int ID)
        {
            var ModuleContent = ModulesAccessManager.GetModuleViaID(ID);

            ModuleID = ID;
            IsSystemModule = ModuleContent.ModuleSystem;
            ModuleType = ModuleContent.ModuleType;
        }
    }
}
