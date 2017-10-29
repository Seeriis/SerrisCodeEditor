using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisModulesServer.Type.Addon
{
    public enum WidgetType
    {
        Button,
        TextBox
    }

    public class AddonWidget
    {
        public WidgetType Type { get; set; }
        public string WidgetName { get; set; }
        public string FunctionName { get; set; }

        //Button
        public string IconButton { get; set; }

        //TextBox
        public string PlaceHolderText { get; set; }

        //Flyout
        public bool WithFlyout { get; set; }
        public string PathFlyoutPage { get; set; }
    }
}
