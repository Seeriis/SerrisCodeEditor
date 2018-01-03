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
        public int ID { get; set; }
        public ModulesPinedModification Modification { get; set; }
    }

    public class ModulesPinned
    {
        int[] DefaultModulesPinned = { 7, 4, 3 };
        StorageFile file;

        public ModulesPinned()
        {
            file = AsyncHelpers.RunSync(() => ApplicationData.Current.LocalFolder.CreateFileAsync("modules_pinned.json", CreationCollisionOption.OpenIfExists).AsTask());
        }

        public async Task<List<int>> GetModulesPinned()
        {

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<int> list = new JsonSerializer().Deserialize<List<int>>(JsonReader);
                    if (list != null)
                    {
                        return list;
                    }
                    else
                    {
                        list = new List<int>();
                        foreach (int id in DefaultModulesPinned)
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

        public async void AddNewModule(int id)
        {
            List<int> List = await GetModulesPinned();
            List.Add(id);

            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(List, Formatting.Indented));
            Messenger.Default.Send<ModulesPinnedNotification>(new ModulesPinnedNotification { ID = id, Modification = ModulesPinedModification.Added });
        }

        public async void RemoveModule(int id)
        {
            List<int> List = await GetModulesPinned();
            List.Remove(id);

            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(List, Formatting.Indented));
            Messenger.Default.Send<ModulesPinnedNotification>(new ModulesPinnedNotification { ID = id, Modification = ModulesPinedModification.Removed });
        }

    }
}
