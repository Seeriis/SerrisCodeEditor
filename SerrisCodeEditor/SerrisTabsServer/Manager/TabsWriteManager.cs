using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace SerrisTabsServer.Manager
{
    public class TabsWriteManager
    {
        StorageFile file = null; StorageFolder folder_tabs = null;

        public TabsWriteManager()
        {
            file = AsyncHelpers.RunSync<StorageFile>(() => ApplicationData.Current.LocalFolder.CreateFileAsync("tabs_list.json", CreationCollisionOption.OpenIfExists).AsTask());
            folder_tabs = AsyncHelpers.RunSync<StorageFolder>(() => ApplicationData.Current.LocalFolder.CreateFolderAsync("tabs", CreationCollisionOption.OpenIfExists).AsTask());
        }

        /// <summary>
        /// Create a new tabs list
        /// </summary>
        /// <param name="new_name">Name of your tabs list</param>
        /// <returns>ID of the new tabs list created</returns>
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

                    list.Add(new TabsList { ID = id, name = new_name, tabs = new List<InfosTab>() });
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewList, ID = new TabID { ID_TabsList = id } });
                        });
                    }

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
                    TabsList list_tabs = list.First(m => m.ID == id);

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

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.ListDeleted, ID = new TabID { ID_TabsList = id } });
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

        /// <summary>
        /// Create a tab in a tabs list who was selected by his ID
        /// </summary>
        /// <param name="tab">Tab you want to create</param>
        /// <param name="id_list">ID of the tabs list</param>
        /// <returns>ID of the new tab</returns>
        public async Task<int> CreateTabAsync(InfosTab tab, int id_list)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    tab.ID = new Random().Next(999999);
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    TabsList list_tabs = list.First(m => m.ID == id_list);

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
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewTab, ID = new TabID { ID_Tab = tab.ID, ID_TabsList = id_list } });
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

        /// <summary>
        /// Delete a tab who was selected by his ID and tabs list ID
        /// </summary>
        /// <param name="ids">ID of the tab and tabs list where is the tab</param>
        /// <returns></returns>
        public async Task<bool> DeleteTabAsync(TabID ids)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    TabsList list_tabs = list.First(m => m.ID == ids.ID_TabsList);
                    InfosTab tab = list_tabs.tabs.First(m => m.ID == ids.ID_Tab);
                    list_tabs.tabs.Remove(tab);
                    var delete_file = await folder_tabs.CreateFileAsync(ids.ID_TabsList + "_" + ids.ID_Tab + ".json", CreationCollisionOption.ReplaceExisting); await delete_file.DeleteAsync();
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabDeleted, ID = new TabID { ID_Tab = ids.ID_Tab, ID_TabsList = ids.ID_TabsList } });
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

        /// <summary>
        /// Push code content in a tab
        /// </summary>
        /// <param name="id">ID of the tab and tabs list where is the tab</param>
        /// <param name="content">Content you want to push in the tab</param>
        /// <returns></returns>
        public async Task<bool> PushTabContentViaIDAsync(TabID id, string content)
        {
            try
            {
                StorageFile file_content = await folder_tabs.GetFileAsync(id.ID_TabsList + "_" + id.ID_Tab + ".json");

                using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
                using (JsonReader JsonReader = new JsonTextReader(reader))
                {
                    try
                    {
                        ContentTab _content = new JsonSerializer().Deserialize<ContentTab>(JsonReader);

                        if (content != null)
                        {
                            _content.Content = content;
                            await FileIO.WriteTextAsync(file_content, JsonConvert.SerializeObject(_content, Formatting.Indented));
                            return true;
                        }
                    }
                    catch
                    {
                        return false;
                    }
                }

                return false;
            }
            catch
            { return false; }
        }

        /// <summary>
        /// Update tab informations (please use PushTabContentViaIDAsync() for set tab content)
        /// </summary>
        /// <param name="tab">Tab infos you want to update</param>
        /// <param name="id_list">ID of the tabs list where is the tab</param>
        /// <returns></returns>
        public async Task<bool> PushUpdateTabAsync(InfosTab tab, int id_list)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<TabsList> list = new JsonSerializer().Deserialize<List<TabsList>>(JsonReader);
                    TabsList list_tabs = list.First(m => m.ID == id_list);
                    InfosTab _tab = list_tabs.tabs.First(m => m.ID == tab.ID);
                    int index_list = list.IndexOf(list_tabs), index_tab = list_tabs.tabs.IndexOf(_tab);

                    list[index_list].tabs[index_tab] = tab;

                    var data_tab = await folder_tabs.CreateFileAsync(id_list + "_" + tab.ID + ".json", CreationCollisionOption.OpenIfExists);

                    if(tab.TabContentTemporary != null)
                        await FileIO.WriteTextAsync(data_tab, JsonConvert.SerializeObject(new ContentTab { ID = tab.ID, Content = tab.TabContentTemporary }, Formatting.Indented));

                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach(CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabUpdated, ID = new TabID { ID_Tab = tab.ID, ID_TabsList = id_list } });
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

    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            synch.Post(async _ =>
            {
                try
                {
                    await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();

            SynchronizationContext.SetSynchronizationContext(oldContext);
        }

        /// <summary>
        /// Execute's an async Task<T> method which has a T return type synchronously
        /// </summary>
        /// <typeparam name="T">Return Type</typeparam>
        /// <param name="task">Task<T> method to execute</param>
        /// <returns></returns>
        public static T RunSync<T>(Func<Task<T>> task)
        {
            var oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            T ret = default(T);
            synch.Post(async _ =>
            {
                try
                {
                    ret = await task();
                }
                catch (Exception e)
                {
                    synch.InnerException = e;
                    throw;
                }
                finally
                {
                    synch.EndMessageLoop();
                }
            }, null);
            synch.BeginMessageLoop();
            SynchronizationContext.SetSynchronizationContext(oldContext);
            return ret;
        }

        private class ExclusiveSynchronizationContext : SynchronizationContext
        {
            private bool done;
            public Exception InnerException { get; set; }
            readonly AutoResetEvent workItemsWaiting = new AutoResetEvent(false);
            readonly Queue<Tuple<SendOrPostCallback, object>> items =
                new Queue<Tuple<SendOrPostCallback, object>>();

            public override void Send(SendOrPostCallback d, object state)
            {
                throw new NotSupportedException("We cannot send to our same thread");
            }

            public override void Post(SendOrPostCallback d, object state)
            {
                lock (items)
                {
                    items.Enqueue(Tuple.Create(d, state));
                }
                workItemsWaiting.Set();
            }

            public void EndMessageLoop()
            {
                Post(_ => done = true, null);
            }

            public void BeginMessageLoop()
            {
                while (!done)
                {
                    Tuple<SendOrPostCallback, object> task = null;
                    lock (items)
                    {
                        if (items.Count > 0)
                        {
                            task = items.Dequeue();
                        }
                    }
                    if (task != null)
                    {
                        task.Item1(task.Item2);
                        if (InnerException != null) // the method threw an exeption
                        {
                            throw new AggregateException("AsyncHelpers.Run method threw an exception.", InnerException);
                        }
                    }
                    else
                    {
                        workItemsWaiting.WaitOne();
                    }
                }
            }

            public override SynchronizationContext CreateCopy()
            {
                return this;
            }
        }
    }

}
