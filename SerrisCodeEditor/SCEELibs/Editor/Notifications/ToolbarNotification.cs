using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace SCEELibs.Editor.Notifications
{
    public enum ToolbarProperties
    {
        Visibility,

        ButtonEnabled,
        IsButtonEnabled,

        SetTextBoxContent,
        GetTextBoxContent,

        FlyoutEnabled
    }

    public sealed class ToolbarNotification
    {
        public int id { get; set; } //ID of the module who sent the notification
        public string uiElementName { get; set; }
        public bool answerNotification { get; set; }

        public ToolbarProperties propertie { get; set; }
        public object content { get; set; }
    }
}
