using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisTabsServer.Items
{
    public enum TypeUpdate
    {
        TabUpdated,
        NewTab,
        TabDeleted
    }

    public class STSNotification
    {
        public TypeUpdate Type { get; set; }
        public TabID ID { get; set; }
    }
}
