using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.UI.Xaml.Media;

namespace SerrisModulesServer.Type.Theme
{
    public class ThemeReader
    {
        int id_module; bool system_module;

        public ThemeReader(int ID)
        {
            id_module = ID;
            AsyncHelpers.RunSync(() => IsSystemModuleOrNot(ID));
        }

        async Task IsSystemModuleOrNot(int _id)
        {
            InfosModule ModuleAccess = await new ModulesAccessManager().GetModuleViaIDAsync(_id);

            if (ModuleAccess.ModuleSystem)
                system_module = true;
            else
                system_module = false;

        }

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
        /// Get all RGBA parameters of the theme
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
        public async Task<ThemeModuleBrush> GetThemeBrushsContent()
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
                        ThemeModuleBrush content_brushs = new ThemeModuleBrush();
                        System.Diagnostics.Debug.WriteLine(Path.Combine(folder_module.Path, content.BackgroundImagePath));

                        content_brushs.SetBrushsAndImageViaThemeModule(content, folder_module.Path);

                        /*if (system_module)
                            content_brushs.SetBrushsAndImageViaThemeModule(content, "ms-appx://SerrisModulesServer/SystemModules/" + id_module + "/");
                        else
                            content_brushs.SetBrushsAndImageViaThemeModule(content, folder_module.Path);*/

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
