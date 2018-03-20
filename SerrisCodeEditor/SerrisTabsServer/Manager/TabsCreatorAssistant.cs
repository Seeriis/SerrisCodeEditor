using GalaSoft.MvvmLight.Messaging;
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
            string extension = "";
            foreach (string type_file in FileTypesManager.List_Type_extensions)
            {
                if (FileName.Contains(type_file))
                {
                    extension = FileTypesManager.GetExtensionType(Path.GetExtension(FileName));
                    break;
                }
                else
                {
                    continue;
                }
            }
            var newtab = new InfosTab
            {
                TabName = FileName,
                TabStorageMode = type,
                TabEncoding = encoding.CodePage,
                TabContentType = ContentType.File,
                CanBeDeleted = true,
                CanBeModified = true,
                TabType = extension,
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

        public static async Task<List<int>> OpenFilesAndCreateNewTabsFiles(int IDList, StorageListTypes type)
        {
            var list_ids = new List<int>();

            var opener = new FileOpenPicker();
            opener.ViewMode = PickerViewMode.Thumbnail;
            opener.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            opener.FileTypeFilter.Add("*");

            foreach (string ext in FileTypesManager.List_Type_extensions)
            {
                opener.FileTypeFilter.Add(ext);
            }

            IReadOnlyList<StorageFile> files = await opener.PickMultipleFilesAsync();
            foreach (StorageFile file in files)
            {
                await Task.Run(() =>
                {
                    if (file != null)
                    {
                        StorageApplicationPermissions.FutureAccessList.Add(file);
                        var tab = new InfosTab { TabName = file.Name, TabStorageMode = type, TabContentType = ContentType.File, CanBeDeleted = true, CanBeModified = true, PathContent = file.Path, TabInvisibleByDefault = false };

                        foreach (string _type in FileTypesManager.List_Type_extensions)
                        {
                            if (tab.TabName.Contains(_type))
                            {
                                tab.TabType = FileTypesManager.GetExtensionType(file.FileType);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

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
    }
}
