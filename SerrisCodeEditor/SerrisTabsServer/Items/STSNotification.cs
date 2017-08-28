using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Items
{
    public enum TypeUpdateTab
    {
        TabUpdated,
        NewTab,
        TabDeleted,
        NewList,
        ListDeleted
    }

    public class STSNotification
    {
        public TypeUpdateTab Type { get; set; }
        public TabID ID { get; set; }
    }
}
