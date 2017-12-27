using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisCodeEditor.Functions.Settings
{
    public class DefaultSettings
    {
        public List<SettingsMenu> DefaultSettingsMenuList = new List<SettingsMenu>
        {

            //EDITOR
            new SettingsMenu
            {
                Name = "Editor",
                Icon = "",

                Settings = new List<Setting>
                {
                    new Setting
                    {
                        Description = "Show line numbers",
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_linenumbers",
                        VarSaveDefaultContent = true
                    },

                    new Setting
                    {
                        Description = "Wrapping code",
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_wordwrap",
                        VarSaveDefaultContent = false
                    }
                }

            },

            //CREDITS
            new SettingsMenu
            {
                Name = "Credits",
                Icon = "",

                Settings = new List<Setting>
                {
                    new Setting
                    {
                        Description = "Serris Code Editor",
                        Type = SettingType.SecondDescription,

                        Parameter = "By Survivaliste Project"
                    },

                    new Setting
                    {
                        Description = "Main developer",
                        Type = SettingType.SecondDescription,

                        Parameter = "DeerisLeGris (France)"
                    },

                    new Setting
                    {
                        Description = "Become a SCE developer on GitHub !",
                        Type = SettingType.Link,

                        Parameter = "http://www.github.com/"
                    }
                }

            }


        };
    }
}
