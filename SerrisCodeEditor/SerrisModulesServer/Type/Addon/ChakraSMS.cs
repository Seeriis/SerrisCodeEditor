using ChakraBridge;

namespace SerrisModulesServer.Type.Addon
{
    public class ChakraSMS
    {
        public ChakraHost Chakra;

        public ChakraSMS()
        {
            Chakra = new ChakraHost();

            /*
             *  ====================================
             *  = NAMESPACES CHAKRAHOST REGISTERED =
             *  ====================================
             */

            Chakra.ProjectNamespace("System");
            Chakra.ProjectNamespace("Windows.UI.Xaml.Controls");
            Chakra.ProjectNamespace("Windows.ApplicationModel.DataTransfer");
            Chakra.ProjectNamespace("Windows.UI.Popups");
            Chakra.ProjectNamespace("Windows.Foundation");
            Chakra.ProjectNamespace("Windows.Storage");
            Chakra.ProjectNamespace("SerrisModulesServer");
            Chakra.ProjectNamespace("SerrisModulesServer.Manager");
            Chakra.ProjectNamespace("SerrisModulesServer.Items");

            Chakra.ProjectNamespace("Windows.UI.Notifications");
            Chakra.ProjectNamespace("Windows.Data.Xml.Dom");
            Chakra.ProjectNamespace("SCEELibs.Helpers");
        }

    }
}
