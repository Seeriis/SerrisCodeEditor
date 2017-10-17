using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
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
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public sealed partial class ModuleSheetView : UserControl
    {
        public ModuleSheetView()
        {
            this.InitializeComponent();
        }

        private void SheetView_Loaded(object sender, RoutedEventArgs e)
        { SetMessenger(); }

        private void SetMessenger()
        {
            Messenger.Default.Register<ModuleSheetNotification>(this, (notification) =>
            {
                try
                {
                    if (notification.type == ModuleSheetNotificationType.SelectSheet)
                    {
                        FrameView.Content = notification.sheetContent;
                    }
                }
                catch { }
            });

        }
    }
}
