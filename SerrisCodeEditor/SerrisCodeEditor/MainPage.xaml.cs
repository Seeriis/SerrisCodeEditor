using GalaSoft.MvvmLight.Messaging;
using Newtonsoft.Json;
using SerrisCodeEditor.Items;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Addon;
using SerrisModulesServer.Type.Theme;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

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
            this.InitializeComponent();
            ChakraSMS sms = new ChakraSMS(); //Initialize Chakra Engine (important)

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            lel();
        }

        public void PushCodeViaIDs()
        {
            Messenger.Default.Send(new ContactSCEE { IDs = IDs, Code = ContentViewer.Code, ContactType = ContactTypeSCEE.GetCodeForTab });
        }

        public async void lel()
        {
            Messenger.Default.Register<STSNotification>(this, (notification) =>
            {
                try
                {
                    if (IDs.ID_TabsList == notification.ID.ID_TabsList)
                    {

                        switch (notification.Type)
                        {
                            case TypeUpdateTab.NewTab:
                                list_ids.Items.Add(notification.ID.ID_Tab);
                                break;

                            case TypeUpdateTab.TabDeleted:
                                list_ids.Items.Remove(notification.ID.ID_Tab);
                                break;

                            case TypeUpdateTab.NewList:
                                list_ids_list.Items.Add(notification.ID.ID_TabsList);
                                break;

                            case TypeUpdateTab.ListDeleted:
                                list_ids_list.Items.Remove(notification.ID.ID_TabsList);
                                break;
                        }

                    }
                    else
                        switch (notification.Type)
                        {
                            case TypeUpdateTab.NewList:
                                list_ids_list.Items.Add(notification.ID.ID_TabsList);
                                break;

                            case TypeUpdateTab.ListDeleted:
                                list_ids_list.Items.Remove(notification.ID.ID_TabsList);
                                break;
                        }
                }
                catch { }
            });

            Messenger.Default.Register<ContactSCEE>(this, (notification) =>
            {
                try
                {
                    if (notification.ContactType == ContactTypeSCEE.SetCodeForEditor)
                    {
                        IDs = notification.IDs;
                        ContentViewer.CodeLanguage = notification.TypeCode; ContentViewer.Code = notification.Code;
                    }
                }
                catch { }
            });

            var sts_initialize = await Tabs_manager_access.GetTabsListIDAsync();
            
            if(sts_initialize.Count == 0)
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
            }

            list_ids_list.Items.Clear();
            foreach(int id in sts_initialize)
            {
                list_ids_list.Items.Add(id);
            }

            var sms_initialize = await Modules_manager_access.GetModulesAsync(true);
            foreach(InfosModule module in sms_initialize)
            {
                if(module.ModuleType == SerrisModulesServer.Type.ModuleTypesList.Addon)
                {
                    PinnedModule pinned = new PinnedModule { ID = module.ID, ModuleName = module.ModuleName, ModuleType = module.ModuleType };
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

            ThemeModuleBrush brushs_theme = await new ThemeReader(await Modules_manager_access.GetCurrentThemeID()).GetThemeBrushsContent();
            image_bg.ImageSource = brushs_theme.BackgroundImage;
        }

        public void AddTabs(List<int> tabs)
        {
            list_ids.Items.Clear();
            foreach(int nmb in tabs)
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
                Frame frame = new Frame();
                frame.Navigate(typeof(MainPage), null);
                Window.Current.Content = frame;
                Window.Current.Activate();

                newViewId = ApplicationView.GetForCurrentView().Id;
            });
            bool viewShown = await ApplicationViewSwitcher.TryShowAsStandaloneAsync(newViewId);
        }

        private async void OpenFiles_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.OpenFilesAndCreateNewTabsFiles(IDs.ID_TabsList, StorageListTypes.LocalStorage);
        }

        private async void list_ids_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(list_ids.SelectedItem != null)
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
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
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

                if(tabslist != null)
                    name_box.Text = tabslist.name;

                List<int> list_ids = await Tabs_manager_access.GetTabsIDAsync(IDs.ID_TabsList);
                AddTabs(list_ids);
            }

        }

        private async void CreateFileViaTab_Click(object sender, RoutedEventArgs e)
        {
            TabsCreatorAssistant creator = new TabsCreatorAssistant();
            await creator.CreateNewFileViaTab(IDs);
        }

        private void PinnedModuleButton_Click(object sender, RoutedEventArgs e)
        {
            PinnedModule module = (sender as Button).DataContext as PinnedModule;
            Flyout osef = new Flyout(); Frame osef_b = new Frame();
            AddonExecutor executor = new AddonExecutor(module.ID, AddonExecutorFuncTypes.main, ref osef, ref osef_b);
        }
    }

}
