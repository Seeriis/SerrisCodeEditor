using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
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
        => SetMessenger();

        private void SetMessenger()
        {
            Messenger.Default.Register<ModuleSheetNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        if (notification.type == ModuleSheetNotificationType.SelectSheet)
                        {
                            FrameView.Content = notification.sheetContent;
                            FrameName.Text = notification.sheetName;
                        }
                    }
                    catch { }
                });

            });

            Messenger.Default.Register<SheetViewerNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    switch(notification)
                    {
                        case SheetViewerNotification.PinViewer:
                            GridUnpin.Background = GlobalVariables.CurrentTheme.SecondaryColor;
                            FrameName.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

                            UnpinButton.Background = GlobalVariables.CurrentTheme.MainColor;
                            UnpinButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                            GridUnpin.Visibility = Visibility.Visible;
                            break;

                        case SheetViewerNotification.UnpinViewer:
                            GridUnpin.Visibility = Visibility.Collapsed;
                            break;
                    }
                });

            });
        }

        private void UnpinButton_Click(object sender, RoutedEventArgs e)
        => Messenger.Default.Send(SheetViewerNotification.UnpinViewer);
    }
}
