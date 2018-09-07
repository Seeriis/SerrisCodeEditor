using GalaSoft.MvvmLight.Messaging;
using SerrisCodeEditor.Functions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Views
{
    public enum BonjourViewControl
    {
        CloseView
    }

    public sealed partial class BonjourView : Page
    {
        public BonjourView()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetTheme();

            VersionTitle.Text = SCEELibs.SCEInfos.versionName;
            ShowNewVersionTitle.Begin();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ApplicationData.Current.LocalSettings.Values["version_sce"] = SCEELibs.SCEInfos.versionNumber;
            Messenger.Default.Send(BonjourViewControl.CloseView);
        }

        private async void ChangelogButton_Click(object sender, RoutedEventArgs e)
        => await Launcher.LaunchUriAsync(new Uri("https://yoshilegris.wordpress.com/2017/06/04/join-the-private-beta-of-serris-code-editor-marne-la-vallee-update-1-0/"));

        private void VideoChangelog_Loaded(object sender, RoutedEventArgs e)
        => VideoChangelog.Navigate(new Uri("https://www.youtube.com/embed/U4U19zwFENs"));

        private void VideoChangelog_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        => VideoShowAnimation.Begin();

        private void SetTheme()
        {
            SolidColorBrush MainColor = new SolidColorBrush(Windows.UI.Color.FromArgb(255, GlobalVariables.CurrentTheme.MainColor.Color.R, GlobalVariables.CurrentTheme.MainColor.Color.G, GlobalVariables.CurrentTheme.MainColor.Color.B));

            bgLeft.Background = MainColor;
            bgRight.Background = MainColor;

            separatorA.Fill = GlobalVariables.CurrentTheme.MainColorFont;
            separatorB.Fill = GlobalVariables.CurrentTheme.MainColorFont;
            VersionTitle.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            NowVersion.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

            ButtonChangelogIconBG.Background = GlobalVariables.CurrentTheme.MainColorFont;
            ButtonChangelogIcon.Foreground = GlobalVariables.CurrentTheme.MainColor;
            ButtonChangelogText.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

            CloseButtonText.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            CloseButtonIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            CloseButton.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;

            betaTitle.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            betaText.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

            SolidColorBrush ColorSettingsButtons = new SolidColorBrush(Windows.UI.Color.FromArgb(150, GlobalVariables.CurrentTheme.SecondaryColor.Color.R, GlobalVariables.CurrentTheme.SecondaryColor.Color.G, GlobalVariables.CurrentTheme.SecondaryColor.Color.B));
            bgColor.Fill = ColorSettingsButtons;
            bgColor.Stroke = GlobalVariables.CurrentTheme.SecondaryColor;
        }
    }
}
