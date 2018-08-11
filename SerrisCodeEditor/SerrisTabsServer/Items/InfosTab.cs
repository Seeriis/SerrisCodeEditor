using Newtonsoft.Json;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;

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
        public ContentType TabContentType { get; set; }

        //If "TabContentType" == 1
        public List<int> FolderContent { get; set; }
        public bool FolderOpened { get; set; }

        //If "TabContentType" == 0
        public bool TabInvisibleByDefault { get; set; }
        public string TabOriginalPathContent { get; set; }
        public StorageListTypes TabStorageMode { get; set; }
        public string TabType { get; set; }
        public string TabName { get; set; }
        public int TabEncoding { get; set; }
        public bool TabNewModifications { get; set; }
        public DateTimeOffset DateTabContentUpdated { get; set; }
        public string TabDateModified { get; set; }
        public CursorPosition TabCursorPosition { get; set; }

        [JsonIgnore]
        public string TabContentTemporary { get; set; }

        public string TabMonacoModelID { get; set; }

        public bool CanBeDeleted { get; set; }
        public bool CanBeModified { get; set; }
    }
}
