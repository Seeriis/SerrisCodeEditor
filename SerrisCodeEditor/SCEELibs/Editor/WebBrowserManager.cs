using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Components;
using SCEELibs.Editor.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Windows.UI.Xaml.Media.Imaging;

namespace SCEELibs.Editor
{
    [AllowForWeb]
    public sealed class WebBrowserManager
    {

        public void openWebBrowser()
        {
            openWebBrowserWithURL("https://qwant.com/?b=0");
        }

        public async void openWebBrowserWithURL(string url)
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Messenger.Default.Send(new ModuleSheetNotification { id = "-1", sheetName = "Web browser", type = ModuleSheetNotificationType.NewSheet, sheetContent = new WebBrowser(url), sheetIcon = new BitmapImage(new Uri("ms-appx://SerrisCodeEditor/Assets/Icons/web_browser.png")), sheetSystem = false });
                Messenger.Default.Send(SheetViewerNotification.DeployViewer);
            });
        }

    }
}
