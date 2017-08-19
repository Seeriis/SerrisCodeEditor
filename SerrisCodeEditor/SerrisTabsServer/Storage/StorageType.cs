using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Storage
{
    public class StorageType
    {

        public StorageType(InfosTab tab, int _ListTabsID) { }

        public InfosTab Tab { get; set; }
        public int ListTabsID { get; set; }
        public TabsAccessManager TabsReader = new TabsAccessManager();
        public TabsWriteManager TabsWriter = new TabsWriteManager();
        public FileTypesManager FileTypes = new FileTypesManager();

    }
}
