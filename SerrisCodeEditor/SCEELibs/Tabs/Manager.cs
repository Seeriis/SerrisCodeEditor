using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using SCEELibs.Tabs.Items;
using SerrisTabsServer.Items;
using SerrisTabsServer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Metadata;

namespace SCEELibs.Tabs
{

    [AllowForWeb]
    public sealed class Manager
    {

        public Tab getTabViaID(TabIDs id)
        {
            var tab = TabsAccessManager.GetTabViaID(new TabID { ID_Tab = id.tabID, ID_TabsList = id.listID });
            Tab newTab = new Tab();

            //Convert InfosTab (.NET Lib) to Tab (WinRT Component)
            newTab.id = id;
            newTab.dateTabContentUpdated = tab.DateTabContentUpdated;

            if(tab.TabOriginalPathContent == null)
                newTab.pathContent = "";
            else
                newTab.pathContent = tab.TabOriginalPathContent;

            switch (tab.TabContentType)
            {
                case ContentType.File:
                    newTab.tabContentType = ContentTypeInfos.File;
                    break;

                case ContentType.Folder:
                    newTab.tabContentType = ContentTypeInfos.Folder;
                    break;
            }
            newTab.tabDateModified = tab.TabDateModified;
            newTab.tabName = tab.TabName;
            newTab.tabNewModifications = tab.TabNewModifications;
            newTab.tabType = tab.TabType;

            return newTab;
        }

        public TabIDs getCurrentSelectedTabAndTabsListID()
        {
            bool notif_received = false;
            TabIDs result = new TabIDs();

            Messenger.Default.Register<TempContentNotification>(this, (notification) =>
            {
                if(notification.answerNotification && notification.type == TempContentType.currentIDs)
                {
                    TabID currentIDs = (TabID)notification.content;
                    result.listID = currentIDs.ID_TabsList;
                    result.tabID = currentIDs.ID_Tab;
                    notif_received = true;
                }
            });

            Messenger.Default.Send(new TempContentNotification { answerNotification = false, type = TempContentType.currentIDs });

            while (!notif_received) ;

            return result;
        }

        public IList<TabIDs> getTabsIDOfTheCurrentList()
        {
            int currentList = getCurrentSelectedTabAndTabsListID().listID;
            List<int> ids = TabsAccessManager.GetTabsID(currentList);
            IList<TabIDs> list_ids = new List<TabIDs>();

            foreach(int id in ids)
            {
                list_ids.Add(new TabIDs { listID = currentList, tabID = id });
            }

            return list_ids;
        }

        public async void createNewList(string listName)
        => await TabsWriteManager.CreateTabsListAsync(listName);

        public void createNewTabInTheCurrentList(string fileName, string content)
        => TabsCreatorAssistant.CreateNewTab(getCurrentSelectedTabAndTabsListID().listID, fileName, Encoding.UTF8, SerrisTabsServer.Storage.StorageListTypes.LocalStorage, content);

    }
}
