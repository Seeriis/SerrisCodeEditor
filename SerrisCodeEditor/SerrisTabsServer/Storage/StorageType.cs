using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Storage
{
    abstract class StorageType
    {
        public InfosTab tab;
        public StorageType(InfosTab _tab)
        {
            tab = _tab;
        }

        public abstract void CreateFile();
        public abstract void WriteFile();
        public abstract string ReadFile();
    }
}
