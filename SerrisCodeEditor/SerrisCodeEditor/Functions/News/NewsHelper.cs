using Microsoft.Toolkit.Uwp.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Notifications;
using Windows.Web.Http;

namespace SerrisCodeEditor.Functions.News
{
    public static class NewsHelper
    {
        private static StorageFile NewsListFile;
        private static ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;
        private static HttpClient NewsClient = new HttpClient();

        private static void LoadNewsData()
        {
            NewsListFile = NewsListFile ?? Task.Run(async () => { return await ApplicationData.Current.LocalFolder.CreateFileAsync("news.json", CreationCollisionOption.OpenIfExists); }).Result;
        }

        private async static void NotificationFunc()
        {
            List<News> List = await GetNewsList();

            if (List.Count != 0)
            {

                ToastContent toastContent = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            HeroImage = new ToastGenericHeroImage()
                            {
                                Source = List[0].HeaderImage
                            },

                            AppLogoOverride = new ToastGenericAppLogo()
                            {
                                Source = "ms-appx:///Assets/Icons/news.png",
                                HintCrop = ToastGenericAppLogoCrop.Circle
                            },

                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "News: " + List[0].Title
                                }
                            }
                        }
                    }

                };

                ToastNotification toast = new ToastNotification(toastContent.GetXml());
                ToastNotificationManager.CreateToastNotifier().Show(toast);
            }
        }

        private static void SendNewsNotification()
        {
            if (AppSettings.Values.ContainsKey("news_notifications"))
            {
                if((bool)AppSettings.Values["news_notifications"])
                {
                    NotificationFunc();
                }
            }
            else
            {
                NotificationFunc();
            }

        }

        public async static Task<int> GetCurrentNewsToken()
        {
            try
            {
                JObject content = JObject.Parse(await NewsClient.GetStringAsync(new Uri("https://sce.seeriis.net/")));
                return content.GetValue("Token").ToObject<int>();
            }
            catch
            {
                return 0;
            }
        }

        private async static Task<List<News>> GetNewsOnLocalFile()
        {
            LoadNewsData();

            using (StreamReader Reader = new StreamReader(await NewsListFile.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(Reader))
            {
                return new JsonSerializer().Deserialize<List<News>>(JsonReader);
            }
        }

        public async static void CheckNewsUpdate()
        {
            if (AppSettings.Values.ContainsKey("news_token"))
            {
                if ((int)AppSettings.Values["news_token"] != await GetCurrentNewsToken())
                {
                    await RefreshNewsList();
                    SendNewsNotification();
                }
            }
            else
            {
                await RefreshNewsList();
                SendNewsNotification();
            }
        }

        public async static Task RefreshNewsList()
        {
            LoadNewsData();

            try
            {
                JObject Content = JObject.Parse(await NewsClient.GetStringAsync(new Uri("https://sce.seeriis.net/")));

                //Token
                AppSettings.Values["news_token"] = Content.GetValue("Token").ToObject<int>();

                //News list
                List<News> NewsList = Content.GetValue("News").ToObject<List<News>>();
                await FileIO.WriteTextAsync(NewsListFile, JsonConvert.SerializeObject(NewsList, Formatting.Indented));
            }
            catch { }

        }

        public async static Task<List<News>> GetNewsList()
        {
            LoadNewsData();

            try
            {
                if (AppSettings.Values.ContainsKey("news_token"))
                {
                    if ((int)AppSettings.Values["news_token"] != await GetCurrentNewsToken())
                    {
                        JObject Content = JObject.Parse(await NewsClient.GetStringAsync(new Uri("https://sce.seeriis.net/")));

                        //Token
                        AppSettings.Values["news_token"] = Content.GetValue("Token").ToObject<int>();

                        //News list
                        List<News> NewsList = Content.GetValue("News").ToObject<List<News>>();
                        await FileIO.WriteTextAsync(NewsListFile, JsonConvert.SerializeObject(NewsList, Formatting.Indented));

                        return await GetNewsOnLocalFile();
                    }
                    else
                    {
                        return await GetNewsOnLocalFile();
                    }
                }
                else
                {
                    JObject Content = JObject.Parse(await NewsClient.GetStringAsync(new Uri("https://sce.seeriis.net/")));

                    //Token
                    AppSettings.Values["news_token"] = Content.GetValue("Token").ToObject<int>();

                    //News list
                    List<News> NewsList = Content.GetValue("News").ToObject<List<News>>();
                    await FileIO.WriteTextAsync(NewsListFile, JsonConvert.SerializeObject(NewsList, Formatting.Indented));

                    return await GetNewsOnLocalFile();
                }
            }
            catch
            {
                return new List<News>();
            }
            
        }

        public async static Task<NewsContent> GetNewsContent(int ArticleID)
        {
            try
            {
                JObject Content = JObject.Parse(await NewsClient.GetStringAsync(new Uri($"https://sce.seeriis.net/News/ArticleJson/{ArticleID}")));
                return Content.ToObject<NewsContent>();
            }
            catch { return new NewsContent(); }
            
        }
    }
}
