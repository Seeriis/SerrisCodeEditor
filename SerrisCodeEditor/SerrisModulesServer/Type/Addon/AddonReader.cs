using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisModulesServer.Type.Addon
{
    public class AddonReader
    {
        ModulesAccessManager AccessManager;
        int id_module; bool system_module;

        public AddonReader(int ID)
        {
            AccessManager = new ModulesAccessManager();

            id_module = ID;
            AsyncHelpers.RunSync(() => IsSystemModuleOrNot(ID));
        }

        async Task IsSystemModuleOrNot(int _id)
        {
            InfosModule ModuleAccess = await new ModulesAccessManager().GetModuleViaIDAsync(_id);

            if (ModuleAccess.ModuleSystem)
            {
                system_module = true;
            }
            else
            {
                system_module = false;
            }
        }

        public async Task<string> GetAddonMainJsViaIDAsync()
        {
            StorageFolder folder_module;

            if (system_module)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"),
                    folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
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

        public async Task<BitmapImage> GetAddonIconViaIDAsync()
        {
            StorageFolder folder_module;

            if (system_module)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"),
                    folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }

            StorageFile file_content = await folder_module.GetFileAsync("icon.png");

            try
            {
                using (var reader = (FileRandomAccessStream)await file_content.OpenAsync(FileAccessMode.Read))
                {
                    var bitmapImage = new BitmapImage();
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
