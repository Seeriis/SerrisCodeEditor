using SerrisCodeEditor.Functions.News;
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

namespace SerrisCodeEditor.Xaml.Views
{

    public sealed partial class NewsReader : Page
    {
        private int ArticleID = 0;
        private bool CommentsAlreadyLoaded = false;

        public NewsReader(NewsContent news)
        {
            this.InitializeComponent();

            ArticleID = news.ArticleID;
            TitleContent.Text = news.Title;
            DateContent.Text = news.Date;
            AuthorContent.Text = news.Author;
            HeaderImage.Source = new BitmapImage(new Uri(news.HeaderImage));
            ContentView.NavigateToString(news.Content);
        }

        private void ShowCommentsButton_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Visibility = Visibility.Collapsed;
            CommentsGrid.Visibility = Visibility.Visible;

            if(!CommentsAlreadyLoaded)
            {
                CommentsView.Navigate(new Uri($"https://sce.seeriis.net/News/Comments/{ArticleID}"));
                CommentsAlreadyLoaded = true;
            }
        }

        private void GoBackButton_Click(object sender, RoutedEventArgs e)
        {
            ContentGrid.Visibility = Visibility.Visible;
            CommentsGrid.Visibility = Visibility.Collapsed;
        }
    }

}
