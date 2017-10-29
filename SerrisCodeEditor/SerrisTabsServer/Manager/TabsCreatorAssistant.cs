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
    public class TabsCreatorAssistant
    {
        TabsWriteManager WriteManager = new TabsWriteManager();
        TabsAccessManager AccessManager = new TabsAccessManager();
        FileTypesManager FileTypes = new FileTypesManager();

        public async Task<int> CreateNewTab(int IDList, string FileName, Encoding encoding, StorageListTypes type, string content)
        {
            string extension = "";
            foreach (string type_file in FileTypes.List_Type_extensions)
            {
                if (FileName.Contains(type_file))
                {
                    extension = FileTypes.GetExtensionType(Path.GetExtension(FileName));
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
            int id_tab = await WriteManager.CreateTabAsync(newtab, IDList);
            await WriteManager.PushTabContentViaIDAsync(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }, content, false);
            return id_tab;
        }

        public async Task<bool> CreateNewFileViaTab(TabID ids)
        {
            try
            {
                await new StorageRouter(await AccessManager.GetTabViaIDAsync(ids), ids.ID_TabsList).CreateFile().ContinueWith(async (e) =>
                {
                    await new StorageRouter(await AccessManager.GetTabViaIDAsync(ids), ids.ID_TabsList).WriteFile();
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<int>> OpenFilesAndCreateNewTabsFiles(int IDList, StorageListTypes type)
        {
            var list_ids = new List<int>();

            var opener = new FileOpenPicker();
            var FileTypes = new FileTypesManager();
            opener.ViewMode = PickerViewMode.Thumbnail;
            opener.SuggestedStartLocation = PickerLocationId.ComputerFolder;
            opener.FileTypeFilter.Add("*");

            foreach (string ext in FileTypes.List_Type_extensions)
            {
                opener.FileTypeFilter.Add(ext);
            }

            IReadOnlyList<StorageFile> files = await opener.PickMultipleFilesAsync();
            foreach (StorageFile file in files)
            {
                await Task.Run(async () => 
                {
                    if (file != null)
                    {
                        StorageApplicationPermissions.FutureAccessList.Add(file);
                        var tab = new InfosTab { TabName = file.Name, TabStorageMode = type, TabContentType = ContentType.File, CanBeDeleted = true, CanBeModified = true, PathContent = file.Path, TabInvisibleByDefault = false };

                        foreach (string _type in FileTypes.List_Type_extensions)
                        {
                            if (tab.TabName.Contains(_type))
                            {
                                tab.TabType = FileTypes.GetExtensionType(file.FileType);
                                break;
                            }
                            else
                            {
                                continue;
                            }
                        }

                        int id_tab = 0;
                        await Task.Run(async () => 
                        {
                            id_tab = await WriteManager.CreateTabAsync(tab, IDList);
                            new StorageRouter(await AccessManager.GetTabViaIDAsync(new TabID { ID_Tab = id_tab, ID_TabsList = IDList }), IDList).ReadFile(true);
                        });
                        list_ids.Add(id_tab);
                    }

                });
            }

            return list_ids;
        }
    }
}
