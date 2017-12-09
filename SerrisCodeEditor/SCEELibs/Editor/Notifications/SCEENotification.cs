using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEELibs.Editor.Notifications
{

    public enum SCEENotifType
    {
        Injection,
        InjectionAndReturn,
        SaveCurrentTab
    }

    public sealed class SCEENotification
    {
        public SCEENotifType type { get; set; }
        public object content { get; set; }
        public bool answerNotification { get; set; }

    }
}
