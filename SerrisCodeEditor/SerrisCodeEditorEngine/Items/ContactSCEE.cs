using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisCodeEditorEngine.Items
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
    }
}
