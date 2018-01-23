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

            //UI
            new SettingsMenu
            {
                Name = "UI",
                Icon = "",

                Settings = new List<Setting>
                {
                    new Setting
                    {
                        Description = "UI extended view mode",
                        Type = SettingType.Checkbox,

                        VarSaveName = "ui_extendedview",
                        VarSaveDefaultContent = false
                    },

                    new Setting
                    {
                        Description = "Close panel automatically",
                        Type = SettingType.Checkbox,

                        VarSaveName = "ui_closepanelauto",
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

                        Parameter = "By Seeris"
                    },

                    new Setting
                    {
                        Description = "Main developer",
                        Type = SettingType.SecondDescription,

                        Parameter = "DeerisLeGris (France)"
                    },

                    new Setting
                    {
                        Description = "Version",
                        Type = SettingType.SecondDescription,

                        Parameter = new SCEELibs.SCEInfos().versionName
                    },

                    new Setting
                    {
                        Description = "Become a SCE developer on GitHub !",
                        Type = SettingType.Link,

                        Parameter = "https://github.com/Seeris/SerrisCodeEditor"
                    }
                }

            }


        };
    }
}
