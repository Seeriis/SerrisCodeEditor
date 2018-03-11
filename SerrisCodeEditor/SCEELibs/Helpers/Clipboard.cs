using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;

namespace SCEELibs.Helpers
{
    public static class Clipboard
    {
        static private DataPackage DataPackage = new DataPackage();

        public async static void SetContent(string content)
        {
            DataPackage.SetText(content);
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Windows.ApplicationModel.DataTransfer.Clipboard.SetContent(DataPackage);
            });
        }

        public static string GetContent()
        {
            return Task.Run(async () => 
            {
                return await DispatcherHelper.ExecuteOnUIThreadAsync(async () =>
                {
                    return await Windows.ApplicationModel.DataTransfer.Clipboard.GetContent().GetTextAsync();
                });
                
            }).Result;
        }
    }
}
