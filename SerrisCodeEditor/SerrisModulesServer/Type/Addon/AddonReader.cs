using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using SerrisModulesServer.Type.Theme;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisModulesServer.Type.Addon
{
    public class AddonReader : ModuleReader
    {
        public AddonReader(int ID) : base(ID) { }

        public async Task<string> GetAddonMainJsViaIDAsync()
        {
            try
            {
                StorageFile MainFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModuleFolderPath + "main.js"));

                using (var reader = new StreamReader(await MainFile.OpenStreamForReadAsync()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch
            {
                return "";
            }

        }

        public async Task<StackPanel> GetAddonWidgetViaIDAsync(object sceelibs, ThemeModuleBrush theme)
        {
            StorageFile WidgetFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModuleFolderPath + "widget.json"));
            var widget_content = new StackPanel { Padding = new Thickness(5, 0, 10, 0), Orientation = Orientation.Horizontal, Name = "" + ModuleID };

            using (var reader = new StreamReader(await WidgetFile.OpenStreamForReadAsync()))
            using (JsonReader JsonReader = new JsonTextReader(reader))
            {
                try
                {
                    List<AddonWidget> list = new JsonSerializer().Deserialize<List<AddonWidget>>(JsonReader);
                    if (list != null)
                    {
                        foreach (AddonWidget widget in list)
                        {
                            switch (widget.Type)
                            {
                                case WidgetType.Button:
                                    var new_button = new Button();
                                    new_button.Margin = new Thickness(5, 0, 5, 0);

                                    new_button.Name = widget.WidgetName + ModuleID; new_button.Style = (Style)Application.Current.Resources["Round_Button"];
                                    new_button.Padding = new Thickness(0);
                                    new_button.Width = 25; new_button.Height = 25;
                                    new_button.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets");
                                    new_button.Content = widget.IconButton;
                                    new_button.Foreground = theme.ToolbarColorFont; new_button.Background = new SolidColorBrush(Colors.Transparent);
                                    new_button.Click += ((e, f) =>
                                    {
                                        Task.Run(() => new AddonExecutor(ModuleID, sceelibs).ExecutePersonalizedFunction(widget.FunctionName));
                                    });

                                    widget_content.Children.Add(new_button);

                                    break;

                                case WidgetType.TextBox:
                                    var new_textbox = new TextBox();
                                    new_textbox.Margin = new Thickness(5, 0, 5, 0);

                                    new_textbox.Name = widget.WidgetName + ModuleID; new_textbox.Style = (Style)Application.Current.Resources["RoundTextBox"];
                                    //new_textbox.Padding = new Thickness(0);
                                    new_textbox.Width = 150; new_textbox.Height = 25;
                                    new_textbox.PlaceholderText = widget.PlaceHolderText;
                                    new_textbox.FontSize = 14; new_textbox.Background = theme.ToolbarColorFont; new_textbox.Foreground = theme.ToolbarColor;
                                    new_textbox.KeyDown += (async (e, f) =>
                                    {
                                        if (f.KeyStatus.RepeatCount == 1)
                                        {
                                            if (f.Key == Windows.System.VirtualKey.Enter)
                                            {
                                                await Task.Run(() => new AddonExecutor(ModuleID, sceelibs).ExecutePersonalizedFunction(widget.FunctionName));
                                            }
                                        }
                                    });

                                    widget_content.Children.Add(new_textbox);
                                    break;
                            }
                        }
                    }
                }
                catch
                {
                    return null;
                }
            }

            return widget_content;
        }
    }
}
