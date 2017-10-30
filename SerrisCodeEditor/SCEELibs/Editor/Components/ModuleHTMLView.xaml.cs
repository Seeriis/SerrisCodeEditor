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
        WebView html_view; bool isLoaded = false; int current_id;

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



        public async void LoadPage(string path, int id)
        {
            if (!isLoaded)
            {
                html_view = new WebView(WebViewExecutionMode.SeparateThread);
                html_view.NavigationStarting += Html_view_NavigationStarting;
                html_view.SetValue(ScrollViewer.HorizontalScrollBarVisibilityProperty, ScrollBarVisibility.Visible);
                MasterGrid.Children.Insert(0, html_view);

                isLoaded = true;
            }
            current_id = id;

            ModulesAccessManager AccessManager = new ModulesAccessManager();

            InfosModule ModuleAccess = await AccessManager.GetModuleViaIDAsync(id);
            StorageFolder folder_module;

            if (ModuleAccess.ModuleSystem)
            {
                StorageFolder folder_content = await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer"),
                    folder_systemmodules = await folder_content.GetFolderAsync("SystemModules");
                folder_module = await folder_systemmodules.CreateFolderAsync(id + "", CreationCollisionOption.OpenIfExists);
            }
            else
            {
                StorageFolder folder_content = await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists);
                folder_module = await folder_content.CreateFolderAsync(id + "", CreationCollisionOption.OpenIfExists);
            }

            StorageFolder _folder_temp = folder_module; StorageFile _file_read = await folder_module.GetFileAsync("main.js"); bool file_found = false; string path_temp = path;

            while (!file_found)
            {
                if (path_temp.Contains(Path.AltDirectorySeparatorChar))
                {
                    _folder_temp = await _folder_temp.GetFolderAsync(path_temp.Split(Path.AltDirectorySeparatorChar).First());
                    path_temp = path_temp.Substring(path_temp.Split(Path.AltDirectorySeparatorChar).First().Length + 1);
                }
                else
                {
                    _file_read = AsyncHelpers.RunSync<StorageFile>(async () => await _folder_temp.GetFileAsync(path_temp));
                    file_found = true;
                    break;
                }
            }

            try
            {
                using (var reader = AsyncHelpers.RunSync<StreamReader>(async () => new StreamReader(await _file_read.OpenStreamForReadAsync())))
                {
                    html_view.NavigateToString(await reader.ReadToEndAsync());
                }
            }
            catch
            {
                Debug.WriteLine("Erreur ! :(");
            }
        }

    }
}
