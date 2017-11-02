using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using SerrisTabsServer.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace SCEELibs.Tabs.Items
{
    public enum ContentTypeInfos
    {
        File,
        Folder
    }

    [AllowForWeb]
    public sealed class CursorPositionInfos
    {
        public int row { get; set; }
        public int column { get; set; }
    }

    [AllowForWeb]
    public sealed class Tab
    {

        public TabIDs id { get; set; }
        public string pathContent { get; set; }
        public ContentTypeInfos tabContentType { get; set; }


        //If "TabContentType" == 0
        public string tabType { get; set; }
        public string tabName { get; set; }
        public bool tabNewModifications { get; set; }
        public DateTimeOffset dateTabContentUpdated { get; set; }
        public string tabDateModified { get; set; }

        /* =============
         * = FUNCTIONS =
         * =============
         */

        public void saveContentToTabIfIsCurrentTab()
        {

        }

        public async void saveContentToFile()
        {
            await new StorageRouter(new TabsAccessManager().GetTabViaID(new TabID { ID_Tab = id.tabID, ID_TabsList = id.listID }), id.listID).WriteFile();
        }
    }
}
