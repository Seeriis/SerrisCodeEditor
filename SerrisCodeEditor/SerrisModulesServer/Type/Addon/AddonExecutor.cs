using ChakraBridge;
using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace SerrisModulesServer.Type.Addon
{
    public enum AddonExecutorFuncTypes
    {
        main,
        onEditorStart,
        onEditorViewReady
    }

    public class AddonExecutor
    {

        private ModulesAccessManager AccessManager;
        private ChakraSMS host;

        Flyout current_flyout; Frame current_frame;

        public AddonExecutor(int ID, AddonExecutorFuncTypes FuncType, ref Flyout flyout, ref Frame big_frame)
        {
            AccessManager = new ModulesAccessManager();
            current_flyout = flyout; current_frame = big_frame;
            InitializeExecutor(ID, FuncType);
        }

        async void InitializeExecutor(int _id, AddonExecutorFuncTypes FuncType)
        {
            host = new ChakraSMS();


            /*
             * =============================
             * = ADDONS EXECUTOR VARIABLES =
             * =============================
             */
            host.Chakra.ProjectObjectToGlobal(current_flyout, "FlyoutView");
            host.Chakra.ProjectObjectToGlobal(current_frame, "BigView");
            host.Chakra.ProjectObjectToGlobal(_id, "currentID");


            InfosModule ModuleAccess = AsyncHelpers.RunSync<InfosModule>(async () => await AccessManager.GetModuleViaIDAsync(_id));
            StorageFolder folder_module;

            if (ModuleAccess.ModuleSystem)
            {
                StorageFolder folder_content = AsyncHelpers.RunSync<StorageFolder>(async () => await Package.Current.InstalledLocation.GetFolderAsync("SerrisModulesServer")),
                    folder_systemmodules = AsyncHelpers.RunSync<StorageFolder>(async () => await folder_content.GetFolderAsync("SystemModules"));
                folder_module = AsyncHelpers.RunSync<StorageFolder>(async () => await folder_systemmodules.CreateFolderAsync(_id + "", CreationCollisionOption.OpenIfExists));
            }
            else
            {
                StorageFolder folder_content = AsyncHelpers.RunSync<StorageFolder>(async () => await ApplicationData.Current.LocalFolder.CreateFolderAsync("modules", CreationCollisionOption.OpenIfExists));
                folder_module = AsyncHelpers.RunSync<StorageFolder>(async () => await folder_content.CreateFolderAsync(_id + "", CreationCollisionOption.OpenIfExists));
            }

            foreach (string path in ModuleAccess.JSFilesPathList)
            {
                StorageFolder _folder_temp = folder_module; StorageFile _file_read = await folder_module.GetFileAsync("main.js"); bool file_found = false; string path_temp = path;

                while (!file_found)
                {
                    if (path_temp.Contains(Path.AltDirectorySeparatorChar))
                    {
                        //Debug.WriteLine(path_temp.Split(Path.AltDirectorySeparatorChar).First());
                        _folder_temp = AsyncHelpers.RunSync<StorageFolder>(async () => await _folder_temp.GetFolderAsync(path_temp.Split(Path.AltDirectorySeparatorChar).First()));
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
                        host.Chakra.RunScript(AsyncHelpers.RunSync<string>(async () => await reader.ReadToEndAsync()));
                    }
                }
                catch
                {
                    Debug.WriteLine("Erreur ! :(");
                }

            }

            StorageFile main_js = AsyncHelpers.RunSync<StorageFile>(async () => await folder_module.GetFileAsync("main.js"));
            try
            {
                string code = AsyncHelpers.RunSync<string>(async () => await FileIO.ReadTextAsync(main_js));
                host.Chakra.RunScript(code);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            /*
             * =============================
             * = ADDONS EXECUTOR FUNCTIONS =
             * =============================
             */

            switch (FuncType)
            {
                case AddonExecutorFuncTypes.main:
                    host.Chakra.CallFunction("main");
                    break;

                case AddonExecutorFuncTypes.onEditorStart:
                    host.Chakra.CallFunction("onEditorStart");
                    break;

                case AddonExecutorFuncTypes.onEditorViewReady:
                    host.Chakra.CallFunction("onEditorViewReady");
                    break;
            }
        }

    }
}
