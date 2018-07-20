using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Items
{
    public class OneDriveItem
    {
        public string ItemName { get; set; }
        public Stream StreamContent { get; set; }
        public OneDriveItemType ItemType { get; set; }
    }

    public enum OneDriveItemType
    {
        File,
        Folder
    }
}
