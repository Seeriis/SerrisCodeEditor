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
    public class ThemeReader : ModuleReader
    {
        public ThemeReader(int ID) : base (ID) { }

        /// <summary>
        /// Get the JavaScript content of the monaco theme
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetThemeJSContentAsync()
        {
            StorageFile ThemeContent = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModuleFolderPath + "theme.js"));

            try
            {
                using (var reader = new StreamReader(await ThemeContent.OpenStreamForReadAsync()))
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
            StorageFile ThemeContent = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModuleFolderPath + "theme.json"));

            using (var reader = new StreamReader(await ThemeContent.OpenStreamForReadAsync()))
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
            try
            {
                ThemeModule Content = await GetThemeContentAsync();

                if (Content != null)
                {
                    ThemeModuleBrush Brushs = new ThemeModuleBrush();
                    Brushs.SetBrushsAndImageViaThemeModule(Content, ModuleFolderPath);

                    return Brushs;
                }
            }
            catch
            {
                return null;
            }

            return null;

        }

    }
}
