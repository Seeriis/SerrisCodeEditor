using Microsoft.Graph;
using Microsoft.OneDrive.Sdk;
using Newtonsoft.Json;
using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;

namespace SerrisTabsServer.Manager
{
    public static class TabsDataCache
    {
        //ONEDRIVE VARIABLES
        public static IOneDriveClient OneDriveClient { get; set; }
        public static IAuthenticationProvider AuthProvider { get; set; }

        public static StorageFile TabsListFile;
        public static StorageFolder TabsListFolder;
        public static List<TabsList> TabsListDeserialized;

        private static void SetTabsListJsonReader()
        {
            using (StreamReader Reader = new StreamReader(Task.Run(async () => { return await TabsListFile.OpenStreamForReadAsync(); }).Result))
            using (JsonReader JsonReader = new JsonTextReader(Reader))
            {
                TabsListDeserialized = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                TabsListDeserialized = TabsListDeserialized ?? new List<TabsList>();
            }
        }

        private static Dispatch.SerialQueue WriterQueue = new Dispatch.SerialQueue();
        public static void WriteTabsListContentFile()
        => WriterQueue.DispatchSync(() => { Task.Run(async () => { await FileIO.WriteTextAsync(TabsListFile, JsonConvert.SerializeObject(TabsListDeserialized, Formatting.Indented)); }); });

        public static void LoadTabsData()
        {
            TabsListFile = TabsListFile ?? Task.Run(async () => { return await ApplicationData.Current.LocalFolder.CreateFileAsync("tabs_list.json", CreationCollisionOption.OpenIfExists); }).Result;
            TabsListFolder = TabsListFolder ?? Task.Run(async () => { return await ApplicationData.Current.LocalFolder.CreateFolderAsync("tabs", CreationCollisionOption.OpenIfExists); }).Result;

            if (TabsListDeserialized == null)
                SetTabsListJsonReader();
        }
    }
}
