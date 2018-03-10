using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace SerrisTabsServer.Manager
{
    public static class TabsWriteManager
    {

        /// <summary>
        /// Create a new tabs list
        /// </summary>
        /// <param name="new_name">Name of your tabs list</param>
        /// <returns>ID of the new tabs list created</returns>
        public static async Task<int> CreateTabsListAsync(string new_name)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                int id = new Random().Next(999999);

                TabsDataCache.TabsListDeserialized.Add(new TabsList { ID = id, name = new_name, tabs = new List<InfosTab>() });
                await FileIO.WriteTextAsync(TabsDataCache.TabsListFile, JsonConvert.SerializeObject(TabsDataCache.TabsListDeserialized, Formatting.Indented));

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

        /// <summary>
        /// Delete tabs list and the tabs content of the list
        /// </summary>
        /// <param name="id">ID of the tabs list</param>
        /// <returns></returns>
        public static async Task<bool> DeleteTabsListAsync(int id)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                TabsList list_tabs = TabsDataCache.TabsListDeserialized.First(m => m.ID == id);

                foreach (InfosTab tab in list_tabs.tabs)
                {
                    try
                    {
                        await TabsDataCache.TabsListFolder.GetFileAsync(id + "_" + tab.ID + ".json").GetResults().DeleteAsync();
                    }
                    catch { }
                }

                TabsDataCache.TabsListDeserialized.Remove(list_tabs);
                await FileIO.WriteTextAsync(TabsDataCache.TabsListFile, JsonConvert.SerializeObject(TabsDataCache.TabsListDeserialized, Formatting.Indented));

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

        /// <summary>
        /// Create a tab in a tabs list who was selected by his ID
        /// </summary>
        /// <param name="tab">Tab you want to create</param>
        /// <param name="id_list">ID of the tabs list</param>
        /// <returns>ID of the new tab</returns>
        public static async Task<int> CreateTabAsync(InfosTab tab, int id_list, bool SendNotification)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                tab.ID = new Random().Next(999999);
                TabsList list_tabs = TabsDataCache.TabsListDeserialized.First(m => m.ID == id_list);

                if (list_tabs.tabs == null)
                {
                    list_tabs.tabs = new List<InfosTab>();
                }

                list_tabs.tabs.Add(tab);
                StorageFile data_tab = await TabsDataCache.TabsListFolder.CreateFileAsync(id_list + "_" + tab.ID + ".json", CreationCollisionOption.ReplaceExisting);
                await FileIO.WriteTextAsync(data_tab, JsonConvert.SerializeObject(new ContentTab { ID = tab.ID, Content = "" }, Formatting.Indented));
                await FileIO.WriteTextAsync(TabsDataCache.TabsListFile, JsonConvert.SerializeObject(TabsDataCache.TabsListDeserialized, Formatting.Indented));

                foreach (CoreApplicationView view in CoreApplication.Views)
                {
                    await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        if (SendNotification)
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

        /// <summary>
        /// Delete a tab who was selected by his ID and tabs list ID
        /// </summary>
        /// <param name="ids">ID of the tab and tabs list where is the tab</param>
        /// <returns></returns>
        public static async Task<bool> DeleteTabAsync(TabID ids)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                TabsList list_tabs = TabsDataCache.TabsListDeserialized.First(m => m.ID == ids.ID_TabsList);
                InfosTab tab = list_tabs.tabs.First(m => m.ID == ids.ID_Tab);
                list_tabs.tabs.Remove(tab);
                StorageFile delete_file = await TabsDataCache.TabsListFolder.CreateFileAsync(ids.ID_TabsList + "_" + ids.ID_Tab + ".json", CreationCollisionOption.ReplaceExisting); await delete_file.DeleteAsync();
                await FileIO.WriteTextAsync(TabsDataCache.TabsListFile, JsonConvert.SerializeObject(TabsDataCache.TabsListFile, Formatting.Indented));

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

        /// <summary>
        /// Push code content in a tab
        /// </summary>
        /// <param name="id">ID of the tab and tabs list where is the tab</param>
        /// <param name="content">Content you want to push in the tab</param>
        /// <param name="sendnotification">Send (or not) a notification about the updated content with MVVMLight</param>
        /// <returns></returns>
        public static async Task<bool> PushTabContentViaIDAsync(TabID id, string content, bool sendnotification)
        {
            try
            {
                TabsDataCache.LoadTabsData();
                StorageFile file_content = await TabsDataCache.TabsListFolder.CreateFileAsync(id.ID_TabsList + "_" + id.ID_Tab + ".json", CreationCollisionOption.OpenIfExists);

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

                            if (sendnotification)
                            {
                                foreach (CoreApplicationView view in CoreApplication.Views)
                                {
                                    await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                    {
                                        Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabUpdated, ID = id });
                                    });
                                }
                            }

                            return true;
                        }
                    }
                    catch { }
                }
            }
            catch { }
            return false;
        }

        /// <summary>
        /// Update tab informations (please use PushTabContentViaIDAsync() for set tab content)
        /// </summary>
        /// <param name="tab">Tab infos you want to update</param>
        /// <param name="id_list">ID of the tabs list where is the tab</param>
        /// <returns></returns>
        public static async Task<bool> PushUpdateTabAsync(InfosTab tab, int id_list)
        {
            TabsDataCache.LoadTabsData();

            try
            {
                TabsList list_tabs = TabsDataCache.TabsListDeserialized.First(m => m.ID == id_list);
                InfosTab _tab = list_tabs.tabs.First(m => m.ID == tab.ID);
                int index_list = TabsDataCache.TabsListDeserialized.IndexOf(list_tabs), index_tab = list_tabs.tabs.IndexOf(_tab);

                TabsDataCache.TabsListDeserialized[index_list].tabs[index_tab] = tab;

                StorageFile data_tab = await TabsDataCache.TabsListFolder.CreateFileAsync(id_list + "_" + tab.ID + ".json", CreationCollisionOption.OpenIfExists);

                if (tab.TabContentTemporary != null)
                {
                    await FileIO.WriteTextAsync(data_tab, JsonConvert.SerializeObject(new ContentTab { ID = tab.ID, Content = tab.TabContentTemporary }, Formatting.Indented));
                }

                await FileIO.WriteTextAsync(TabsDataCache.TabsListFile, JsonConvert.SerializeObject(TabsDataCache.TabsListDeserialized, Formatting.Indented));

                foreach (CoreApplicationView view in CoreApplication.Views)
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

    public static class AsyncHelpers
    {
        /// <summary>
        /// Execute's an async Task<T> method which has a void return value synchronously
        /// </summary>
        /// <param name="task">Task<T> method to execute</param>
        public static void RunSync(Func<Task> task)
        {
            SynchronizationContext oldContext = SynchronizationContext.Current;
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
            SynchronizationContext oldContext = SynchronizationContext.Current;
            var synch = new ExclusiveSynchronizationContext();
            SynchronizationContext.SetSynchronizationContext(synch);
            var ret = default(T);
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
            => this;
        }
    }

}
