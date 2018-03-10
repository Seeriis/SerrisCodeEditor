using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace SerrisTabsServer.Manager
{
    public static class TabsAccessManager
    {

        public static List<InfosTab> GetTabs(int id)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                if (TabsDataCache.TabsListDeserialized != null)
                {
                    return TabsDataCache.TabsListDeserialized.Where(m => m.ID == id).FirstOrDefault().tabs.Where(n => n.TabInvisibleByDefault == false).ToList();
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

        public static List<int> GetTabsListID()
        {
            TabsDataCache.LoadTabsData();

            try
            {
                var list_ids = new List<int>();

                if (TabsDataCache.TabsListDeserialized != null)
                {
                    foreach (TabsList list_tabs in TabsDataCache.TabsListDeserialized)
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

        public static List<int> GetTabsID(int id_list)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                var list_ids = new List<int>();

                if (TabsDataCache.TabsListDeserialized != null)
                {
                    if (TabsDataCache.TabsListDeserialized.Where(m => m.ID == id_list).FirstOrDefault().tabs != null)
                    {
                        foreach (InfosTab tab in TabsDataCache.TabsListDeserialized.Where(m => m.ID == id_list).FirstOrDefault().tabs)
                        {
                            if (!tab.TabInvisibleByDefault)
                            {
                                list_ids.Add(tab.ID);
                            }
                        }
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

        public static InfosTab GetTabViaID(TabID id)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                if (TabsDataCache.TabsListDeserialized != null)
                {
                    List<InfosTab> InfosTabList = TabsDataCache.TabsListDeserialized.Where(m => m.ID == id.ID_TabsList).FirstOrDefault().tabs;

                    if (InfosTabList != null)
                    {
                        return InfosTabList.Where(m => m.ID == id.ID_Tab).FirstOrDefault();
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static TabsList GetTabsListViaID(int id)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                if (TabsDataCache.TabsListDeserialized != null)
                {
                    TabsList List = TabsDataCache.TabsListDeserialized.Where(m => m.ID == id).FirstOrDefault();

                    if (List.tabs != null)
                    {
                        return List;
                    }
                }
            }
            catch
            {
                return null;
            }

            return null;
        }

        public static async Task<string> GetTabContentViaIDAsync(TabID id)
        {
            try
            {
                StorageFile file_content = await TabsDataCache.TabsListFolder.GetFileAsync(id.ID_TabsList + "_" + id.ID_Tab + ".json");

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
            {
                return null;
            }
        }

        public static void UpdateTabRef(ref InfosTab tab, int id_list)
        {
            TabsDataCache.LoadTabsData();

            using (var reader = new StreamReader(TabsDataCache.TabsListFile.OpenStreamForReadAsync().Result))
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
                catch { }
            }

        }

    }
}
