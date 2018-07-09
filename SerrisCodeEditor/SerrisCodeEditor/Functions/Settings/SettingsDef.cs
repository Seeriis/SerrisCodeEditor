using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisCodeEditor.Functions.Settings
{
    public enum SettingType
    {
        Checkbox,
        TextboxText,
        TextboxNumber,
        SecondDescription,
        Link,
        ComboBox
    }

    public class SettingsMenu
    {
        public string Name { get; set; }
        public string Icon { get; set; } //SEGOE MDL2 ASSETS

        public List<Setting> Settings { get; set; }
    }

    public class Setting
    {
        public string Description { get; set; }
        public SettingType Type { get; set; }
        public object Parameter { get; set; }

        public string VarSaveName { get; set; }
        public object VarSaveDefaultContent { get; set; }
    }
}
