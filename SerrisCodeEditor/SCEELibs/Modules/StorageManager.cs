using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace SCEELibs.Modules
{
    [AllowForWeb]
    public sealed class StorageManager
    {
        int currentID;

        public StorageManager(int id)
        => currentID = id;

        public IAsyncOperation<StorageFolder> getTemporaryFolder()
        {
            return Task.Run(() =>
            {

                return ApplicationData.Current.TemporaryFolder;

            }).AsAsyncOperation();
        }

        public IAsyncOperation<StorageFolder> getModuleFolder()
        {
            return Task.Run(async () =>
            {

                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                return await folder_content.CreateFolderAsync(currentID + "", CreationCollisionOption.OpenIfExists);

            }).AsAsyncOperation();
        }

    }
}
