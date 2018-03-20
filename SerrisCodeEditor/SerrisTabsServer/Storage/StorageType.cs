using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;

namespace SerrisTabsServer.Storage
{
    public class StorageType
    {

        public StorageType(InfosTab tab, int _ListTabsID) { }

        public InfosTab Tab { get; set; }
        public int ListTabsID { get; set; }

    }
}
