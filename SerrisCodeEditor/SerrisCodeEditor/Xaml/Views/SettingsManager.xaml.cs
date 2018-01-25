using GalaSoft.MvvmLight.Messaging;
using SerrisCodeEditor.Functions;
using SerrisCodeEditor.Functions.Settings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

namespace SerrisCodeEditor.Xaml.Views
{

    public sealed partial class SettingsManager : Page
    {
        ApplicationDataContainer AppSettings = ApplicationData.Current.LocalSettings;

        public SettingsManager()
        {
            this.InitializeComponent();

            SetTheme();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            int ColumnCount = 0;

            foreach (SettingsMenu menu in DefaultSettings.DefaultSettingsMenuList)
            {
                //BUTTON
                ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

                Grid ButtonMenu = new Grid();
                ButtonMenu.Name = menu.Name;
                ButtonMenu.Background = new SolidColorBrush(Colors.Transparent);
                ButtonMenu.HorizontalAlignment = HorizontalAlignment.Stretch; ButtonMenu.VerticalAlignment = VerticalAlignment.Stretch;
                ButtonMenu.PointerPressed += ((a, f) => 
                {
                    SelectMenu(menu.Name);
                });

                //BUTTON CONTENT
                StackPanel ButtonContent = new StackPanel();
                ButtonContent.HorizontalAlignment = HorizontalAlignment.Center;
                ButtonContent.VerticalAlignment = VerticalAlignment.Center;

                TextBlock ButtonIcon = new TextBlock();
                ButtonIcon.FontSize = 18;
                ButtonIcon.FontFamily = new FontFamily("Segoe MDL2 Assets");
                ButtonIcon.HorizontalAlignment = HorizontalAlignment.Center;
                ButtonIcon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
                ButtonIcon.Text = menu.Icon;
                ButtonContent.Children.Add(ButtonIcon);

                TextBlock ButtonTitle = new TextBlock();
                ButtonTitle.FontSize = 15;
                ButtonTitle.Text = menu.Name;
                ButtonTitle.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
                ButtonContent.Children.Add(ButtonTitle);

                ButtonMenu.Children.Add(ButtonContent);

                ButtonsGrid.Children.Add(ButtonMenu);
                Grid.SetColumn(ButtonMenu, ColumnCount);

                ColumnCount++;

                //SEPARATOR
                if(ColumnCount < (DefaultSettings.DefaultSettingsMenuList.Length * 2) - 1)
                {
                    ButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Pixel) });

                    Rectangle Separator = new Rectangle();
                    Separator.Width = 2;
                    Separator.Margin = new Thickness(0, 15, 0, 15);
                    Separator.RadiusX = 2; Separator.RadiusY = 1;
                    Separator.Fill = GlobalVariables.CurrentTheme.SecondaryColorFont;

                    ButtonsGrid.Children.Add(Separator);
                    Grid.SetColumn(Separator, ColumnCount);

