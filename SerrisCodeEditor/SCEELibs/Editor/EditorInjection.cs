using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace SCEELibs.Editor
{

    [AllowForWeb]
    public sealed class EditorInjection
    {
        public void injectJS(string content)
        => Messenger.Default.Send(new SCEENotification { type = SCEENotifType.Injection, content = content });

    }
}
