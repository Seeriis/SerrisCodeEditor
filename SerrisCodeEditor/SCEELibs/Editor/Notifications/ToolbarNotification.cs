using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SCEELibs.Editor.Notifications
{
    public sealed class ToolbarNotification
    {
        public int id { get; set; } //ID of the module who sent the notification
        public UIElement widget { get; set; } //Widget of the module
    }
}
