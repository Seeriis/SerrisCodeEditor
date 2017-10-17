using SerrisModulesServer.Type.Addon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

                switch(typefunc)
                {
                    case AddonFunction.main:
                        new AddonExecutor(ID, AddonExecutorFuncTypes.main, ref f, ref ff);
                        break;

                    case AddonFunction.onEditorStart:
                        new AddonExecutor(ID, AddonExecutorFuncTypes.onEditorStart, ref f, ref ff);
                        break;

                    case AddonFunction.onEditorViewReady:
                        new AddonExecutor(ID, AddonExecutorFuncTypes.onEditorViewReady, ref f, ref ff);
                        break;
                }
            });
        }

        public async void executeAddonViaIDAndXAMLElements(int ID, AddonFunction typefunc, Flyout flyout, Frame frame)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
            {
                switch (typefunc)
                {
                    case AddonFunction.main:
                        new AddonExecutor(ID, AddonExecutorFuncTypes.main, ref flyout, ref frame);
                        break;

                    case AddonFunction.onEditorStart:
                        new AddonExecutor(ID, AddonExecutorFuncTypes.onEditorStart, ref flyout, ref frame);
                        break;

                    case AddonFunction.onEditorViewReady:
                        new AddonExecutor(ID, AddonExecutorFuncTypes.onEditorViewReady, ref flyout, ref frame);
                        break;
                }
            });
        }
    }
}
