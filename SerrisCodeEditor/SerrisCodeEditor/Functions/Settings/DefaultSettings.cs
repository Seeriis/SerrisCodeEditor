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
                Name = GlobalVariables.GlobalizationRessources.GetString("settings-editor"),
                Icon = "",

                Settings = new List<Setting>
                {
                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-editor_showline"),
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_linenumbers",
                        VarSaveDefaultContent = true
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-editor_showminimap"),
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_minimap",
                        VarSaveDefaultContent = true
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-editor_quicksuggests"),
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_quicksuggestions",
                        VarSaveDefaultContent = true
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-editor_wrappingcode"),
                        Type = SettingType.Checkbox,

                        VarSaveName = "editor_wordwrap",
                        VarSaveDefaultContent = false
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-editor_fontsize"),
                        Type = SettingType.TextboxNumber,

                        VarSaveName = "editor_fontsize",
                        VarSaveDefaultContent = 14
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-editor_fontfamily"),
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
                Name = GlobalVariables.GlobalizationRessources.GetString("settings-ui"),
                Icon = "",

                Settings = new List<Setting>
                {
                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-ui_extendedview"),
                        Type = SettingType.Checkbox,

                        VarSaveName = "ui_extendedview",
                        VarSaveDefaultContent = false
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-ui_closepanelauto"),
                        Type = SettingType.Checkbox,

                        VarSaveName = "ui_closepanelauto",
                        VarSaveDefaultContent = true
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-ui_leftpanelength"),
                        Type = SettingType.TextboxNumber,

                        VarSaveName = "ui_leftpanelength",
                        VarSaveDefaultContent = 60
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-ui_leftpaneopenlength"),
                        Type = SettingType.TextboxNumber,

                        VarSaveName = "ui_leftpaneopenlength",
                        VarSaveDefaultContent = 320
                    }
                }

            },

            //CREDITS
            new SettingsMenu
            {
                Name = GlobalVariables.GlobalizationRessources.GetString("settings-credits"),
                Icon = "",

                Settings = new List<Setting>
                {
                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-credits_aboutapp"),
                        Type = SettingType.Separator
                    },

                    new Setting
                    {
                        Description = "Serris Code Editor",
                        Type = SettingType.SecondDescription,

                        Parameter = GlobalVariables.GlobalizationRessources.GetString("settings-credits_byseeriis")
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-credits_maindev"),
                        Type = SettingType.SecondDescription,

                        Parameter = "DeerisLeGris (France)"
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-credits_version"),
                        Type = SettingType.SecondDescription,

                        Parameter = SCEELibs.SCEInfos.versionName
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-credits_scegithub"),
                        Type = SettingType.Link,

                        Parameter = "https://github.com/Seeris/SerrisCodeEditor"
                    },

                    new Setting
                    {
                        Description = GlobalVariables.GlobalizationRessources.GetString("settings-credits_licenses"),
                        Type = SettingType.Separator
                    },

                    new Setting
                    {
                        Description = "JsBridge",
                        Type = SettingType.License,

                        Parameter = new Tuple<string, string>("by deltakosh (with Apache License 2.0)", "https://github.com/deltakosh/JsBridge")
                    },

                    new Setting
                    {
                        Description = "MVVMLight",
                        Type = SettingType.License,

                        Parameter = new Tuple<string, string>("by Laurent Bugnion (GalaSoft) (with MIT license)", "https://github.com/lbugnion/mvvmlight")
                    },

                    new Setting
                    {
                        Description = "Newtonsoft.Json",
                        Type = SettingType.License,

                        Parameter = new Tuple<string, string>("by James Newton-King (with MIT license)", "https://github.com/JamesNK/Newtonsoft.Json")
                    },

                    new Setting
                    {
                        Description = "UWP Community Toolkit",
                        Type = SettingType.License,

                        Parameter = new Tuple<string, string>("by Microsoft (with MIT license)", "https://github.com/Microsoft/UWPCommunityToolkit")
                    },

                    new Setting
                    {
                        Description = "Monaco Editor",
                        Type = SettingType.License,

                        Parameter = new Tuple<string, string>("by Microsoft (with MIT license)", "https://github.com/Microsoft/monaco-editor")
                    },

                    new Setting
                    {
                        Description = "SerialQueue",
                        Type = SettingType.License,

                        Parameter = new Tuple<string, string>("by Orion Edwards (with MIT license)", "https://github.com/borland/SerialQueue")
                    },

                    new Setting
                    {
                        Description = "Devicon",
                        Type = SettingType.License,

                        Parameter = new Tuple<string, string>("by Konpa (with MIT license)", "https://github.com/konpa/devicon/")
                    },
                }

            }


        };
    }
}
