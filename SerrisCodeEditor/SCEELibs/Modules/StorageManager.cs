using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;
using Windows.Storage;

namespace SCEELibs.Modules
{
    [AllowForWeb]
    public sealed class StorageManager
    {
        int currentID;
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

        public StorageManager(int id)
        => currentID = id;

        private string generateSettingName(string name)
        {
            return string.Format("{0}_{1}", currentID, name);
        }

        public IAsyncOperation<StorageFolder> getTemporaryFolder()
        {
            return Task.Run(() =>
            {

                return ApplicationData.Current.TemporaryFolder;

            }).AsAsyncOperation();
        }

        public IAsyncOperation<StorageFolder> getModuleFolder()
        {
            return Task.Run(async () =>
            {

                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                return await folder_content.CreateFolderAsync(currentID + "", CreationCollisionOption.OpenIfExists);

            }).AsAsyncOperation();
        }

        //SETTINGS generator

        public bool checkAppSettingAvailable(string name)
        {
            return AppSettings.Values.ContainsKey(generateSettingName(name));
        }

        public object readAppSettingContent(string name)
        {
            return AppSettings.Values[generateSettingName(name)];
        }

        public bool removeAppSetting(string name)
        {
            try
            {
                AppSettings.Values.Remove(generateSettingName(name));
                return true;
            }
            catch { return false; }
        }

        public bool writeAppSetting(string name, object content)
        {
            try
            {
                AppSettings.Values[generateSettingName(name)] = content;
                return true;
            }
            catch { return false; }
        }

    }
}
