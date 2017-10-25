using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEELibs.Editor.Notifications
{
    public enum EditorViewNotificationType
    {
        UpdateUI
    }

    public sealed class EditorViewNotification
    {
        public int ID { get; set; }
        public EditorViewNotificationType type { get; set; }
    }
}
