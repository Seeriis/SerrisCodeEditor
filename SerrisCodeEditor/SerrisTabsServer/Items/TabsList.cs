using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Items
{
    public class TabsList
    {
        public int ID { get; set; }
        public string name { get; set; }
        public List<InfosTab> tabs { get; set; }
    }

    public struct TabID
    {
        public int ID_TabsList { get; set; }
        public int ID_Tab { get; set; }
    }
}
