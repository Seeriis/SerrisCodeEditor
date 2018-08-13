using Microsoft.Toolkit.Uwp.Helpers;
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

namespace SerrisCodeEditor.Xaml.Views
{
    public class WindowFlyoutContent
    {
        public string WindowIcon { get; set; }
        public string WindowTitle { get; set; }

        public Type Content { get; set; }
    }

    public sealed partial class WindowFlyout : Page
    {
        public WindowFlyout()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            WindowFlyoutContent Content = e.Parameter as WindowFlyoutContent;

            IconTitle.Text = Content.WindowIcon;
            TextTitle.Text = Content.WindowTitle;
            WindowContent.Navigate(Content.Content);
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetTheme();
        }

        private void SetTheme()
        {
            TitleBG.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            TextTitle.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            IconTitleBG.Fill = GlobalVariables.CurrentTheme.SecondaryColorFont;
            IconTitle.Foreground = GlobalVariables.CurrentTheme.SecondaryColor;

            BorderFlyout.Stroke = GlobalVariables.CurrentTheme.SecondaryColorFont;
            BorderTitle.Fill = GlobalVariables.CurrentTheme.SecondaryColorFont;
        }
    }

}
