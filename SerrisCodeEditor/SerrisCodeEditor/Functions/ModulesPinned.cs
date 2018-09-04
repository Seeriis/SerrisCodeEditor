using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using SerrisCodeEditorEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SerrisCodeEditor.Functions
{
    public enum ModulesPinedModification
    {
        Removed,
        Added
    }

    public class ModulesPinnedNotification
    {
        public string ID { get; set; }
        public ModulesPinedModification Modification { get; set; }
    }

    public static class ModulesPinned
    {
        static string[] DefaultModulesPinned = { "7", "4", "3", "5" };
        static StorageFile file = AsyncHelpers.RunSync(() => ApplicationData.Current.LocalFolder.CreateFileAsync("modules_pinned.json", CreationCollisionOption.OpenIfExists).AsTask());

        public static async Task<List<string>> GetModulesPinned()
        {

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<string> list = new JsonSerializer().Deserialize<List<string>>(JsonReader);
                    if (list != null)
                    {
                        return list;
                    }
                    else
                    {
                        list = new List<string>();
                        foreach (string id in DefaultModulesPinned)
                        {
                            list.Add(id);
                        }

                        await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));
                        return list;
                    }
                }
                catch
                {
                    return null;
                }
            }


        }

        public static async void AddNewModule(string id)
        {
            List<string> List = await GetModulesPinned();
            List.Add(id);

            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(List, Formatting.Indented));
            Messenger.Default.Send<ModulesPinnedNotification>(new ModulesPinnedNotification { ID = id, Modification = ModulesPinedModification.Added });
        }

        public static async void RemoveModule(string id)
        {
            List<string> List = await GetModulesPinned();
            List.Remove(id);

            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(List, Formatting.Indented));
            Messenger.Default.Send<ModulesPinnedNotification>(new ModulesPinnedNotification { ID = id, Modification = ModulesPinedModification.Removed });
        }

    }
}
