using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEELibs.Editor.Notifications
{
    public enum TempContentType
    {
        currentIDs,
        currentDevice,
        currentTheme
    }

    public sealed class TempContentNotification
    {
        public TempContentType type { get; set; }
        public object content { get; set; }
        public bool answerNotification { get; set; }
    }
}
