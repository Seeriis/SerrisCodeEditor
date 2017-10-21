using SCEELibs.Editor;
using SerrisTabsServer.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Profile;

namespace SerrisCodeEditor.Functions
{

    public class TempContent
    {

        //Current selected list tabs and tab
        private static TabID _CurrentIDs { get; set; }
        public TabID CurrentIDs
        {
            get { return _CurrentIDs; }
            set { _CurrentIDs = value; }
        }

        public CurrentDevice CurrentDevice
        {
            get
            {
                if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
                    return CurrentDevice.WindowsMobile;

                if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Holographic")
                    return CurrentDevice.Hololens;

                return CurrentDevice.Desktop;
            }
        }

    }

}
