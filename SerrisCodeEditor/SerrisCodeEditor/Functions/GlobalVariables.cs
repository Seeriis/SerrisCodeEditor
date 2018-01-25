using SCEELibs.Editor;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Theme;
using SerrisTabsServer.Items;
using Windows.System.Profile;
using Windows.UI.Xaml.Controls;

namespace SerrisCodeEditor.Functions
{

    public static class GlobalVariables
    {

        //Current selected list tabs and tab
        public static TabID CurrentIDs { get; set; }

        public static CurrentDevice CurrentDevice
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

        public static ThemeModuleBrush CurrentTheme { get; set; }

    }

}
