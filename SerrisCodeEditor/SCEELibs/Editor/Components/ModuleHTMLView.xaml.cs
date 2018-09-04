using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace SCEELibs.Editor.Components
{
    public sealed partial class ModuleHTMLView : UserControl
    {
        WebView html_view; bool isLoaded = false; string current_id;

        public ModuleHTMLView()
        {
            this.InitializeComponent();
        }

        private void HTMLView_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Html_view_NavigationStarting(WebView sender, WebViewNavigationStartingEventArgs args)
        {
            html_view.AddWebAllowedObject("sceelibs", new SCEELibs(current_id));
        }



        /* =============
         * = FUNCTIONS =
         * =============
         */



        public void LoadPage(string path, string id)
        {
            if (!isLoaded)
            {
                html_view = new WebView(WebViewExecutionMode.SeparateThread);
                html_view.NavigationStarting += Html_view_NavigationStarting;
                html_view.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Visible);
                MasterGrid.Children.Insert(0, html_view);

                isLoaded = true;
            }

            if(!string.IsNullOrEmpty(path))
            {
                current_id = id;

                InfosModule ModuleAccess = ModulesAccessManager.GetModuleViaID(id);

                if (ModuleAccess.ModuleSystem)
                {
                    html_view.Navigate(new Uri("ms-appx-web:///SerrisModulesServer/SystemModules/Addons/" + id + "/" + path));
                }
                else
                {
                    html_view.Navigate(new Uri("ms-appdata:///local/modules/" + id + "/" + path));
                    Debug.WriteLine("ms-appdata:///local/modules/" + id + "/" + path);
                }

            }

        }

    }
}
