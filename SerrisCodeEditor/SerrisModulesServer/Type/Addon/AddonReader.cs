using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
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
    public class AddonReader
    {
        ModulesAccessManager AccessManager;
        int id_module; bool system_module;

        public AddonReader(int ID)
        {
            AccessManager = new ModulesAccessManager();

            id_module = ID;
            AsyncHelpers.RunSync(() => IsSystemModuleOrNot(ID));
        }

        async Task IsSystemModuleOrNot(int _id)
        {
            InfosModule ModuleAccess = await new ModulesAccessManager().GetModuleViaIDAsync(_id);

            if (ModuleAccess.ModuleSystem)
            {
                system_module = true;
            }
            else
            {
                system_module = false;
            }
        }

        public async Task<string> GetAddonMainJsViaIDAsync()
        {
            StorageFolder folder_module;

            if (system_module)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"),
                    folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }
            StorageFile file_content = await folder_module.GetFileAsync("main.js");

            try
            {
                using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch
            {
                return "";
            }

        }

        public async Task<BitmapImage> GetAddonIconViaIDAsync()
        {
            StorageFolder folder_module;

            if (system_module)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"),
                    folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }

            StorageFile file_content = await folder_module.GetFileAsync("icon.png");

            try
            {
                using (var reader = (FileRandomAccessStream)await file_content.OpenAsync(FileAccessMode.Read))
                {
                    var bitmapImage = new BitmapImage();
                    bitmapImage.SetSource(reader);

                    return bitmapImage;
                }
            }
            catch
            {
                return null;
            }

        }

        public async Task<StackPanel> GetAddonWidgetViaIDAsync(object sceelibs)
        {
            StorageFolder folder_module;
            var widget_content = new StackPanel { Padding = new Thickness(5, 0, 10, 0), Orientation = Orientation.Horizontal, Name = "" + id_module };

            if (system_module)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"),
                    folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(id_module + "", CreationCollisionOption.OpenIfExists);
            }

            StorageFile file_content = await folder_module.GetFileAsync("widget.json");

            /*AddonWidget widgeet = new AddonWidget { Type = WidgetType.Button, FunctionName = "main", PlaceHolderText = "mdrrhooo" };
            AddonWidget widget_b = new AddonWidget { Type = WidgetType.TextBox, FunctionName = "main", PlaceHolderText = "mdrrhooo" };
            List<AddonWidget> widgets = new List<AddonWidget>();
            widgets.Add(widgeet); widgets.Add(widget_b);
            var dataPackage = new DataPackage();
            dataPackage.SetText(JsonConvert.SerializeObject(widgets, Formatting.Indented));
            Clipboard.SetContent(dataPackage);*/


            using (var reader = new StreamReader(await file_content.OpenStreamForReadAsync()))
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

                                    new_button.Name = widget.WidgetName + id_module; new_button.Style = (Style)Application.Current.Resources["Round_Button"];
                                    new_button.Padding = new Thickness(0);
                                    new_button.Width = 25; new_button.Height = 25;
                                    new_button.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets");
                                    new_button.Content = widget.IconButton;
                                    new_button.Foreground = new SolidColorBrush(Colors.White); new_button.Background = new SolidColorBrush(Colors.Transparent);
                                    new_button.Click += ((e, f) =>
                                    {
                                        new AddonExecutor(id_module, sceelibs).ExecutePersonalizedFunction(widget.FunctionName);
                                    });

                                    widget_content.Children.Add(new_button);

                                    break;

                                case WidgetType.TextBox:
                                    var new_textbox = new TextBox();
                                    new_textbox.Margin = new Thickness(5, 0, 5, 0);

                                    new_textbox.Name = widget.WidgetName + id_module; new_textbox.Style = (Style)Application.Current.Resources["RoundTextBox"];
                                    //new_textbox.Padding = new Thickness(0);
                                    new_textbox.Width = 150; new_textbox.Height = 25;
                                    new_textbox.PlaceholderText = widget.PlaceHolderText;
                                    new_textbox.FontSize = 14; new_textbox.Background = new SolidColorBrush(Colors.White); new_textbox.Foreground = new SolidColorBrush(Colors.Black);
                                    new_textbox.KeyDown += ((e, f) =>
                                    {
                                        if (f.Key == Windows.System.VirtualKey.Enter)
                                        {
                                            new AddonExecutor(id_module, sceelibs).ExecutePersonalizedFunction(widget.FunctionName);
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
