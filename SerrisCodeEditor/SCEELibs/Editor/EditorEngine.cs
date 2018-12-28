using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
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

        public string injectJSAndReturnResult(string content)
        {
            bool notif_received = false; string result = "";

            Messenger.Default.Register<SCEENotification>(this, (notification) =>
            {
                if (notification.answerNotification && notification.type == SCEENotifType.InjectionAndReturn)
                {
                    result = (string)notification.content;
                    notif_received = true;
                }
            });

            Messenger.Default.Send(new SCEENotification { type = SCEENotifType.InjectionAndReturn, answerNotification = false, content = content });

            while (!notif_received)
            {
                Task.Delay(10);
            }

            return result;

        }

        //Convert C# String to JS String (HttpUtility doesn't exist in WinRT) - this function has been fund here: http://joonhachu.blogspot.fr/2010/01/c-javascript-encoder.html
        public string javaScriptEncode(string s)
        {
            if (s == null || s.Length == 0)
            {
                return string.Empty;
            }
            char c;
            int i;
            int len = s.Length;
            var sb = new StringBuilder(len + 4);


            for (i = 0; i < len; ++i)
            {
                c = s[i];
                switch (c)
                {
                    case '\\':
                    case '"':
                    case '>':
                    case '\'':
                        sb.Append('\\');
                        sb.Append(c);
                        break;

                    case '\b':
                        sb.Append("\\b");
                        break;

                    case '\t':
                        sb.Append("\\t");
                        break;

                    case '\n':
                        sb.Append("\\n");
                        break;

                    case '\f':
                        sb.Append("\\f");
                        break;

                    case '\r':
                        sb.Append("\\r");
                        break;

                    default:
                        sb.Append(c);
                        break;
                }
            }
            return sb.ToString();
        }


    }
}
