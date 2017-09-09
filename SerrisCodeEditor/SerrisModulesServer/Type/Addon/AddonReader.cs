using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisModulesServer.Type.Addon
{
    public class AddonReader
    {
        StorageFile file; ModulesAccessManager AccessManager;

        public AddonReader()
        {
            file = AsyncHelpers.RunSync<StorageFile>(() => ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists).AsTask());
            AccessManager = new ModulesAccessManager();
        }

        public async Task<string> GetAddonMainJsViaIDAsync(int id, bool IsSystemModule)
        {
            StorageFolder folder_module;

            if (IsSystemModule)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"), folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(await AccessManager.GetCurrentThemeID() + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(await AccessManager.GetCurrentThemeID() + "", CreationCollisionOption.OpenIfExists);
            }

            StorageFile file_content = await folder_module.GetFileAsync("main.js");

            try
            {
                using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch
            {
                return "";
            }

        }

        public async Task<BitmapImage> GetAddonIconViaIDAsync(int id, bool IsSystemModule)
        {
            StorageFolder folder_module;

            if (IsSystemModule)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"), folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(await AccessManager.GetCurrentThemeID() + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(await AccessManager.GetCurrentThemeID() + "", CreationCollisionOption.OpenIfExists);
            }

            StorageFile file_content = await folder_module.GetFileAsync("icon.png");

            try
            {
                using (FileRandomAccessStream reader = (FileRandomAccessStream)await file_content.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(reader);

                    return bitmapImage;
                }
            }
            catch
            {
                return null;
            }

        }
    }
}