                    ColumnCount++;
                }

            }

            //Default selection
            SelectMenu(DefaultSettings.DefaultSettingsMenuList[0].Name);
        }

        private void SetTheme()
        {
            BackgroundList.Fill = GlobalVariables.CurrentTheme.MainColor;
            ButtonsGrid.Background = GlobalVariables.CurrentTheme.SecondaryColor;
        }

        private void SelectMenu(string MenuName)
        {
            foreach(SettingsMenu menu in DefaultSettings.DefaultSettingsMenuList)
            {
                if(menu.Name == MenuName)
                {
                    //Delete the controls of the ancient menu
                    MenuControls.Children.Clear();

                    //Add the new controls of selected menu
                    foreach(Setting SettingControl in menu.Settings)
                    {
                        switch(SettingControl.Type)
                        {
                            case SettingType.Checkbox:
                                ToggleSwitch Switch = new ToggleSwitch();
                                Switch.Style = (Style)Application.Current.Resources["SwitchControl"];
                                Switch.Margin = new Thickness(0, 20, 0, 0);
                                Switch.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
                                Switch.Background = GlobalVariables.CurrentTheme.MainColor;
                                Switch.Header = SettingControl.Description;

                                if (AppSettings.Values.ContainsKey(SettingControl.VarSaveName))
                                    Switch.IsOn = (bool)AppSettings.Values[SettingControl.VarSaveName];
                                else
                                    Switch.IsOn = (bool)SettingControl.VarSaveDefaultContent;

                                Switch.Toggled += ((e, f) => 
                                {
                                    ToggleSwitch SwitchContent = (ToggleSwitch)e;
                                    AppSettings.Values[SettingControl.VarSaveName] = SwitchContent.IsOn;
                                    Messenger.Default.Send(new SettingsNotification { SettingsUpdatedName = menu.Name });

                                });


                                MenuControls.Children.Add(Switch);
                                break;

                            case SettingType.TextboxText:
                                StackPanel TextBoxControl = new StackPanel();
                                TextBoxControl.Margin = new Thickness(0, 20, 0, 0);

                                TextBlock TitleTextBox = new TextBlock();
                                TitleTextBox.FontSize = 15;
                                TitleTextBox.Text = SettingControl.Description;
                                TitleTextBox.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                                TextBoxControl.Children.Add(TitleTextBox);

                                TextBox TextBox = new TextBox();
                                TextBox.Margin = new Thickness(5, 5, 0, 0);

                                TextBox.Style = (Style)Application.Current.Resources["RoundTextBox"];
                                TextBox.Width = 150; TextBox.Height = 25;
                                //TextBox.PlaceholderText = "";
                                TextBox.FontSize = 14; TextBox.Background = GlobalVariables.CurrentTheme.SecondaryColor; TextBox.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
                                TextBox.KeyDown += ((e, f) =>
                                {
                                    if (f.KeyStatus.RepeatCount == 1)
                                    {
                                        if (f.Key == Windows.System.VirtualKey.Enter)
                                        {
                                            Messenger.Default.Send(new SettingsNotification { SettingsUpdatedName = menu.Name });
                                        }
                                    }
                                });

                                TextBoxControl.Children.Add(TextBox);
                                MenuControls.Children.Add(TextBoxControl);
                                break;

                            case SettingType.Link:
                                StackPanel LinkControl = new StackPanel();
                                LinkControl.Margin = new Thickness(0, 20, 0, 0);

                                TextBlock TitleLink = new TextBlock();
                                TitleLink.FontSize = 15;
                                TitleLink.Text = SettingControl.Description;
                                TitleLink.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                                LinkControl.Children.Add(TitleLink);

                                TextBlock Link = new TextBlock();
                                Link.Margin = new Thickness(5, 5, 0, 0);
                                Link.FontSize = 12;
                                Link.FontStyle = FontStyle.Italic;
                                Link.FontWeight = FontWeights.Light;
                                Link.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                                Underline UnderlineText = new Underline();
                                Run TextLink = new Run();
                                TextLink.Text = (string)SettingControl.Parameter;
                                UnderlineText.Inlines.Add(TextLink);
                                Link.Inlines.Add(UnderlineText);

                                Link.PointerPressed += (async (e, f) => 
                                {
                                    TextBlock LinkContent = (TextBlock)e;
                                    await Windows.System.Launcher.LaunchUriAsync(new Uri(LinkContent.Text));
                                });

                                LinkControl.Children.Add(Link);
                                MenuControls.Children.Add(LinkControl);
                                break;

                            case SettingType.SecondDescription:
                                StackPanel DescriptionControl = new StackPanel();
                                DescriptionControl.Margin = new Thickness(0, 20, 0, 0);

                                TextBlock TitleDescription = new TextBlock();
                                TitleDescription.FontSize = 15;
                                TitleDescription.Text = SettingControl.Description;
                                TitleDescription.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                                DescriptionControl.Children.Add(TitleDescription);

                                TextBlock Description = new TextBlock();
                                Description.Margin = new Thickness(5, 5, 0, 0);
                                Description.FontSize = 13;
                                Description.FontWeight = FontWeights.Light;
                                Description.Text = (string)SettingControl.Parameter;
                                Description.Foreground = GlobalVariables.CurrentTheme.MainColorFont;

                                DescriptionControl.Children.Add(Description);
                                MenuControls.Children.Add(DescriptionControl);
                                break;
                        }
                    }
                }

            }

        }
    }

}
