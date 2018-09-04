using Newtonsoft.Json;
using SerrisModulesServer.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;

namespace SerrisModulesServer
{
    public static class SMSInitialize
    {
        public static void InitializeSMSJson()
        {
            StorageFile file = Task.Run(async () => { return await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists); }).Result;

            using (var reader = new StreamReader(Task.Run(async () => { return await file.OpenStreamForReadAsync(); }).Result))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);

                    if (list == null)
                    {
                        WriteNewSMSConfiguration();
                    }
                }
                catch
                {
                    WriteNewSMSConfiguration();
                }
            }
        }

        private static void WriteNewSMSConfiguration()
        {
            var new_list = new ModulesList
            {
                CurrentThemeMonacoID = SMSInfos.DefaultMonacoThemeID,
                CurrentThemeID = SMSInfos.DefaultThemeID,
                Modules = new List<InfosModule>()
            };

            StorageFile file = Task.Run(async () => { return await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists); }).Result;
            Task.Run(async () => { await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(new_list, Formatting.Indented)); }).Wait();

        }
    }
}
