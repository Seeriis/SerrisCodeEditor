using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCEELibs.Editor.Notifications
{
    public enum ConsoleTypeNotification
    {
        Error,
        Warning,
        Information,
        Result
    }

    public sealed class ConsoleNotification
    {
        public ConsoleTypeNotification typeNotification { get; set; }
        public string content { get; set; }
        public DateTimeOffset date { get; set; }
        
        //For know where come from the error, information...
        public int id_source { get; set; }
        public int id_tabslist_source { get; set; }
    }
}
