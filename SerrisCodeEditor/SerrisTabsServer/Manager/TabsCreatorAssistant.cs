using GalaSoft.MvvmLight.Messaging;
using SerrisModulesServer.Type.ProgrammingLanguage;
using SerrisTabsServer.Items;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.AccessCache;
using Windows.Storage.Pickers;

namespace SerrisTabsServer.Manager
{
    public static class TabsCreatorAssistant
    {

        public static void CreateNewTab(int IDList, string FileName, Encoding encoding, StorageListTypes type, string content)
        {
            var newtab = new InfosTab
            {
                TabName = FileName,
                TabStorageMode = type,
                TabEncoding = encoding.CodePage,
                TabContentType = ContentType.File,
                CanBeDeleted = true,
                CanBeModified = true,
                TabType = LanguagesHelper.GetLanguageType(FileName),
                TabInvisibleByDefault = false
            };

            Task.Run(() => 
            {
                int id_tab = Task.Run(async () => { return await TabsWriteManager.CreateTabAsync(newtab, IDList, false); }).Result;
                if (Task.Run(async () => { return await TabsWriteManager.PushTabContentViaIDAsync(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }, content, false); }).Result)
                {
                    Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewTab, ID = new TabID { ID_Tab = id_tab, ID_TabsList = IDList } });
                }
            });

        }

        public static void CreateNewTabInFolder(int IDList, TabID FolderIDs, string FileName, Encoding encoding, StorageListTypes type, string content)
        {
            var newtab = new InfosTab
            {
                TabName = FileName,
                TabStorageMode = type,
                TabEncoding = encoding.CodePage,
                TabContentType = ContentType.File,
                CanBeDeleted = true,
                CanBeModified = true,
                TabType = LanguagesHelper.GetLanguageType(FileName),
                TabInvisibleByDefault = true
            };

            Task.Run(() =>
            {
                int id_tab = Task.Run(async () => { return await TabsWriteManager.CreateTabAsync(newtab, IDList, false); }).Result;
                if (Task.Run(async () => { return await TabsWriteManager.PushTabContentViaIDAsync(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }, content, false); }).Result)
                {
                    Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewTab, ID = new TabID { ID_Tab = id_tab, ID_TabsList = IDList } });

                    InfosTab Folder = TabsAccessManager.GetTabViaID(FolderIDs);
                    Folder.FolderContent.Add(id_tab);
                    Task.Run(async () => { await TabsWriteManager.PushUpdateTabAsync(Folder, FolderIDs.ID_TabsList); });

                }
            });

        }

        public static async Task<bool> CreateNewFileViaTab(TabID ids)
        {
            try
            {
                await new StorageRouter(TabsAccessManager.GetTabViaID(ids), ids.ID_TabsList).CreateFile().ContinueWith(async (e) =>
                {
                    await new StorageRouter(TabsAccessManager.GetTabViaID(ids), ids.ID_TabsList).WriteFile();
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static async Task<List<int>> OpenFolder(int IDList, StorageListTypes type)
        {
            var list_ids = new List<int>();

            var opener = new FolderPicker();
            opener.ViewMode = PickerViewMode.Thumbnail;
            opener.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            opener.FileTypeFilter.Add("*");

            var Folder = await opener.PickSingleFolderAsync();

            if(Folder != null)
            {
                StorageApplicationPermissions.FutureAccessList.Add(Folder);

                foreach (IStorageItem Item in await Folder.GetItemsAsync())
                {
                    await Task.Run(async () =>
                    {
                        if (Item.IsOfType(StorageItemTypes.File))
                        {
                            StorageFile file = (StorageFile)Item;

                            if (file != null)
                            {
                                StorageApplicationPermissions.FutureAccessList.Add(file);

                                if (file.ContentType.Contains("text") || LanguagesHelper.IsFileLanguageIsCompatible(file.Name))
                                {
                                    var tab = new InfosTab { TabName = file.Name, TabStorageMode = type, TabContentType = ContentType.File, CanBeDeleted = true, CanBeModified = true, PathContent = file.Path, TabInvisibleByDefault = true, TabType = LanguagesHelper.GetLanguageType(file.Name) };

                                    int id_tab = Task.Run(async () => { return await TabsWriteManager.CreateTabAsync(tab, IDList, false); }).Result;


                                    if (Task.Run(async () => { return await new StorageRouter(TabsAccessManager.GetTabViaID(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }), IDList).ReadFile(true); }).Result)
                                    {
                                        Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewTab, ID = new TabID { ID_Tab = id_tab, ID_TabsList = IDList } });
                                    }

                                    list_ids.Add(id_tab);
                                }

                            }
                        }
                        else
                        {
                            StorageFolder FolderItem = (StorageFolder)Item;
                            list_ids.Add(await OpenSubFolder(FolderItem, IDList, type));
                        }


                    });
                }
                await TabsWriteManager.CreateTabAsync(new InfosTab { TabName = Folder.Name, PathContent = Folder.Path, TabContentType = ContentType.Folder, FolderContent = list_ids, FolderOpened = true }, IDList, true);

            }

            return list_ids;
        }

        private async static Task<int> OpenSubFolder(StorageFolder Folder, int IDList, StorageListTypes type)
        {
            return await Task.Run(async () => 
            {
                var FolderItemIDs = new List<int>();

                foreach (IStorageItem Item in await Folder.GetItemsAsync())
                {
                    StorageApplicationPermissions.FutureAccessList.Add(Item);

                    if (Item.IsOfType(StorageItemTypes.File))
                    {
                        StorageFile file = (StorageFile)Item;

                        if(file.ContentType.Contains("text") || LanguagesHelper.IsFileLanguageIsCompatible(file.Name))
                        {
                            var tab = new InfosTab { TabName = file.Name, TabStorageMode = type, TabContentType = ContentType.File, CanBeDeleted = true, CanBeModified = true, PathContent = file.Path, TabInvisibleByDefault = true, TabType = LanguagesHelper.GetLanguageType(file.Name) };

                            int id_tab = await TabsWriteManager.CreateTabAsync(tab, IDList, false);

                            await new StorageRouter(TabsAccessManager.GetTabViaID(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }), IDList).ReadFile(true);

                            FolderItemIDs.Add(id_tab);
                        }
                    }
                    else
                    {
                        FolderItemIDs.Add(await OpenSubFolder((StorageFolder)Item, IDList, type));
                    }
                }

                return await TabsWriteManager.CreateTabAsync(new InfosTab { TabName = Folder.Name, PathContent = Folder.Path, TabContentType = ContentType.Folder, FolderContent = FolderItemIDs, FolderOpened = true, TabInvisibleByDefault = true }, IDList, false);
            });
        }

        public static async Task<List<int>> OpenFilesAndCreateNewTabsFiles(int IDList, StorageListTypes type)
        {
            var list_ids = new List<int>();

            var opener = new FileOpenPicker();
            opener.ViewMode = PickerViewMode.Thumbnail;
            opener.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            opener.FileTypeFilter.Add("*");

            IReadOnlyList<StorageFile> files = await opener.PickMultipleFilesAsync();
            foreach (StorageFile file in files)
            {
                await Task.Run(() =>
                {
                    if (file != null)
                    {
                        StorageApplicationPermissions.FutureAccessList.Add(file);
                        var tab = new InfosTab { TabName = file.Name, TabStorageMode = type, TabContentType = ContentType.File, CanBeDeleted = true, CanBeModified = true, PathContent = file.Path, TabInvisibleByDefault = false, TabType = LanguagesHelper.GetLanguageType(file.Name) };

                        int id_tab = Task.Run(async () => { return await TabsWriteManager.CreateTabAsync(tab, IDList, false); }).Result;
                        if (Task.Run(async () => { return await new StorageRouter(TabsAccessManager.GetTabViaID(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }), IDList).ReadFile(true); }).Result)
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewTab, ID = new TabID { ID_Tab = id_tab, ID_TabsList = IDList } });
                        }

                        list_ids.Add(id_tab);
                    }

                });
            }

            return list_ids;
        }

        public static async Task<List<int>> OpenFilesAlreadyOpenedAndCreateNewTabsFiles(int IDList, IReadOnlyList<IStorageItem> files)
        {
            var list_ids = new List<int>();

            foreach (StorageFile file in files)
            {
                await Task.Run(() =>
                {
                    bool FileAlreadyAvailable = false; int IDSelectedFile = 0;

                    if (file != null)
                    {
                        foreach (InfosTab Tab in TabsAccessManager.GetTabsListViaID(IDList).tabs)
                        {
                            if (Tab.PathContent == file.Path)
                            {
                                IDSelectedFile = Tab.ID;
                                FileAlreadyAvailable = true;
                                break;
                            }
                        }

                        if(!FileAlreadyAvailable)
                        {
                            StorageApplicationPermissions.FutureAccessList.Add(file);

                            var tab = new InfosTab { TabName = file.Name, TabStorageMode = StorageListTypes.LocalStorage, TabContentType = ContentType.File, CanBeDeleted = true, CanBeModified = true, PathContent = file.Path, TabInvisibleByDefault = false, TabType = LanguagesHelper.GetLanguageType(file.Name) };

                            int id_tab = Task.Run(async () => { return await TabsWriteManager.CreateTabAsync(tab, IDList, false); }).Result;
                            if (Task.Run(async () => { return await new StorageRouter(TabsAccessManager.GetTabViaID(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }), IDList).ReadFile(true); }).Result)
                            {
                                Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.NewTab, ID = new TabID { ID_Tab = id_tab, ID_TabsList = IDList } });
                            }

                            list_ids.Add(id_tab);
                        }
                        else
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.SelectTab, ID = new TabID { ID_Tab = IDSelectedFile, ID_TabsList = IDList } });
                        }
                    }

                });
            }

            return list_ids;
        }
    }
}
