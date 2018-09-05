using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisCodeEditor.Xaml.Views;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public sealed partial class ModulesSheetManager : UserControl
    {
        string sheet_tabslist = "123456789";

        public ModulesSheetManager()
        {
            this.InitializeComponent();
        }

        private void ModulesSheetContent_Loaded(object sender, RoutedEventArgs e)
        {
            AddTabsListSheet();
            //Messenger.Default.Send(new ModuleSheetNotification { id = sheet_tabslist, sheetName = "Tabs list", type = ModuleSheetNotificationType.NewSheet, sheetContent = new TabsViewer(), sheetIcon = new BitmapImage(new Uri(this.BaseUri, "/Assets/Icons/tabs.png")), sheetSystem = true });
        }

        private void SheetManager_Loaded(object sender, RoutedEventArgs e)
        { SetMessenger(); }



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private void SetMessenger()
        {
            Messenger.Default.Register<ModuleSheetNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        switch (notification.type)
                        {
                            case ModuleSheetNotificationType.NewSheet:
                                AddModule(notification);
                                break;

                            case ModuleSheetNotificationType.RemoveSheet:
                                RemoveModule(notification.id);
                                SelectTabsListSheet();
                                break;

                            case ModuleSheetNotificationType.InitalizedSheet:
                                if (notification.id == sheet_tabslist)
                                    SelectTabsListSheet();

                                break;
                        }
                    }
                    catch { }
                });

            });

        }

        private void AddModule(ModuleSheetNotification notif)
        {
            string name = "" + notif.id;
            while (ModulesSheetContent.FindName(name) != null)
            {
                name = "" + (int.Parse(name) + 1);
            }

            ModuleSheet button = new ModuleSheet();
            button.DataContext = notif;

            button.Name = name;
            button.Margin = new Thickness(0, 5, 0, 0);

            ModulesSheetContent.Children.Add(button);
        }

        public void SelectTabsListSheet()
        { Messenger.Default.Send(new ModuleSheetNotification { id = sheet_tabslist, sheetName = GlobalVariables.GlobalizationRessources.GetString("tabslist-titlesheet"), type = ModuleSheetNotificationType.TriggerSheet, sheetIcon = new BitmapImage(new Uri(this.BaseUri, "/Assets/Icons/tabs.png")), sheetSystem = true }); }

        public void AddTabsListSheet()
        { Messenger.Default.Send(new ModuleSheetNotification { id = sheet_tabslist, sheetName = GlobalVariables.GlobalizationRessources.GetString("tabslist-titlesheet"), type = ModuleSheetNotificationType.NewSheet, sheetContent = new TabsViewer(), sheetIcon = new BitmapImage(new Uri(this.BaseUri, "/Assets/Icons/tabs.png")), sheetSystem = true }); }

        private void RemoveModule(string ID)
        { ModulesSheetContent.Children.Remove((UIElement)ModulesSheetContent.FindName("" + ID)); }

    }
}
