using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisCodeEditor.Functions.News;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
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

    public class NewsItemList
    {
        public BitmapImage HeaderImage { get; set; }
        public News NewsItem { get; set; }
    }

    public sealed partial class NewsViewer : Page
    {
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

        public NewsViewer()
        {
            this.InitializeComponent();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            SetTheme();
        }

        private void SetTheme()
        {
            BackgroundBlur.Fill = GlobalVariables.CurrentTheme.MainColor;
            RefreshButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            ToggleNotifications.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
        }

        private void SetNewsOnTheList(List<News> List)
        {
            NewsList.Items.Clear();

            foreach (var item in List)
            {
                NewsList.Items.Add(new NewsItemList { HeaderImage = new BitmapImage(new Uri(item.HeaderImage, UriKind.Absolute)), NewsItem = item });
            }

            LoadingGrid.IsLoading = false;
        }

        private void ToggleNotifications_Loaded(object sender, RoutedEventArgs e)
        {
            if (AppSettings.Values.ContainsKey("news_notifications"))
            {
                ToggleNotifications.IsOn = (bool)AppSettings.Values["news_notifications"];
            }
            else
            {
                ToggleNotifications.IsOn = true;
            }

        }

        private async void NewsList_Loaded(object sender, RoutedEventArgs e)
        {
            LoadingGrid.IsLoading = true;
            SetNewsOnTheList(await NewsHelper.GetNewsList());
        }

        private void ToggleNotifications_Toggled(object sender, RoutedEventArgs e)
        {
            AppSettings.Values["news_notifications"] = ToggleNotifications.IsOn;
        }

        private async void NewsList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(NewsList.SelectedIndex != -1)
            {
                try
                {
                    NewsItemList item = NewsList.SelectedItem as NewsItemList;
                    Messenger.Default.Send(new ModuleSheetNotification { id = -1, sheetName = $"News - {item.NewsItem.Title}", type = ModuleSheetNotificationType.NewSheet, sheetContent = new NewsReader(await NewsHelper.GetNewsContent(item.NewsItem.ArticleID)), sheetIcon = new BitmapImage(new Uri(this.BaseUri, "/Assets/Icons/news.png")), sheetSystem = false });
                    Messenger.Default.Send(SheetViewerNotification.DeployViewer);
                }
                catch { }
            }
        }

        private async void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            LoadingGrid.IsLoading = true;
            await NewsHelper.RefreshNewsList();
            SetNewsOnTheList(await NewsHelper.GetNewsList());
        }
    }

}
