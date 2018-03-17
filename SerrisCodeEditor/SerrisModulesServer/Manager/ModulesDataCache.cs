using Newtonsoft.Json;
using SerrisModulesServer.Items;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace SerrisModulesServer.Manager
{
    public static class ModulesDataCache
    {
        public static StorageFile ModulesListFile;
        public static StorageFolder ModulesListFolder;
        public static ModulesList ModulesListDeserialized;

        private static void SetModulesListJsonReader()
        {
            using (StreamReader Reader = new StreamReader(Task.Run(async () => { return await ModulesListFile.OpenStreamForReadAsync(); }).Result))
            using (JsonReader JsonReader = new JsonTextReader(Reader))
            {
                ModulesListDeserialized = new ModulesList();
                ModulesListDeserialized = new JsonSerializer().Deserialize<ModulesList>(JsonReader);
            }
        }

        private static Dispatch.SerialQueue WriterQueue = new Dispatch.SerialQueue();
        public static void WriteModulesListContentFile()
        => WriterQueue.DispatchSync(() => { Task.Run(async () => { await FileIO.WriteTextAsync(ModulesListFile, JsonConvert.SerializeObject(ModulesListDeserialized, Formatting.Indented)); }); });

        public static void LoadModulesData()
        {
            ModulesListFile = ModulesListFile ?? Task.Run(async () => { return await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists); }).Result;
            ModulesListFolder = ModulesListFolder ?? Task.Run(async () => { return await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists); }).Result;

            if (ModulesListDeserialized == null)
                SetModulesListJsonReader();
        }
    }
}
