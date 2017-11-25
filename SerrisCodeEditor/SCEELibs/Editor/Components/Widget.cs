using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Notifications;
using SerrisModulesServer.Type.Addon;
using Windows.Foundation.Metadata;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace SCEELibs.Editor.Components
{
    [AllowForWeb]
    public sealed class Widget
    {
        private StackPanel widget_content = new StackPanel();
        private int id;

        public Widget(int ID)
        {
            widget_content.Padding = new Thickness(5, 0, 5, 0);
            widget_content.Orientation = Orientation.Horizontal;
            widget_content.Name = "" + ID;

            id = ID;
        }

        /* =============
         * = FUNCTIONS =
         * =============
         */

        public void addButtonWithIcon(string button_name, string icon_mdl2, string function_name)
        {
            var new_button = new Button();
            new_button.Margin = new Thickness(5, 0, 5, 0);

            new_button.Name = button_name; new_button.Style = (Style)Application.Current.Resources["Round_Button"];
            new_button.Padding = new Thickness(0);
            new_button.Width = 25; new_button.Height = 25;
            new_button.FontFamily = new Windows.UI.Xaml.Media.FontFamily("Segoe MDL2 Assets");
            new_button.Content = icon_mdl2;
            new_button.Foreground = new SolidColorBrush(Colors.White); new_button.Background = new SolidColorBrush(Colors.Transparent);
            new_button.Click += ((e, f) =>
            {
                new AddonExecutor(id, new SCEELibs(id)).ExecutePersonalizedFunction(function_name);
            });

            widget_content.Children.Add(new_button);
        }

        public void addTextBox(string textbox_name, string placeholder_text, string function_name)
        {
            var new_textbox = new TextBox();
            new_textbox.Margin = new Thickness(5, 0, 5, 0);

            new_textbox.Name = textbox_name; new_textbox.Style = (Style)Application.Current.Resources["RoundTextBox"];
            //new_textbox.Padding = new Thickness(0);
            new_textbox.Width = 150; new_textbox.Height = 25;
            new_textbox.PlaceholderText = placeholder_text;
            new_textbox.FontSize = 14; new_textbox.Background = new SolidColorBrush(Colors.White); new_textbox.Foreground = new SolidColorBrush(Colors.Black);
            new_textbox.KeyDown += ((e, f) =>
            {
                if (f.Key == Windows.System.VirtualKey.Enter)
                {
                    new AddonExecutor(id, new SCEELibs(id)).ExecutePersonalizedFunction(function_name);
                }
            });

            widget_content.Children.Add(new_textbox);
        }


        public void sendWidget()
        {
            //Messenger.Default.Send(new ToolbarNotification { id = id, widget = widget_content });
        }
    }
}
