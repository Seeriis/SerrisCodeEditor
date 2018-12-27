using Microsoft.Toolkit.Uwp.Helpers;
using SerrisModulesServer.Type.ProgrammingLanguage;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;

namespace SerrisTabsServer.Storage.StorageTypes
{
    public class LocalStorage : StorageType
    {
        public LocalStorage(InfosTab tab, int _ListTabsID) : base(tab, _ListTabsID)
        {
            Tab = tab; ListTabsID = _ListTabsID;
        }

        public async Task<bool> CreateFile()
        {
            bool result = false;

            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                FileSavePicker filePicker = new FileSavePicker();
                filePicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
                filePicker.SuggestedFileName = Path.GetFileNameWithoutExtension(Tab.TabName);

                string Extension = Path.GetExtension(Tab.TabName);
                if(Extension != "")
                {
                    filePicker.FileTypeChoices.Add("File", new List<string> { Extension });
                }

                foreach (string name in LanguagesHelper.GetLanguagesNames())
                {
                    List<string> Types = LanguagesHelper.GetLanguageExtensions(LanguagesHelper.GetLanguageTypeViaName(name));

                    if (Types.Count == 0)
                    {
                        Types.Add(".txt");
                    }

                    filePicker.FileTypeChoices.Add(name, Types);
                }

                StorageFile file = await filePicker.PickSaveFileAsync();
                if (file != null)
                {
                    Windows.Storage.AccessCache.StorageApplicationPermissions.FutureAccessList.Add(file);
                    Windows.Storage.FileProperties.BasicProperties date = await file.GetBasicPropertiesAsync();

                    Tab.TabDateModified = date.DateModified.ToString();
                    Tab.TabType = LanguagesHelper.GetLanguageType(file.FileType);
                    Tab.TabOriginalPathContent = file.Path;
                    Tab.TabName = file.Name;

                    await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID, true);

                    result = true;
                }

            });

            return result;
        }

        public async void DeleteFile()
        {
            StorageFile file = AsyncHelpers.RunSync(() => StorageFile.GetFileFromPathAsync(Tab.TabOriginalPathContent).AsTask());
            await file.DeleteAsync();
            Tab.TabStorageMode = StorageListTypes.Nothing;
            Tab.TabOriginalPathContent = "";
            await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID, false);
        }

        public async Task<bool> ReadFile(bool ReplaceEncoding)
        {
            try
            {
                StorageFile file = AsyncHelpers.RunSync(() => StorageFile.GetFileFromPathAsync(Tab.TabOriginalPathContent).AsTask());
                string encode_type = ""; bool encode_bom = true;

                await Task.Run(() =>
                {
                    using (FileStream fs = File.OpenRead(Tab.TabOriginalPathContent))
                    {
                        var cdet = new Ude.CharsetDetector();
                        cdet.Feed(fs);
                        cdet.DataEnd();
                        if (cdet.Charset != null)
                        {
                            encode_type = cdet.Charset;
                        }
                    }
                });

                if (Encoding.UTF8.CodePage == Encoding.GetEncoding(encode_type).CodePage)
                    encode_bom = false;

                if (encode_type == "")
                    encode_type = "utf-8";

                using (var st = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding(encode_type)))
                {
                    await TabsWriteManager.PushTabContentViaIDAsync(new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID }, st.ReadToEnd(), true);

                    if (ReplaceEncoding)
                    {
                        Tab.TabEncoding = Encoding.GetEncoding(encode_type).CodePage;
                        Tab.TabEncodingWithBOM = encode_bom;
                        await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID, true);
                    }

                    st.Dispose();
                }

                return true;
            }
            catch (Exception e)
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    await new MessageDialog(e.Message, new ResourceLoader().GetString("popup-errorreadingfile")).ShowAsync();
                });
                return false;
            }
            
        }

        public async Task<string> ReadFileAndGetContent()
        {
            StorageFile file = AsyncHelpers.RunSync(() => StorageFile.GetFileFromPathAsync(Tab.TabOriginalPathContent).AsTask());
            string encode_type = "", content = "";

            await Task.Run(() =>
            {
                using (FileStream fs = File.OpenRead(Tab.TabOriginalPathContent))
                {
                    var cdet = new Ude.CharsetDetector();
                    cdet.Feed(fs);
                    cdet.DataEnd();
                    if (cdet.Charset != null)
                    {
                        encode_type = cdet.Charset;
                    }
                }
            });

            using (var st = new StreamReader(await file.OpenStreamForReadAsync(), Encoding.GetEncoding(encode_type)))
            {
                content = st.ReadToEnd();
                st.Dispose();
                return content;
            }
        }

        public async Task WriteFile()
        {

            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                try
                {
                    StorageFile file = await StorageFile.GetFileFromPathAsync(Tab.TabOriginalPathContent);

                    if (file != null)
                    {
                        await FileIO.WriteTextAsync(file, string.Empty);

                        Encoding TempEncoding = Encoding.GetEncoding(Tab.TabEncoding);

                        if(TempEncoding == Encoding.UTF8 && !Tab.TabEncodingWithBOM)
                        {
                            TempEncoding = new UTF8Encoding(false);
                        }

                        string Content = await TabsAccessManager.GetTabContentViaIDAsync(new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID });

                        using (var rd = new StreamWriter(await file.OpenStreamForWriteAsync(), TempEncoding))
                        {
                            rd.Write(Content);
                            rd.Flush(); rd.Dispose();
                        }


                        if(TempEncoding.CodePage == Encoding.ASCII.CodePage && Tab.TabEncodingReplacingRequest != EncodingReplacingRequest.Never)
                        {
                            var stream = new MemoryStream();
                            var writer = new StreamWriter(stream);
                            writer.Write(Content);
                            writer.Flush();
                            stream.Position = 0;

                            using (MemoryStream str = stream)
                            {
                                var cdet = new Ude.CharsetDetector();
                                cdet.Reset();
                                cdet.Feed(str);
                                cdet.DataEnd();
                                if (cdet.Charset != null)
                                {
                                    if (Encoding.GetEncoding(cdet.Charset).CodePage != TempEncoding.CodePage && Encoding.GetEncoding(cdet.Charset).CodePage == Encoding.UTF8.CodePage)
                                    {
                                        await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                                        {
                                            MessageDialog Dialog = new MessageDialog(new ResourceLoader().GetString("popup-changeencodingcontent"), String.Format(new ResourceLoader().GetString("popup-changeencodingtitle"), TempEncoding.EncodingName, cdet.Charset));
                                            Dialog.Commands.Add(new UICommand { Label = new ResourceLoader().GetString("popup-changeencodingaccept"), Invoked = async (e) => { Tab.TabEncoding = Encoding.GetEncoding(cdet.Charset).CodePage; Tab.TabEncodingWithBOM = false; Tab.TabEncodingReplacingRequest = EncodingReplacingRequest.NotRequested; await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID, true); } });
                                            Dialog.Commands.Add(new UICommand { Label = new ResourceLoader().GetString("popup-changeencodinglater"), Invoked = async (e) => { Tab.TabEncodingReplacingRequest = EncodingReplacingRequest.MaybeLater; await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID, true); } });
                                            Dialog.Commands.Add(new UICommand { Label = new ResourceLoader().GetString("popup-changeencodingno"), Invoked = async (e) => { Tab.TabEncodingReplacingRequest = EncodingReplacingRequest.Never; await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID, true); } });
                                            await Dialog.ShowAsync();
                                        });
                                    }
                                }
                            }
                        }

                    }

                }
                catch
                {
                    await CreateFile().ContinueWith(async (e) => 
                    {
                        if(e.Result)
                        {
                            await WriteFile();
                        }
                    });
                }

            });

        }

    }
}
