using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Addon;
using SerrisModulesServer.Type.Theme;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace SerrisCodeEditor
{

    public sealed partial class MainPage : Page
    {
        TabsAccessManager Tabs_manager_access = new TabsAccessManager(); TabsWriteManager Tabs_manager_writer = new TabsWriteManager();
        ModulesAccessManager Modules_manager_access = new ModulesAccessManager(); ModulesWriteManager Modules_manager_writer = new ModulesWriteManager();
        TabID IDs = new TabID();

        /*protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(e.Parameter != null)
            {
                Tuple<TabsAccessManager, TabsWriteManager> tuple = e.Parameter as Tuple<TabsAccessManager, TabsWriteManager>;
                if(tuple != null)
                {
                    manager_access = tuple.Item1; manager_writer = tuple.Item2;
                    lel();
                }
                else { lel(); }
            }
            else
            {
                lel();
            }
        }*/

        public MainPage()
        {
            InitializeComponent();
            //ChakraSMS sms = new ChakraSMS(); //Initialize Chakra Engine (important)

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            lel();
        }

        public async void PushCodeViaIDs()
        {
            Messenger.Default.Send(new TabSelectedNotification { tabID = IDs.ID_Tab, tabsListID = IDs.ID_TabsList, code = await ContentViewer.GetCode(), contactType = ContactTypeSCEE.GetCodeForTab });
        }

        List<TabSelectedNotification> Queue_Tabs = new List<TabSelectedNotification>(); bool CanManageQueue = true;
        public async void ManageQueueTabs()
        {
            while (!CanManageQueue)
            {
                await Task.Delay(20);
            }

            if (CanManageQueue)
            {
                CanManageQueue = false;

                try
                {
                    if (IDs.ID_Tab != 0)
                    {
                        string content = await ContentViewer.GetCode();
                        SerrisModulesServer.Manager.AsyncHelpers.RunSync(() => Tabs_manager_writer.PushTabContentViaIDAsync(IDs, content, false));
                    }
                }
                catch { }

                foreach (CoreApplicationView view in CoreApplication.Views)
                {
                    if (Dispatcher != view.Dispatcher)
                    {
                        await view.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            Messenger.Default.Send(new STSNotification { Type = TypeUpdateTab.TabUpdated, ID = IDs });
                        });
                    }
                }

                IDs = new TabID { ID_Tab = Queue_Tabs[0].tabID, ID_TabsList = Queue_Tabs[0].tabsListID };
                ContentViewer.CodeLanguage = Queue_Tabs[0].typeLanguage; ContentViewer.Code = Queue_Tabs[0].code;

                Queue_Tabs.RemoveAt(0);
                CanManageQueue = true;
            }
        }

        public async void lel()
        {
            Messenger.Default.Register<TabSelectedNotification>(this, (notification) =>
            {
                try
                {
                    switch (notification.contactType)
                    {
                        case ContactTypeSCEE.SetCodeForEditor:
                            Queue_Tabs.Add(notification);
                            ManageQueueTabs();
                            break;

                        case ContactTypeSCEE.SetCodeForEditorWithoutUpdate:
                            ContentViewer.CodeLanguage = notification.typeCode; ContentViewer.Code = notification.code;
                            break;
                    }
                }
                catch { }
            });

            List<int> sts_initialize = await Tabs_manager_access.GetTabsListIDAsync();

            /*if (sts_initialize.Count == 0)
            {
                IDs.ID_TabsList = await Tabs_manager_writer.CreateTabsListAsync("Liste des onglets - test");
                List<int> list_ids = await Tabs_manager_access.GetTabsIDAsync(IDs.ID_TabsList);
                AddTabs(list_ids);
            }
            else
            {
                IDs.ID_TabsList = sts_initialize[0];
                List<int> list_ids = await Tabs_manager_access.GetTabsIDAsync(IDs.ID_TabsList);
                AddTabs(list_ids);
            }*/

            list_ids_list.Items.Clear();
            foreach (int id in sts_initialize)
            {
                list_ids_list.Items.Add(id);
            }

            List<InfosModule> sms_initialize = await Modules_manager_access.GetModulesAsync(true);
            foreach (InfosModule module in sms_initialize)
            {
                if (module.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon)
                {
                    var pinned = new PinnedModule { ID = module.ID, ModuleName = module.ModuleName, ModuleType = module.ModuleType };
                    pinned.Image = await new AddonReader(module.ID).GetAddonIconViaIDAsync();

                    ModulesList.Items.Add(pinned);
                }
            }

            //var list_modules = await Modules_manager_access.GetModulesAsync(true);
            //new MessageDialog("Nombre de modules: " + list_modules.Count + ";\n " + list_modules[0].ModuleName + " par " + list_modules[0].ModuleAuthor).ShowAsync();

            /*ThemeModule theme = new ThemeModule
            {
                AddonDefaultColor = new RGBA { A = 1, B = 255, G = 255, R = 255 },
                AddonDefaultFontColor = new RGBA { A = 1, B = 255, G = 255, R = 255 },

                MainColor = new RGBA { A = 1, B = 255, G = 255, R = 255 },
                MainColorFont = new RGBA { A = 1, B = 255, G = 255, R = 255 },

                RoundNotificationColor = new RGBA { A = 1, B = 255, G = 255, R = 255 },

                SecondaryColor = new RGBA { A = 1, B = 255, G = 255, R = 255 },
                SecondaryColorFont = new RGBA { A = 1, B = 255, G = 255, R = 255 },

                ToolbarColor = new RGBA { A = 1, B = 255, G = 255, R = 255 },
                ToolbarColorFont = new RGBA { A = 1, B = 255, G = 255, R = 255 },

                ToolbarRoundButtonColor = new RGBA { A = 1, B = 255, G = 255, R = 255 },
                ToolbarRoundButtonColorFont = new RGBA { A = 1, B = 255, G = 255, R = 255 },

                BackgroundImagePath = "lol.png"
            };

            var dataPackage = new DataPackage();
            //dataPackage.SetText(Package.Current.InstalledLocation.Path);
            dataPackage.SetText(JsonConvert.SerializeObject(theme, Formatting.Indented));
            Clipboard.SetContent(dataPackage);*/

            ThemeModuleBrush brushs_theme = await new ThemeReader(await Modules_manager_access.GetCurrentThemeID()).GetThemeBrushesContent();
            image_bg.ImageSource = brushs_theme.BackgroundImage;
        }

        public void AddTabs(List<int> tabs)
        {
            list_ids.Items.Clear();
            foreach (int nmb in tabs)
            {
                list_ids.Items.Add(nmb);
            }
        }

        private async void NewTab_Click(object sender, RoutedEventArgs e)
        {
            await Tabs_manager_writer.CreateTabAsync(new InfosTab { }, IDs.ID_TabsList);
        }

        private async void DeleteTab_Click(object sender, RoutedEventArgs e)
        {
            await Tabs_manager_writer.DeleteTabAsync(new TabID { ID_Tab = int.Parse(id_box.Text), ID_TabsList = IDs.ID_TabsList });
        }

        private async void NewWindow_Click(object sender, RoutedEventArgs e)
        {
            CoreApplicationView newView = CoreApplication.CreateNewView();
            int newViewId = 0;
            await newView.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                var frame = new Frame();
                frame.Navigate(typeof(MainPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void OpenFiles_Click(object sender, RoutedEventArgs e)
        {
            var creator = new TabsCreatorAssistant();
            await creator.OpenFilesAndCreateNewTabsFiles(IDs.ID_TabsList, StorageListTypes.LocalStorage);
        }

        private async void list_ids_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_ids.SelectedItem != null)
            {
                IDs.ID_Tab = (int)list_ids.SelectedItem;
                InfosTab tab = await Tabs_manager_access.GetTabViaIDAsync(IDs);
                TabName.Text = "Nom: " + tab.TabName;
                TabEncoding.Text = "Encoding: " + Encoding.GetEncoding(tab.TabEncoding).EncodingName;

                ContentViewer.CodeLanguage = tab.TabType.ToUpper();
                ContentViewer.Code = await Tabs_manager_access.GetTabContentViaIDAsync(IDs);

                /*var dataPackage = new DataPackage();
                dataPackage.SetText(Package.Current.InstalledLocation.Path);
                Clipboard.SetContent(dataPackage);*/
            }
        }

        private async void CreateFile_Click(object sender, RoutedEventArgs e)
        {
            var creator = new TabsCreatorAssistant();
            await creator.CreateNewTab(IDs.ID_TabsList, "test.json", Encoding.ASCII, StorageListTypes.LocalStorage, "Je suis une patate !");
        }

        private async void DeleteList_Click(object sender, RoutedEventArgs e)
        {
            await Tabs_manager_writer.DeleteTabsListAsync(IDs.ID_TabsList);
        }

        private async void NewList_Click(object sender, RoutedEventArgs e)
        {
            await Tabs_manager_writer.CreateTabsListAsync(name_box.Text);
        }

        private async void list_ids_list_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (list_ids_list.SelectedItem != null)
            {
                IDs.ID_TabsList = (int)list_ids_list.SelectedItem; TabsList tabslist = await Tabs_manager_access.GetTabsListViaIDAsync(IDs.ID_TabsList);

                if (tabslist != null)
                {
                    name_box.Text = tabslist.name;
                }

                List<int> list_ids = await Tabs_manager_access.GetTabsIDAsync(IDs.ID_TabsList);
                AddTabs(list_ids);
            }

        }

        private async void CreateFileViaTab_Click(object sender, RoutedEventArgs e)
        {
            var creator = new TabsCreatorAssistant();
            await creator.CreateNewFileViaTab(IDs);
        }

        private void PinnedModuleButton_Click(object sender, RoutedEventArgs e)
        {
            var module = (sender as Button).DataContext as PinnedModule;
            new AddonExecutor(module.ID, new SCEELibs.SCEELibs(module.ID)).ExecuteDefaultFunction(AddonExecutorFuncTypes.main);

            IList<SCEELibs.Modules.ModuleInfo> test = new SCEELibs.Modules.Manager().getThemesAvailable(true);
        }
    }

}
