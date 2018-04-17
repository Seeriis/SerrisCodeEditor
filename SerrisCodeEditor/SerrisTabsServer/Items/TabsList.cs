using SerrisModulesServer;
using System.Collections.Generic;

namespace SerrisTabsServer.Items
{
    public class TabsList
    {
        public int ID { get; set; }
        public int TabsListProjectTypeID { get; set; } = SMSInfos.DefaultTemplatesID;
        public string name { get; set; }
        public List<InfosTab> tabs { get; set; }
    }

    public struct TabID
    {
        public int ID_TabsList { get; set; }
        public int ID_Tab { get; set; }
    }
}
