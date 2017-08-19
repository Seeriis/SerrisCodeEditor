using SerrisTabsServer.Items;
using SerrisTabsServer.Storage.StorageTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Storage
{
    public enum StorageListTypes
    {
        Nothing,
        LocalStorage,
        Network,
        OneDrive
    }

    public class StorageRouter
    {
        public InfosTab tab; public int IdList;
        public StorageRouter(InfosTab _tab, int id_list)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            tab = _tab; IdList = id_list;
        }

        public async Task CreateFile()
        {
            switch (tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    await new LocalStorage(tab, IdList).CreateFile();
                    break;

                case StorageListTypes.Network:
                    break;

                case StorageListTypes.OneDrive:
                    break;
            }
        }

        public void DeleteFile()
        {
            switch (tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    new LocalStorage(tab, IdList).DeleteFile();
                    break;

                case StorageListTypes.Network:
                    break;

                case StorageListTypes.OneDrive:
                    break;
            }
        }

        public void ReadFile(bool ReplaceEncoding)
        {
            switch (tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    new LocalStorage(tab, IdList).ReadFile(ReplaceEncoding);
                    break;

                case StorageListTypes.Network:
                    break;

                case StorageListTypes.OneDrive:
                    break;
            }
        }

        public async Task<String> ReadFileAndGetContent()
        {
            switch (tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    return await new LocalStorage(tab, IdList).ReadFileAndGetContent();

                case StorageListTypes.Network:
                    return "";

                case StorageListTypes.OneDrive:
                    return "";

                default:
                    return "";
            }
        }

        public async Task WriteFile()
        {
            switch(tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    await new LocalStorage(tab, IdList).WriteFile();
                    break;

                case StorageListTypes.Network:
                    break;

                case StorageListTypes.OneDrive:
                    break;
            }
        }

    }
}
