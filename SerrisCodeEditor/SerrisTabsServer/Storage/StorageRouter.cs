using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Storage
{
    public enum StorageTypes
    {
        Nothing,
        LocalStorage,
        Network,
        OneDrive
    }

    public class StorageRouter
    {
        public InfosTab tab;
        public StorageRouter(InfosTab _tab)
        {
            tab = _tab;
        }

        public void CreateFile()
        {
            switch (tab.TabStorageMode)
            {
                case StorageTypes.LocalStorage:
                    break;

                case StorageTypes.Network:
                    break;

                case StorageTypes.OneDrive:
                    break;
            }
        }

        public void WriteFile()
        {
            switch(tab.TabStorageMode)
            {
                case StorageTypes.LocalStorage:
                    break;

                case StorageTypes.Network:
                    break;

                case StorageTypes.OneDrive:
                    break;
            }
        }

        public string ReadFile()
        {
            switch (tab.TabStorageMode)
            {
                case StorageTypes.LocalStorage:
                    break;

                case StorageTypes.Network:
                    break;

                case StorageTypes.OneDrive:
                    break;
            }

            return "";
        }

    }
}
