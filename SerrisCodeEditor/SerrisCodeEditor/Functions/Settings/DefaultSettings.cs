using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisCodeEditor.Functions.Settings
{
    public static class DefaultSettings
    {
        public static SettingsMenu[] DefaultSettingsMenuList =
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
                        Description = "Show minimap",
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_minimap",
                        VarSaveDefaultContent = true
                    },

                    new Setting
                    {
                        Description = "Wrapping code",
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_wordwrap",
                        VarSaveDefaultContent = false
                    },

                    new Setting
                    {
                        Description = "Font size (in pixel)",
                        Type = SettingType.TextboxNumber,

                        VarSaveName = "editor_fontsize",
                        VarSaveDefaultContent = 14
                    },

                    new Setting
                    {
                        Description = "Font family",
                        Type = SettingType.ComboBox,

                        VarSaveName = "editor_fontfamily",
                        VarSaveDefaultContent = "Consolas",

                        Parameter = Microsoft.Graphics.Canvas.Text.CanvasTextFormat.GetSystemFontFamilies().ToList()
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
                        Description = "Close the left panel automatically",
                        Type = SettingType.Checkbox,

                        VarSaveName = "ui_closepanelauto",
                        VarSaveDefaultContent = true
                    },

                    new Setting
                    {
                        Description = "Left pane reduced length (in pixel)",
                        Type = SettingType.TextboxNumber,

                        VarSaveName = "ui_leftpanelength",
                        VarSaveDefaultContent = 60
                    },

                    new Setting
                    {
                        Description = "Left pane open length (in pixel)",
                        Type = SettingType.TextboxNumber,

                        VarSaveName = "ui_leftpaneopenlength",
                        VarSaveDefaultContent = 320
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

                        Parameter = "By Seeriis"
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

                        Parameter = SCEELibs.SCEInfos.versionName
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
