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
        onEditorViewReady,
        whenModuleIsPinned
    }

    public class AddonExecutor
    {

        private ModulesAccessManager AccessManager;
        private ChakraSMS host;
        private int _id; private object _SCEELibs;

        public AddonExecutor(int ID, object SCEELibs)
        {
            AccessManager = new ModulesAccessManager();
            _id = ID; _SCEELibs = SCEELibs;
            //InitializeExecutor(ID, FuncType, SCEELibs);
        }

        public void ExecutePersonalizedFunction(string function_name)
        {
            host = new ChakraSMS();

            host.Chakra.ProjectNamespace("SCEELibs.Editor");
            host.Chakra.ProjectNamespace("SCEELibs.Modules");
            host.Chakra.ProjectNamespace("SCEELibs.Modules.Type");
            host.Chakra.ProjectNamespace("SCEELibs.Tabs");

            IntializeChakraAndExecute(function_name);

        }

        public void ExecuteDefaultFunction(AddonExecutorFuncTypes FuncType)
        {
            host = new ChakraSMS();
            host.Chakra.ProjectNamespace("SCEELibs.Editor");
            host.Chakra.ProjectNamespace("SCEELibs.Modules");
            host.Chakra.ProjectNamespace("SCEELibs.Modules.Type");
            host.Chakra.ProjectNamespace("SCEELibs.Tabs");

            if (FuncType == AddonExecutorFuncTypes.whenModuleIsPinned)
            {
                host.Chakra.ProjectNamespace("SCEELibs.Editor.Components");
                host.Chakra.ProjectObjectToGlobal(_SCEELibs, "sceelibs");

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

                StorageFile file = AsyncHelpers.RunSync<StorageFile>(async () => await folder_module.GetFileAsync("widget.js"));
                try
                {
                    using (var reader = AsyncHelpers.RunSync<StreamReader>(async () => new StreamReader(await file.OpenStreamForReadAsync())))
                    {
                        host.Chakra.RunScript(AsyncHelpers.RunSync<string>(async () => await reader.ReadToEndAsync()));
                    }
                }
                catch
                {
                    Debug.WriteLine("Erreur ! :(");
                }

                host.Chakra.CallFunction("whenModuleIsPinned");
            }
            else
            {
                switch (FuncType)
                {
                    case AddonExecutorFuncTypes.main:
                        IntializeChakraAndExecute("main");
                        break;

                    case AddonExecutorFuncTypes.onEditorStart:
                        IntializeChakraAndExecute("onEditorStart");
                        break;

                    case AddonExecutorFuncTypes.onEditorViewReady:
                        IntializeChakraAndExecute("onEditorViewReady");
                        break;
                }

            }

        }

        private async void IntializeChakraAndExecute(string function_name)
        {
            /*
             * =============================
             * = ADDONS EXECUTOR VARIABLES =
             * =============================
             */


            host.Chakra.ProjectObjectToGlobal(_SCEELibs, "sceelibs");


            /*
             * ===========================
             * = ADDONS EXECUTOR CONTENT =
             * ===========================
             */


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

            host.Chakra.CallFunction(function_name);
        }

    }
}
