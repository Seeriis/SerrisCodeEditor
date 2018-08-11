using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using SerrisTabsServer.Storage.StorageTypes;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Storage
{
    public enum StorageListTypes
    {
        Nothing,
        LocalStorage,
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

                case StorageListTypes.OneDrive:
                    await new OneDrive(tab, IdList).CreateFile();
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

                case StorageListTypes.OneDrive:
                    new OneDrive(tab, IdList).DeleteFile();
                    break;
            }
        }

        public async Task<bool> ReadFile(bool ReplaceEncoding)
        {
            switch (tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    return await new LocalStorage(tab, IdList).ReadFile(ReplaceEncoding);

                case StorageListTypes.OneDrive:
                    return await new OneDrive(tab, IdList).ReadFile(ReplaceEncoding);
            }

            return false;
        }

        public async Task<string> ReadFileAndGetContent()
        {
            switch (tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    return await new LocalStorage(tab, IdList).ReadFileAndGetContent();

                case StorageListTypes.OneDrive:
                    return await new OneDrive(tab, IdList).ReadFileAndGetContent();

                default:
                    return "";
            }
        }

        public async Task WriteFile()
        {
            switch (tab.TabStorageMode)
            {
                case StorageListTypes.LocalStorage:
                    await new LocalStorage(tab, IdList).WriteFile();
                    break;

                case StorageListTypes.OneDrive:
                    await new OneDrive(tab, IdList).WriteFile();
                    break;
            }

            tab.TabNewModifications = false;
            await TabsWriteManager.PushUpdateTabAsync(tab, IdList, false);
        }

    }
}
