using Microsoft.OneDrive.Sdk;
using Microsoft.Toolkit.Uwp.Helpers;
using SerrisModulesServer.Type.ProgrammingLanguage;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using SerrisTabsServer.Xaml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SerrisTabsServer.Storage.StorageTypes
{
    public class OneDrive : StorageType
    {

        public OneDrive(InfosTab tab, int _ListTabsID) : base(tab, _ListTabsID)
        {
            Tab = tab; ListTabsID = _ListTabsID;
        }

        public async Task CreateFile()
        {
            try
            {
                var currentAV = ApplicationView.GetForCurrentView(); var newAV = CoreApplication.CreateNewView();

                await newAV.Dispatcher.RunAsync(
                                CoreDispatcherPriority.Normal,
                                async () =>
                                {
                                    var newWindow = Window.Current;
                                    var newAppView = ApplicationView.GetForCurrentView();
                                    newAppView.Title = "OneDrive explorer";

                                    var frame = new Frame();
                                    frame.Navigate(typeof(OnedriveExplorer), new Tuple<OnedriveExplorerMode, TabID>(OnedriveExplorerMode.CreateFile, new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID }));
                                    newWindow.Content = frame;
                                    newWindow.Activate();

                                    await ApplicationViewSwitcher.TryShowAsStandaloneAsync(
                                        newAppView.Id,
                                        ViewSizePreference.UseHalf,
                                        currentAV.Id,
                                        ViewSizePreference.UseHalf);
                                });
                newAV.HostedViewClosing += (a, b) => { };
            }
            catch { }

        }

        public async void DeleteFile()
        {
            try
            {
                await TabsDataCache.OneDriveClient.Drive.Items[Tab.TabOriginalPathContent].Request().DeleteAsync();
                Tab.TabStorageMode = StorageListTypes.Nothing;
                Tab.TabOriginalPathContent = "";
                await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID);
            }
            catch { }
        }

        public async Task<bool> ReadFile(bool ReplaceEncoding)
        {
            var Item = await TabsDataCache.OneDriveClient.Drive.Items[Tab.TabOriginalPathContent].Content.Request().GetAsync();

            using (StreamReader st = new StreamReader(Item))
            {
                await TabsWriteManager.PushTabContentViaIDAsync(new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID }, st.ReadToEnd(), true);

                if (ReplaceEncoding)
                {
                    Tab.TabEncoding = st.CurrentEncoding.CodePage;
                    await TabsWriteManager.PushUpdateTabAsync(Tab, ListTabsID);
                }

                st.Dispose();
            }

            return true;
        }

        public async Task<string> ReadFileAndGetContent()
        {
            var Item = await TabsDataCache.OneDriveClient.Drive.Items[Tab.TabOriginalPathContent].Content.Request().GetAsync();
            string Code = "";
            using (StreamReader st = new StreamReader(Item))
            {
                await TabsWriteManager.PushTabContentViaIDAsync(new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID }, st.ReadToEnd(), true);
                Code = st.ReadToEnd();
                st.Dispose();
            }

            return Code;
        }

        public async Task WriteFile()
        {

            await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
            {
                if(string.IsNullOrWhiteSpace(Tab.TabOriginalPathContent))
                {
                   await CreateFile().ContinueWith(async (e) => 
                   {
                       //await WriteFile();
                   });
                }
                else
                {
                    try
                    {
                        if(await OneDriveAuthHelper.OneDriveAuthentification())
                        {
                            using (var stream = new MemoryStream(Encoding.GetEncoding(Tab.TabEncoding).GetBytes(await TabsAccessManager.GetTabContentViaIDAsync(new TabID { ID_Tab = Tab.ID, ID_TabsList = ListTabsID }))))
                            {
                                await TabsDataCache.OneDriveClient.Drive.Items[Tab.TabOriginalPathContent].Content.Request().PutAsync<Item>(stream);

                            }
                        }

                    }
                    catch
                    {
                        await CreateFile().ContinueWith(async (e) =>
                        {
                            //await WriteFile();
                        });
                    }
                }
                

            });

        }

    }
}
