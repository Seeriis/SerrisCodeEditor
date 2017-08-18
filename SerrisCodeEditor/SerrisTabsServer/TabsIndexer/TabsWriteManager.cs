using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace SerrisTabsServer.TabsIndexer
{
    public class TabsWriteManager
    {
        StorageFile file; StorageFolder folder_tabs;

        public TabsWriteManager()
        {
            SetFileVar();
        }

        async void SetFileVar()
        {
            file = await ApplicationData.Current.LocalFolder.CreateFileAsync("tabs_list.json", CreationCollisionOption.OpenIfExists);
            folder_tabs = await ApplicationData.Current.LocalFolder.CreateFolderAsync("tabs", CreationCollisionOption.OpenIfExists);
        }

        public async Task<int> CreateTabsListAsync(string new_name)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    int id = new Random().Next(999999);
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);

                    if (list == null)
                        list = new List<TabsList>();

                    list.Add(new TabsList { ID = id, name = new_name });
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    return id;
                }
                catch
                {
                    return 0;
                }
            }
            
        }

        /// <summary>
        /// Delete tabs list and the tabs content of the list
        /// </summary>
        /// <param name="id">ID of the tabs list</param>
        /// <returns></returns>
        public async Task<bool> DeleteTabsListAsync(int id)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    TabsList list_tabs = list.Where(m => m.ID == id).FirstOrDefault();

                    foreach(InfosTab tab in list_tabs.tabs)
                    {
                        try
                        {
                            await folder_tabs.GetFileAsync(id + "_" + tab.ID + ".json").GetResults().DeleteAsync();
                        }
                        catch { }
                    }

                    list.Remove(list_tabs);
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public async Task<int> CreateTabAsync(InfosTab tab, int id_list)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    tab.ID = new Random().Next(999999);
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    TabsList list_tabs = list.Where(m => m.ID == id_list).FirstOrDefault();

                    if (list_tabs.tabs == null)
                        list_tabs.tabs = new List<InfosTab>();

                    list_tabs.tabs.Add(tab);
                    var data_tab = await folder_tabs.CreateFileAsync(id_list + "_" + tab.ID + ".json", CreationCollisionOption.ReplaceExisting);
                    await FileIO.WriteTextAsync(data_tab, JsonConvert.SerializeObject(new ContentTab { ID = tab.ID, Content = "" }, Formatting.Indented));
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdate.NewTab, ID = new TabID { ID_Tab = tab.ID, ID_TabsList = id_list } });
                        });
                    }
                    return tab.ID;
                }
                catch
                {
                    return 0;
                }
            }

        }

        public async Task<bool> DeleteTabAsync(TabID ids)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    TabsList list_tabs = list.Where(m => m.ID == ids.ID_TabsList).FirstOrDefault();
                    InfosTab tab = list_tabs.tabs.Where(m => m.ID == ids.ID_Tab).FirstOrDefault();
                    list_tabs.tabs.Remove(tab);
                    var delete_file = await folder_tabs.CreateFileAsync(ids.ID_TabsList + "_" + ids.ID_Tab + ".json", CreationCollisionOption.ReplaceExisting); await delete_file.DeleteAsync();
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdate.TabDeleted, ID = new TabID { ID_Tab = ids.ID_Tab, ID_TabsList = ids.ID_TabsList } });
                        });
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }

        public async Task<bool> PushUpdateTabAsync(InfosTab tab, int id_list)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    TabsList list_tabs = list.Where(m => m.ID == id_list).FirstOrDefault();
                    InfosTab _tab = list_tabs.tabs.Where(m => m.ID == tab.ID).FirstOrDefault();
                    _tab = tab;

                    var data_tab = await folder_tabs.CreateFileAsync(id_list + "_" + tab.ID + ".json", CreationCollisionOption.OpenIfExists);

                    if(tab.TabContentTemporary != null)
                        await FileIO.WriteTextAsync(data_tab, JsonConvert.SerializeObject(new ContentTab { ID = tab.ID, Content = tab.TabContentTemporary }, Formatting.Indented));

                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach(CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdate.TabUpdated, ID = new TabID { ID_Tab = tab.ID, ID_TabsList = id_list } });
                        });
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
            }

        }
    }
}
