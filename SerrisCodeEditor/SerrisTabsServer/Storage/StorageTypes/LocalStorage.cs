using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SerrisTabsServer.Items;
using Windows.Storage;
using System.IO;
using Windows.System.Profile;
using Windows.Storage.Pickers;
using System.Diagnostics;
using SerrisTabsServer.Manager;
using Windows.UI.Core;

namespace SerrisTabsServer.Storage.StorageTypes
{
    public class LocalStorage : StorageType
    {
        public LocalStorage(InfosTab tab, int _ListTabsID) : base(tab, _ListTabsID)
        {
            Tab = tab; ListTabsID = _ListTabsID;
        }

        public async Task CreateFile()
        {
            FolderPicker folderPicker = new FolderPicker(); StorageFolder folder;
            folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;

            foreach (string ext in FileTypes.List_Type_extensions)
            { folderPicker.FileTypeFilter.Add(ext); }

            folder = await folderPicker.PickSingleFolderAsync();
            if (folder != null)
            {
                var file = await folder.CreateFileAsync(Tab.TabName, CreationCollisionOption.OpenIfExists);
                Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
                var date = await file.GetBasicPropertiesAsync(); Tab.TabDateModified = date.DateModified.ToString();

                foreach (string type in FileTypes.List_Type_extensions)
                {
                    if (Tab.TabName.Contains(type))
                    {
                        Tab.TabType = FileTypes.GetExtensionType(file.FileType);
                        break;
                    }
                    else continue;
                }
                Tab.PathContent = file.Path;
                await TabsWriter.PushUpdateTabAsync(Tab, ListTabsID);
            }

        }

        public async void DeleteFile()
        {
            StorageFile file = AsyncHelpers.RunSync<StorageFile>(() => StorageFile.GetFileFromPathAsync(Tab.PathContent).AsTask());
            await file.DeleteAsync();
            Tab.TabStorageMode = StorageListTypes.Nothing;
            Tab.PathContent = "";
            await TabsWriter.PushUpdateTabAsync(Tab, ListTabsID);
        }

        public async void ReadFile(bool ReplaceEncoding)
        {
            StorageFile file = AsyncHelpers.RunSync<StorageFile>(() => StorageFile.GetFileFromPathAsync(Tab.PathContent).AsTask());
            string encode_type = "";

            await Task.Run(() =>
            {
                using (FileStream fs = File.OpenRead(Tab.PathContent))
                {
                    Ude.CharsetDetector cdet = new Ude.CharsetDetector();
                    cdet.Feed(fs);
                    cdet.DataEnd();
                    if (cdet.Charset != null)
                        encode_type = cdet.Charset;
                }
            });

            using (StreamReader st = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding(encode_type)))
            {
                await TabsWriter.PushTabContentViaIDAsync(new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID }, st.ReadToEnd());

                if(ReplaceEncoding)
                {
                    Tab.TabEncoding = Encoding.GetEncoding(encode_type).CodePage;
                    await TabsWriter.PushUpdateTabAsync(Tab, ListTabsID);
                }

                st.Dispose();
            }
        }

        public async Task<string> ReadFileAndGetContent()
        {
            StorageFile file = AsyncHelpers.RunSync<StorageFile>(() => StorageFile.GetFileFromPathAsync(Tab.PathContent).AsTask());
            string encode_type = "", content = "";

            await Task.Run(() =>
            {
                using (FileStream fs = File.OpenRead(Tab.PathContent))
                {
                    Ude.CharsetDetector cdet = new Ude.CharsetDetector();
                    cdet.Feed(fs);
                    cdet.DataEnd();
                    if (cdet.Charset != null)
                        encode_type = cdet.Charset;
                }
            });

            using (StreamReader st = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding(encode_type)))
            {
                content = st.ReadToEnd();
                st.Dispose();
                return content;
            }
        }

        public async Task WriteFile()
        {
            StorageFile file = AsyncHelpers.RunSync<StorageFile>(() => StorageFile.GetFileFromPathAsync(Tab.PathContent).AsTask());

            if (file != null)
            {
                await FileIO.WriteTextAsync(file, String.Empty);
                using (StreamWriter rd = new StreamWriter(await file.OpenStreamForWriteAsync(), Encoding.GetEncoding(Tab.TabEncoding)))
                {
                    rd.Write(await TabsReader.GetTabContentViaIDAsync(new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID }));
                    rd.Flush(); rd.Dispose();
                }
            }
        }

    }
}
