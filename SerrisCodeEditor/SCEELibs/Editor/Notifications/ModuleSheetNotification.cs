using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace SCEELibs.Editor.Notifications
{
    public enum ModuleSheetNotificationType
    {
        NewSheet,
        RemoveSheet,
        SelectSheet,
        UpdatedSheet,
        InitalizedSheet
    }

    public sealed class ModuleSheetNotification
    {
        public int id { get; set; }

        public string sheetName { get; set; }
        public BitmapImage sheetIcon { get; set; }
        public object sheetContent { get; set; }
        public bool sheetSystem { get; set; }

        public ModuleSheetNotificationType type { get; set; }
    }
}
