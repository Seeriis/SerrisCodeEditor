using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.SystemModules;
using SerrisModulesServer.Type.Theme;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisModulesServer.Manager
{
    public static class ModulesAccessManager
    {

        public static List<InfosModule> GetModules(bool GetSystemModules)
        {
            ModulesDataCache.LoadModulesData();

            try
            {
                var ModulesList = new List<InfosModule>();
                if (ModulesDataCache.ModulesListDeserialized != null)
                {
                    foreach (InfosModule module in ModulesDataCache.ModulesListDeserialized.Modules)
                    {
                        ModulesList.Add(module);
                    }

                    if (GetSystemModules)
                    {
                        foreach (InfosModule module in SystemModulesList.Modules)
                        { ModulesList.Add(module); }
                    }

                    return ModulesList;
                }
                else
                {
                    if (GetSystemModules)
                    {
                        foreach (InfosModule module in SystemModulesList.Modules)
                        {
                            ModulesList.Add(module);
                        }
                    }

                    return ModulesList;
                }
            }
            catch
            {
                return null;
            }


        }

        public static int GetCurrentThemeID()
        {
            ModulesDataCache.LoadModulesData();

            try
            {
                if (ModulesDataCache.ModulesListDeserialized != null)
                {
                    return ModulesDataCache.ModulesListDeserialized.CurrentThemeID;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static int GetCurrentThemeMonacoID()
        {
            ModulesDataCache.LoadModulesData();

            try
            {
                if (ModulesDataCache.ModulesListDeserialized != null)
                {
                    return ModulesDataCache.ModulesListDeserialized.CurrentThemeMonacoID;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        public static async Task<ThemeModule> GetCurrentThemeTempContent()
        {
            StorageFile file_content = await ApplicationData.Current.LocalFolder.CreateFileAsync("theme_temp.json", CreationCollisionOption.OpenIfExists);

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

        public static async Task<string> GetCurrentThemeAceEditorTempContent()
        {
            StorageFile file_content = await ApplicationData.Current.LocalFolder.CreateFileAsync("themeace_temp.js", CreationCollisionOption.OpenIfExists);

            try
            {
                using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch { return null; }

        }

        public static InfosModule GetModuleViaID(int id)
        {
            ModulesDataCache.LoadModulesData();

            try
            {
                InfosModule module = GetModules(true).Where(m => m.ID == id).FirstOrDefault();

                if (module != null)
                {
                    return module;
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static async Task<BitmapImage> GetModuleDefaultLogoViaIDAsync(int id, bool IsSystemModule)
        {
            StorageFolder folder_module;

            if (IsSystemModule)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"), folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(GetCurrentThemeID() + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(GetCurrentThemeID() + "", CreationCollisionOption.OpenIfExists);
            }

            StorageFile file_content = await folder_module.GetFileAsync("logo.png");

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
