using SerrisModulesServer.Type.Addon;
using System;
using Windows.Foundation.Metadata;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;

namespace SCEELibs.Modules.Type
{
    public enum AddonFunction
    {
        main,
        onEditorStart,
        onEditorViewReady
    }

    [AllowForWeb]
    public sealed class Addon
    {
        public async void executeAddonViaID(int ID, AddonFunction typefunc)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                var f = new Flyout(); var ff = new Frame();

                switch (typefunc)
                {
                    case AddonFunction.main:
                        new AddonExecutor(ID, new SCEELibs(ID)).ExecuteDefaultFunction(AddonExecutorFuncTypes.main);
                        break;

                    case AddonFunction.onEditorStart:
                        new AddonExecutor(ID, new SCEELibs(ID)).ExecuteDefaultFunction(AddonExecutorFuncTypes.onEditorStart);
                        break;

                    case AddonFunction.onEditorViewReady:
                        new AddonExecutor(ID, new SCEELibs(ID)).ExecuteDefaultFunction(AddonExecutorFuncTypes.onEditorViewReady);
                        break;
                }
            });
        }

    }
}
