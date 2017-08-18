using Newtonsoft.Json;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisTabsServer.Items
{
    public enum ContentType
    {
        File,
        Folder
    }

    public struct CursorPosition
    {
        public int row { get; set; }
        public int column { get; set; }
    }

    public class InfosTab
    {
        public int ID { get; set; }
        public string PathContent { get; set; }
        public ContentType TabContentType { get; set; }

        //If "TabContentType" == 1
        public List<InfosTab> FolderContent { get; set; }

        //If "TabContentType" == 0
        public string TabAccessOriginalContent { get; set; }
        public StorageTypes TabStorageMode { get; set; }
        public string TabType { get; set; }
        public string TabName { get; set; }
        public int TabEncoding { get; set; }
        public bool TabNewModifications { get; set; }
        public string TabDateModified { get; set; }
        public CursorPosition TabCursorPosition { get; set; }

        [JsonIgnore]
        public string TabContentTemporary { get; set; }

        public bool CanBeDeleted { get; set; }
        public bool CanBeModified { get; set; }
    }
}
