﻿using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.Type;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;

namespace SerrisModulesServer.Manager
{
    public class ModulesWriteManager
    {
        StorageFile file = null; StorageFolder folder_modules = null; ModulesAccessManager AccessManager;

        public ModulesWriteManager()
        {
            file = AsyncHelpers.RunSync<StorageFile>(() => ApplicationData.Current.LocalFolder.CreateFileAsync("modules_list.json", CreationCollisionOption.OpenIfExists).AsTask());
            folder_modules = AsyncHelpers.RunSync<StorageFolder>(() => ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists).AsTask());
            AccessManager = new ModulesAccessManager();
        }

        public async Task<bool> AddModuleAsync(StorageFile module_zip)
        {
            int id = new Random().Next(999999);
            StorageFolder folder_addon = await folder_modules.CreateFolderAsync(id + "", CreationCollisionOption.OpenIfExists);

            ZipFile.ExtractToDirectory(module_zip.Path, folder_addon.Path);

            StorageFile file_infos = await folder_addon.CreateFileAsync("infos.json", CreationCollisionOption.OpenIfExists);
            using (var reader = new StreamReader(await file_infos.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    InfosModule content = new JsonSerializer().Deserialize<InfosModule>(JsonReader);

                    if (content != null)
                    {
                        content.ID = id; content.ModuleSystem = false; content.IsEnabled = true;

                        if (await folder_addon.TryGetItemAsync("theme_ace.js") != null)
                            content.ContainMonacoTheme = true;
                        else
                            content.ContainMonacoTheme = false;

                        switch(content.ModuleType)
                        {
                            case ModuleTypesList.Addon:
                                content.IsPinnedToToolBar = true;
                                break;

                            case ModuleTypesList.Theme:
                                content.IsPinnedToToolBar = false;
                                break;

                            case ModuleTypesList.Language:
                                content.IsPinnedToToolBar = false;
                                break;
                        }

                        using (var reader_b = new StreamReader(await file.OpenStreamForReadAsync()))
                        using (JsonReader JsonReader_b = new JsonTextReader(reader))
                        {
                            try
                            {
                                ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);

                                if (list == null)
                                {
                                    list = new ModulesList();
                                    list.Modules = new List<InfosModule>();
                                }

                                list.Modules.Add(content);
                                await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                                foreach (CoreApplicationView view in CoreApplication.Views)
                                {
                                    await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                                    {
                                        Messenger.Default.Send(new SMSNotification { Type = TypeUpdateModule.NewModule, ID = id });
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
                catch
                {
                    return false;
                }
            }

            return true;

        }

        public async Task<bool> DeleteModuleViaIDAsync(int id)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);

                    var folder_module = await folder_modules.GetFolderAsync(id + ""); await folder_module.DeleteAsync();
                    list.Modules.Remove(list.Modules.First(m => m.ID == id));
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new SMSNotification { Type = TypeUpdateModule.ModuleDeleted, ID = id });
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

        public async Task<bool> PushUpdateModuleAsync(InfosModule module)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);
                    InfosModule _module = list.Modules.First(m => m.ID == module.ID);
                    int index_module = list.Modules.IndexOf(_module);

                    list.Modules[index_module] = module;

                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new SMSNotification { Type = TypeUpdateModule.UpdateModule, ID = module.ID });
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

        public async Task<bool> SetCurrentThemeIDAsync(int id)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);

                    list.CurrentThemeID = id;
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new SMSNotification { Type = TypeUpdateModule.CurrentThemeUpdated, ID = id });
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

        public async Task<bool> SetCurrentAceEditoThemeIDAsync(int id)
        {
            using (var reader = new StreamReader(await file.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    ModulesList list = new JsonSerializer().Deserialize<ModulesList>(JsonReader);

                    list.CurrentThemeAceID = id;
                    await FileIO.WriteTextAsync(file, JsonConvert.SerializeObject(list, Formatting.Indented));

                    foreach (CoreApplicationView view in CoreApplication.Views)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new SMSNotification { Type = TypeUpdateModule.CurrentThemeUpdated, ID = id });
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

        public async Task<bool> SetCurrentThemeAceEditorTempContentAsync()
        {
            try
            {
                var module = await AccessManager.GetModuleViaIDAsync(await AccessManager.GetCurrentThemeAceEditorID());
                
                StorageFile file_content = await ApplicationData.Current.LocalFolder.CreateFileAsync("themeace_temp.js", CreationCollisionOption.OpenIfExists);
                await FileIO.WriteTextAsync(file_content, await AccessManager.GetCurrentThemeAceEditorContent(module.ModuleSystem));

                return true;
            }
            catch { return false; }
        }

        public async Task<bool> SetCurrentThemeTempContentAsync()
        {
            StorageFolder folder_module = await folder_modules.CreateFolderAsync(await AccessManager.GetCurrentThemeAceEditorID() + "", CreationCollisionOption.OpenIfExists);
            StorageFile file_content_temp = await ApplicationData.Current.LocalFolder.CreateFileAsync("theme_temp.json", CreationCollisionOption.OpenIfExists), file_content = await folder_module.GetFileAsync("theme_ace.js");

            try
            {
                using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
                {
                    await FileIO.WriteTextAsync(file_content_temp, await reader.ReadToEndAsync());
                    return true;
                }
            }
            catch { return false; }

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