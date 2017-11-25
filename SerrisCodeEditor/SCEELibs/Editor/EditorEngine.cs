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
    public sealed class EditorEngine
    {
        public void injectJS(string content)
        => Messenger.Default.Send(new SCEENotification { type = SCEENotifType.Injection, content = content });

        public void saveCurrentSelectedTab()
        {
            //bool notif_received = false;

            Messenger.Default.Register<SCEENotification>(this, (notification) =>
            {
                if (notification.answerNotification && notification.type == SCEENotifType.SaveCurrentTab)
                {
                    //notif_received = true;
                }
            });

            Messenger.Default.Send(new SCEENotification { type = SCEENotifType.SaveCurrentTab, answerNotification = false });

            //while (!notif_received) ;
        }

    }
}
