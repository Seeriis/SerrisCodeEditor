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
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisModulesServer.Manager
{
    public static class ModulesAccessManager
    {
        //static StorageFile file;

        public static async Task<List<InfosModule>> GetModulesAsync(bool GetSystemModules)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists);

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);
                    var ModulesList = new List<InfosModule>();
                    if (list != null)
                    {
                        foreach (InfosModule module in list.Modules)
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

        }

        public static List<InfosModule> GetModules(bool GetSystemModules)
        {
            StorageFile file = AsyncHelpers.RunSync(() => ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists).AsTask());

            using (var reader = new StreamReader(AsyncHelpers.RunSync(() => file.OpenStreamForReadAsync())))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);
                    var ModulesList = new List<InfosModule>();
                    if (list != null)
                    {
                        foreach (InfosModule module in list.Modules)
                        { ModulesList.Add(module); }

                        if (GetSystemModules)
                        {
                            foreach (InfosModule module in SystemModulesList.Modules)
                            {
                                ModulesList.Add(module);
                            }
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

        }


        public static async Task<int> GetCurrentThemeIDAsync()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists);

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);
                    if (list != null)
                    {
                        return list.CurrentThemeID;
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
        }

        public static int GetCurrentThemeID()
        {
            StorageFile file = AsyncHelpers.RunSync(() => ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists).AsTask());

            using (var reader = new StreamReader(AsyncHelpers.RunSync(() => file.OpenStreamForReadAsync())))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);
                    if (list != null)
                    {
                        return list.CurrentThemeID;
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
        }

        public static async Task<int> GetCurrentThemeAceEditorID()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists);

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);
                    if (list != null)
                    {
                        return list.CurrentThemeAceID;
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

        public static async Task<InfosModule> GetModuleViaIDAsync(int id)
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists);

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);

                    if (list != null)
                    {
                        foreach (InfosModule _module in SystemModulesList.Modules)
                        { list.Modules.Add(_module); }

                        InfosModule module = list.Modules.Where(m => m.ID == id).FirstOrDefault();

                        if (module != null)
                        {
                            return module;
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public static InfosModule GetModuleViaID(int id)
        {
            StorageFile file = AsyncHelpers.RunSync(() => ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists).AsTask());

            using (var reader = new StreamReader(AsyncHelpers.RunSync(() => file.OpenStreamForReadAsync())))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);

                    if (list != null)
                    {
                        foreach (InfosModule _module in SystemModulesList.Modules)
                        { list.Modules.Add(_module); }

                        InfosModule module = list.Modules.Where(m => m.ID == id).FirstOrDefault();

                        if (module != null)
                        {
                            return module;
                        }
                    }
                    else
                    {
                        list = new ModulesList();
                        list.Modules = new List<InfosModule>();

                        foreach (InfosModule _module in SystemModulesList.Modules)
                        { list.Modules.Add(_module); }

                        InfosModule module = list.Modules.Where(m => m.ID == id).FirstOrDefault();

                        if (module != null)
                        {
                            return module;
                        }

                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public static async Task<BitmapImage> GetModuleDefaultLogoViaIDAsync(int id, bool IsSystemModule)
        {
            StorageFolder folder_module;

            if (IsSystemModule)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"), folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(await GetCurrentThemeIDAsync() + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(await GetCurrentThemeIDAsync() + "", CreationCollisionOption.OpenIfExists);
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
