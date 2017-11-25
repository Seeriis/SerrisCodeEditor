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
    public sealed class ConsoleManager
    {
        public void sendConsoleInformationNotification(string content)
        {
            Messenger.Default.Send(new ConsoleNotification { date = DateTime.Now, typeNotification = ConsoleTypeNotification.Information, content = content });
        }

        public void sendConsoleErrorNotification(string content)
        {
            Messenger.Default.Send(new ConsoleNotification { date = DateTime.Now, typeNotification = ConsoleTypeNotification.Error, content = content });
        }

        public void sendConsoleResultNotification(string content)
        {
            Messenger.Default.Send(new ConsoleNotification { date = DateTime.Now, typeNotification = ConsoleTypeNotification.Result, content = content });
        }

        public void log(string content)
        {
            Messenger.Default.Send(new ConsoleNotification { date = DateTime.Now, typeNotification = ConsoleTypeNotification.Result, content = content });
        }

        public void sendConsoleWarningNotification(string content)
        {
            Messenger.Default.Send(new ConsoleNotification { date = DateTime.Now, typeNotification = ConsoleTypeNotification.Warning, content = content });
        }

    }
}
