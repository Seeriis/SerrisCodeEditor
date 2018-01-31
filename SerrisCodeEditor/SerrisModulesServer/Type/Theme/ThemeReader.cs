using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;

namespace SerrisModulesServer.Type.Theme
{
    public class ThemeReader
    {
        int id_module; bool system_module;

        public ThemeReader(int ID)
        {
            id_module = ID;
            InfosModule ModuleAccess = ModulesAccessManager.GetModuleViaID(ID);
            system_module = ModuleAccess.ModuleSystem;
        }

        /*async Task IsSystemModuleOrNot(int _id)
        {
        }*/

        /// <summary>
        /// Get the JavaScript content of the monaco theme
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetThemeJSContentAsync()
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

            StorageFile file_content = await folder_module.GetFileAsync("theme_ace.js");

            try
            {
                using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
                {
                    return await reader.ReadToEndAsync();
                }

            }
            catch
            {
                return null;
            }

        }

        /// <summary>
        /// Get all RGBA parameters and images path of the theme
        /// </summary>
        /// <returns></returns>
        public async Task<ThemeModule> GetThemeContentAsync()
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

            StorageFile file_content = await folder_module.GetFileAsync("theme.json");

            using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ThemeModule content = new JsonSerializer().Deserialize<ThemeModule>(JsonReader);

                    if (content != null)
                    {
                        return content;
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;

        }

        /// <summary>
        /// Get all SolidColorBrush (and BitmapImage) of the theme
        /// </summary>
        /// <returns></returns>
        public async Task<ThemeModuleBrush> GetThemeBrushesContent()
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

            StorageFile file_content = await folder_module.GetFileAsync("theme.json");

            using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ThemeModule content = new JsonSerializer().Deserialize<ThemeModule>(JsonReader);

                    if (content != null)
                    {
                        var content_brushs = new ThemeModuleBrush();
                        System.Diagnostics.Debug.WriteLine(Path.Combine(folder_module.Path, content.BackgroundImagePath));

                        content_brushs.SetBrushsAndImageViaThemeModule(content, folder_module.Path);

                        /*if (system_module)
                        {
                            content_brushs.SetBrushsAndImageViaThemeModule(content, "ms-appx://SerrisModulesServer/SystemModules/" + id_module + "/");
                        }
                        else
                        {
                            content_brushs.SetBrushsAndImageViaThemeModule(content, folder_module.Path);
                        }*/

                        return content_brushs;
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;

        }

    }
}
