using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.SystemModules;
using SerrisModulesServer.Type;
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
                var ModulesList = new List<InfosModule>(ModulesDataCache.ModulesListDeserialized.Modules);

                if (GetSystemModules)
                {
                    ModulesList.AddRange(SystemModulesList.Modules);
                }

                return ModulesList;
            }
            catch
            {
                return null;
            }

        }

        public static List<InfosModule> GetSpecificModules(bool GetSystemModules, ModuleTypesList ModuleType)
        {
            ModulesDataCache.LoadModulesData();

            try
            {
                var ModulesList = new List<InfosModule>();

                foreach(InfosModule Module in ModulesDataCache.ModulesListDeserialized.Modules)
                {
                    if (Module.ModuleType == ModuleType)
                        ModulesList.Add(Module);
                }

                if (GetSystemModules)
                {
                    foreach (InfosModule Module in SystemModulesList.Modules)
                    {
                        if (Module.ModuleType == ModuleType)
                            ModulesList.Add(Module);
                    }
                }

                return ModulesList;
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

            try
            {
                StorageFile LogoFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(GetModuleFolderPath(id, IsSystemModule) + "logo.png"));

                using (var reader = (FileRandomAccessStream)await LogoFile.OpenAsync(FileAccessMode.Read))
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

        public static async Task<BitmapImage> GetModuleIconViaIDAsync(int id, bool IsSystemModule)
        {

            try
            {
                StorageFile IconFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(GetModuleFolderPath(id, IsSystemModule) + "icon.png"));

                if(IconFile.Path == null)
                {
                    //Default tab language module
                    IconFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(GetModuleFolderPath(47, true) + "icon.png"));
                }

                using (var reader = (FileRandomAccessStream)await IconFile.OpenAsync(FileAccessMode.Read))
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

        public static string GetModuleFolderPath(int ModuleID, bool IsSystemModule)
        {
            string ModulePath = "";

            if(IsSystemModule)
            {
                ModulePath = "ms-appx:///SerrisModulesServer/SystemModules/{0}/" + ModuleID + "/";
                switch (GetModuleViaID(ModuleID).ModuleType)
                {
                    default:
                    case ModuleTypesList.Addon:
                        return string.Format(ModulePath, "Addons");

                    case ModuleTypesList.ProgrammingLanguage:
                        return string.Format(ModulePath, "ProgrammingLanguages");

                    case ModuleTypesList.Theme:
                        return string.Format(ModulePath, "Themes");
                }
            }
            else
            {
                return "ms-appdata:///local/modules/" + ModuleID + "/";
            }

        }


    }
}
