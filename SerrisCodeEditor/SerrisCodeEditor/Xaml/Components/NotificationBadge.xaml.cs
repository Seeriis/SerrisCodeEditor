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
    public sealed partial class NotificationBadge : UserControl
    {
        public NotificationBadge()
        {
            this.InitializeComponent();
        }

        private void userControl_Loaded(object sender, RoutedEventArgs e)
        {
            SetTheme();

            Messenger.Default.Register<EditorViewNotification>(this, async (notification_ui) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
                {
                    try
                    {
                        SetTheme();
                    }
                    catch { }

                });

            });
        }

        private void SetTheme()
        {
            GridCircle.Background = GlobalVariables.CurrentTheme.RoundNotificationColor;
            GridCircle.BorderBrush = GlobalVariables.CurrentTheme.RoundBorderNotificationColor;
        }

        public bool ShowBadge
        {
            get { return (bool)GetValue(ShowBadgeProperty); }
            set
            {
                SetValue(ShowBadgeProperty, value);

                if (ShowBadge)
                {
                    ShowBadgeAnimation.Begin();
                }
                else
                {
                    HideBadgeAnimation.Begin();
                }
            }
        }

        public static readonly DependencyProperty ShowBadgeProperty = DependencyProperty.Register("ShowBadge", typeof(bool), typeof(NotificationBadge), null);

    }

}
