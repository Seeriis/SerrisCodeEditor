using GalaSoft.MvvmLight.Messaging;
using Microsoft.Toolkit.Uwp.Helpers;
using SCEELibs.Editor.Notifications;
using SerrisCodeEditor.Functions;
using SerrisModulesServer.Type.Addon;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SerrisCodeEditor.Xaml.Components
{
    public class ConsoleNotificationContent
    {
        public string notifIcon { get; set; }
        public string notifDate { get { return notifContent.date.ToString("HH:mm:ss"); } }
        public ConsoleNotification notifContent { get; set; }
        public SolidColorBrush foreground { get => GlobalVariables.CurrentTheme.SecondaryColorFont; }
    }

    public sealed partial class Console : UserControl
    {
        /* =============
         * = VARIABLES =
         * =============
         */

        List<ConsoleNotificationContent> errors_list = new List<ConsoleNotificationContent>(), informations_list = new List<ConsoleNotificationContent>(), results_list = new List<ConsoleNotificationContent>(), warnings_list = new List<ConsoleNotificationContent>();
        public ObservableCollection<ConsoleNotificationContent> CurrentNotifications = new ObservableCollection<ConsoleNotificationContent>();
        List<string> commands_list = new List<string>();
        private ChakraSMS executor;

        int commands_list_index = -1;
        bool isFlyoutOpened = false, disableOpening = false;
        bool ShowErrors = true, ShowInformations = true, ShowResults = true, ShowWarnings = true;



        public Console()
        {
            this.InitializeComponent();
            CurrentNotifications.CollectionChanged += CurrentNotifications_CollectionChanged;
            InitializeChakra();
        }

        private void InitializeChakra()
        {
            executor = new ChakraSMS();
            executor.Chakra.ProjectObjectToGlobal(new SCEELibs.Editor.ConsoleManager(), "console");
            executor.Chakra.ProjectObjectToGlobal(new SCEELibs.SCEELibs(), "sceelibs");
        }

        private void ConsoleUI_Loaded(object sender, RoutedEventArgs e)
        {
            SetMessenger(); SetTheme();

            if(GlobalVariables.CurrentDevice == SCEELibs.Editor.CurrentDevice.WindowsMobile)
            {
                SymbolOpened.Visibility = Visibility.Collapsed;
                disableOpening = true;
            }
        }

        private void ConsoleUI_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            if (!isFlyoutOpened && !disableOpening) { OpenConsole(); }
        }

        private void ConsoleUI_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            if (isFlyoutOpened) { CloseConsole(); }
        }



        /* =============
         * = FUNCTIONS =
         * =============
         */



        private void SetMessenger()
        {
            Messenger.Default.Register<ConsoleNotification>(this, async (notification) =>
            {
                await DispatcherHelper.ExecuteOnUIThreadAsync(() => 
                {
                    try
                    {

                        switch (notification.typeNotification)
                        {
                            case ConsoleTypeNotification.Error:
                                errors_list.Add(new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });

                                if (ShowErrors)
                                    CurrentNotifications.Insert(0, new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });
                                break;

                            case ConsoleTypeNotification.Information:
                                informations_list.Add(new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });

                                if (ShowInformations)
                                    CurrentNotifications.Insert(0, new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });
                                    
                                break;

                            case ConsoleTypeNotification.Result:
                                results_list.Add(new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });

                                if (ShowResults)
                                    CurrentNotifications.Insert(0, new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });

                                break;

                            case ConsoleTypeNotification.Warning:
                                warnings_list.Add(new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });

                                if (ShowWarnings)
                                    CurrentNotifications.Insert(0, new ConsoleNotificationContent { notifContent = notification, notifIcon = "" });

                                break;
                        }

                        UpdateNotifsNumber();
                    }
                    catch { }

                });
            });

            Messenger.Default.Register<EditorViewNotification>(this, (notification_ui) =>
            {
                try
                {
                    SetTheme();
                }
                catch { }
            });

        }

        private void SetTheme()
        {
            MasterGrid.Background = GlobalVariables.CurrentTheme.SecondaryColor;
            MasterGrid.BorderBrush = GlobalVariables.CurrentTheme.SecondaryColorFont;
            ConsoleMoreInfosViewer.Background = GlobalVariables.CurrentTheme.SecondaryColor;

            LastNotifInfos_Icon.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            LastNotifInfos_Text.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            SymbolOpened.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            Command_box.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            ErrorsNumber.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            InformationsNumber.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            ResultsNumber.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;
            WarningsNumber.Foreground = GlobalVariables.CurrentTheme.SecondaryColorFont;

            ErrorsStackpanel.BorderBrush = GlobalVariables.CurrentTheme.MainColor;
            InformationsStackPanel.BorderBrush = GlobalVariables.CurrentTheme.MainColor;
            ResultsStackPanel.BorderBrush = GlobalVariables.CurrentTheme.MainColor;
            WarningsStackPanel.BorderBrush = GlobalVariables.CurrentTheme.MainColor;

            ClearButton.BorderBrush = GlobalVariables.CurrentTheme.MainColor;
            ClearButton.Foreground = GlobalVariables.CurrentTheme.MainColor;

            if (ShowInformations)
            {
                InformationsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                InformationsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                InformationsButton.Background = new SolidColorBrush(Colors.Transparent);
                InformationsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            if (ShowResults)
            {
                ResultsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                ResultsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                ResultsButton.Background = new SolidColorBrush(Colors.Transparent);
                ResultsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            if (ShowWarnings)
            {
                WarningsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                WarningsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                WarningsButton.Background = new SolidColorBrush(Colors.Transparent);
                WarningsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            if (ShowErrors)
            {
                ErrorsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                ErrorsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                ErrorsButton.Background = new SolidColorBrush(Colors.Transparent);
                ErrorsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            RefreshNotificationsList();
        }

        private void CurrentNotifications_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if(CurrentNotifications.Count > 0)
            {
                LastNotifInfos_Icon.Text = CurrentNotifications[0].notifIcon;
                LastNotifInfos_Text.Text = "[" + CurrentNotifications[0].notifContent.date.ToString("HH:mm:ss") + "] " + CurrentNotifications[0].notifContent.content;
            }
        }

        private void Command_box_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            switch(e.Key)
            {
                case VirtualKey.Enter:
                    try
                    {
                        executor.Chakra.RunScript(Command_box.Text);
                    }
                    catch { }

                    commands_list.Add(Command_box.Text);
                    commands_list_index = -1; Command_box.Text = "";
                    break;

                case VirtualKey.Down:
                    if(commands_list_index < 0)
                    {
                        commands_list_index = commands_list.Count;
                    }
                    commands_list_index--;

                    if (commands_list_index >= 0)
                        Command_box.Text = commands_list[commands_list_index];

                    break;

                case VirtualKey.Up:
                    if (commands_list_index + 1 <= commands_list.Count - 1)
                    {
                        commands_list_index++;

                        if (commands_list_index >= 0)
                            Command_box.Text = commands_list[commands_list_index];
                    }
                    else
                    {
                        Command_box.Text = ""; commands_list_index = -1;
                    }
                    break;
            }
        }

        private void InformationsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowInformations = !ShowInformations;

            if (ShowInformations)
            {
                InformationsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                InformationsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                InformationsButton.Background = new SolidColorBrush(Colors.Transparent);
                InformationsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            RefreshNotificationsList();
        }

        private void ResultsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowResults = !ShowResults;

            if (ShowResults)
            {
                ResultsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                ResultsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                ResultsButton.Background = new SolidColorBrush(Colors.Transparent);
                ResultsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            RefreshNotificationsList();
        }

        private void WarningsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowWarnings = !ShowWarnings;

            if (ShowWarnings)
            {
                WarningsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                WarningsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                WarningsButton.Background = new SolidColorBrush(Colors.Transparent);
                WarningsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            RefreshNotificationsList();
        }

        private void ErrorsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowErrors = !ShowErrors;

            if (ShowErrors)
            {
                ErrorsButton.Background = GlobalVariables.CurrentTheme.MainColor;
                ErrorsButton.Foreground = GlobalVariables.CurrentTheme.MainColorFont;
            }
            else
            {
                ErrorsButton.Background = new SolidColorBrush(Colors.Transparent);
                ErrorsButton.Foreground = GlobalVariables.CurrentTheme.MainColor;
            }

            RefreshNotificationsList();
        }

        private void MasterGrid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Hand, 1);

            if(!isFlyoutOpened)
                MasterGrid.BorderThickness = new Thickness(3);
        }

        private void MasterGrid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            Window.Current.CoreWindow.PointerCursor = new CoreCursor(CoreCursorType.Arrow, 1);

            if (!isFlyoutOpened)
                MasterGrid.BorderThickness = new Thickness(1);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            if (ShowErrors)
                errors_list.Clear();

            if (ShowInformations)
                informations_list.Clear();

            if (ShowResults)
                results_list.Clear();

            if (ShowWarnings)
                warnings_list.Clear();

            RefreshNotificationsList();
            UpdateNotifsNumber();
        }

        private void CurrentListNotifications_Loaded(object sender, RoutedEventArgs e)
        {
            CurrentListNotifications.SetBinding(ListView.ItemsSourceProperty, new Binding { Source = CurrentNotifications });
        }

        private void CloseConsole()
        {
            LastNotifInfos.Visibility = Visibility.Visible; SymbolOpened.Text = "";
            ConsoleMoreInfosViewer.Visibility = Visibility.Collapsed; Command_box.Visibility = Visibility.Collapsed;
            MasterGrid.CornerRadius = new CornerRadius(0, 15, 15, 0);
            MasterGrid.BorderThickness = new Thickness(1);
            isFlyoutOpened = false;
        }

        private void OpenConsole()
        {
            LastNotifInfos.Visibility = Visibility.Collapsed; SymbolOpened.Text = "";
            ConsoleMoreInfosViewer.Visibility = Visibility.Visible; Command_box.Visibility = Visibility.Visible;
            MasterGrid.CornerRadius = new CornerRadius(0, 15, 0, 0);
            MasterGrid.BorderThickness = new Thickness(0);
            isFlyoutOpened = true;

            UpdateNotifsNumber();
        }

        private void UpdateNotifsNumber()
        {
            ErrorsNumber.Text = "" + errors_list.Count;
            InformationsNumber.Text = "" + informations_list.Count;
            ResultsNumber.Text = "" + results_list.Count;
            WarningsNumber.Text = "" + warnings_list.Count;
        }

        private void RefreshNotificationsList()
        {
            CurrentNotifications.Clear();
            List<ConsoleNotificationContent> temp_list = new List<ConsoleNotificationContent>();
            
            if(ShowErrors)
                foreach(var element in errors_list) { temp_list.Add(element); }

            if (ShowInformations)
                foreach (var element in informations_list) { temp_list.Add(element); }

            if (ShowResults)
                foreach (var element in results_list) { temp_list.Add(element); }

            if (ShowWarnings)
                foreach (var element in warnings_list) { temp_list.Add(element); }

            temp_list.Sort((a, b) => b.notifDate.CompareTo(a.notifDate));
            foreach (var element in temp_list) { CurrentNotifications.Add(element); }
        }

    }
}
