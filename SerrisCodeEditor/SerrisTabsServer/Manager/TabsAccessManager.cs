using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SerrisTabsServer.Manager
{
    public class TabsAccessManager
    {
        StorageFile file;

        public TabsAccessManager()
        {
            file = AsyncHelpers.RunSync<StorageFile>(() => ApplicationData.Current.LocalFolder.CreateFileAsync("tabs_list.json", CreationCollisionOption.OpenIfExists).AsTask());
        }

        public async Task<List<InfosTab>> GetTabsAsync(int id)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    if (list != null)
                    {
                        return list.Where(m => m.ID == id).FirstOrDefault().tabs;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch
                {
                    return null;
                }
            }

        }

        public async Task<List<int>> GetTabsListIDAsync()
        {

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    List<int> list_ids = new List<int>();

                    if (list != null)
                    {
                        foreach (TabsList list_tabs in list)
                        {
                            list_ids.Add(list_tabs.ID);
                        }
                        return list_ids;
                    }
                    else
                    {
                        return list_ids;
                    }
                }
                catch
                {
                    return null;
                }
            }

        }

        public async Task<List<int>> GetTabsIDAsync(int id_list)
        {

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    List<int> list_ids = new List<int>();

                    if (list != null)
                    {
                        if(list.Where(m => m.ID == id_list).FirstOrDefault().tabs != null)
                            foreach (InfosTab tab in list.Where(m => m.ID == id_list).FirstOrDefault().tabs)
                            {
                                list_ids.Add(tab.ID);
                            }

                        return list_ids;
                    }
                    else
                    {
                        return list_ids;
                    }
                }
                catch
                {
                    return null;
                }
            }

        }

        public async Task<InfosTab> GetTabViaIDAsync(TabID id)
        {

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);

                    if (list != null)
                    {
                        if (list.Where(m => m.ID == id.ID_TabsList).FirstOrDefault().tabs != null)
                            return list.Where(m => m.ID == id.ID_TabsList).FirstOrDefault().tabs.Where(m => m.ID == id.ID_Tab).FirstOrDefault();
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<TabsList> GetTabsListViaIDAsync(int id)
        {

            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);

                    if (list != null)
                    {
                        if (list.Where(m => m.ID == id).FirstOrDefault().tabs != null)
                            return list.Where(m => m.ID == id).FirstOrDefault();
                    }
                }
                catch
                {
                    return null;
                }
            }

            return null;
        }

        public async Task<string> GetTabContentViaIDAsync(TabID id)
        {
            try
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("tabs", CreationCollisionOption.OpenIfExists);
                StorageFile file_content = await folder_content.GetFileAsync(id.ID_TabsList + "_" + id.ID_Tab + ".json");

                using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
                using (JsonReader JsonReader = new JsonTextReader(reader))
                {
                    try
                    {
                        ContentTab content = new JsonSerializer().Deserialize<ContentTab>(JsonReader);

                        if (content != null)
                        {
                            return content.Content;
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

                return null;
            }
            catch
            { return null; }
        }

        public void UpdateTab(ref InfosTab tab, int id_list)
        {

            using (var reader = new StreamReader(file.OpenStreamForReadAsync().Result))
            {
                try
                {
                    int id = tab.ID;
                    JObject tabs = JObject.Parse(reader.ReadToEnd()).Values<JObject>().Where(m => m["ID"].Value<int>() == id_list).FirstOrDefault(), 
                        tab_search = tabs.Values<JObject>().Where(m => m["ID"].Value<int>() == id).FirstOrDefault();

                    if (tab != null)
                    {
                        tab = tab_search.Value<InfosTab>();
                    }
                }
                catch
                {
                    
                }
            }

        }

    }
}
