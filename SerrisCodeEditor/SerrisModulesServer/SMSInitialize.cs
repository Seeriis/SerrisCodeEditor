using Newtonsoft.Json;
using SerrisModulesServer.Items;
using System;
using System.Collections.Generic;
using System.IO;
using Windows.Storage;

namespace SerrisModulesServer
{
    public class SMSInitialize
    {
        public async void InitializeSMSJson()
        {
            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists);

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
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

        async void WriteNewSMSConfiguration()
        {
            var new_list = new ModulesList
            {
                CurrentThemeMonacoID = 0,
                CurrentThemeID = 0,
                Modules = new List<InfosModule>()
            };

            StorageFile file = await ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists);
            await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(new_list, Formatting.Indented));

        }
    }
}
