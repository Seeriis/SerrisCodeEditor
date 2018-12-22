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

namespace SCEELibs.Editor.Components
{

    public sealed partial class WebBrowser : Page
    {
        public WebBrowser(string url)
        {
            this.InitializeComponent();

            View.Navigate(new Uri(url));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (View.CanGoBack)
                View.GoBack();
        }

        private void ForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (View.CanGoForward)
                View.GoForward();
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        => View.Refresh();

        private void View_NavigationCompleted(WebView sender, WebViewNavigationCompletedEventArgs args)
        {
            BackButton.IsEnabled = View.CanGoBack;
            ForwardButton.IsEnabled = View.CanGoForward;

            URLBox.Text = args.Uri.AbsoluteUri;
            ProgressLoading.IsActive = false;
        }

        private void URLBox_KeyDown(object sender, KeyRoutedEventArgs f)
        {
            if (f.KeyStatus.RepeatCount == 1)
            {
                if (f.Key == Windows.System.VirtualKey.Enter)
                {
                    try
                    {
                        string suplement = "";

                        if (!URLBox.Text.Contains("http://") && !URLBox.Text.Contains("https://"))
                        {
                            suplement = "http://";
                        }

                        View.Navigate(new Uri(suplement + URLBox.Text));
                    }
                    catch { }
                }
            }
        }

        private void View_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            ProgressLoading.IsActive = true;
        }
    }

}
