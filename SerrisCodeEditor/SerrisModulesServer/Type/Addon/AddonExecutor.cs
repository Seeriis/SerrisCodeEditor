using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.Storage;

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

        private ChakraSMS host;
        private int _id; private object _SCEELibs;

        public AddonExecutor(int ID, object SCEELibs)
        {
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

            IntializeChakraAndExecute(FuncType.ToString());


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


            InfosModule ModuleAccess = ModulesAccessManager.GetModuleViaID(_id);
            StorageFile MainFile = AsyncHelpers.RunSync(async () => await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModulesAccessManager.GetModuleFolderPath(ModuleAccess.ID, ModuleAccess.ModuleSystem) + "main.js")));
            
            foreach (string Path in ModuleAccess.JSFilesPathList)
            {
                try
                {
                    StorageFile FileFromPath = AsyncHelpers.RunSync(async () => await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModulesAccessManager.GetModuleFolderPath(ModuleAccess.ID, ModuleAccess.ModuleSystem) + Path)));
                    host.Chakra.RunScript(AsyncHelpers.RunSync(async () => await FileIO.ReadTextAsync(FileFromPath)));
                }
                catch
                {
                    Debug.WriteLine("Erreur ! :(");
                }

            }

            try
            {
                string code = AsyncHelpers.RunSync(async () => await FileIO.ReadTextAsync(MainFile));
                host.Chakra.RunScript(code);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            try
            {
                host.Chakra.CallFunction(function_name);
            }
            catch { }
        }

    }
}
