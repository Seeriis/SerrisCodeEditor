using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisCodeEditor.Items
{
    public enum ContactTypeSCEE
    {
        GetCodeForTab,
        SetCodeForEditor
    }

    public class ContactSCEE
    {
        public TabID IDs { get; set; }
        public ContactTypeSCEE ContactType { get; set; }
        public string Code { get; set; }
        public string TypeCode { get; set; }
    }
}
