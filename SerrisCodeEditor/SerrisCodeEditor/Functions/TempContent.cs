using SCEELibs.Editor;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Theme;
using SerrisTabsServer.Items;
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

        private static ThemeModuleBrush _CurrentTheme { get; set; }
        public ThemeModuleBrush CurrentTheme
        {
            get { return _CurrentTheme; }
            set { _CurrentTheme = value; }
        }

        private static ModulesWriteManager _moduleswritemanger = new ModulesWriteManager();
        public ModulesWriteManager ModulesWriteManager { get { return _moduleswritemanger; } }

        private static ModulesAccessManager _modulesaccessmanger = new ModulesAccessManager();
        public ModulesAccessManager ModulesAccessManager { get { return _modulesaccessmanger; } }

    }

}
