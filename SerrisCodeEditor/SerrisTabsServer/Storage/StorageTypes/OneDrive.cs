using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Storage.StorageTypes
{
    public class OneDrive : StorageType
    {
        public OneDrive(InfosTab tab, int _ListTabsID) : base(tab, _ListTabsID)
        {
            Tab = tab; ListTabsID = _ListTabsID;
        }

    }
}
