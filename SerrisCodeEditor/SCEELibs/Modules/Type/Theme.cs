using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;

namespace SCEELibs.Modules.Type
{
    public sealed class Theme
    {

        public async void setUITheme(int ID)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                await new ModulesWriteManager().SetCurrentThemeIDAsync(ID);
            });

        }

        public async void setMonacoEditorTheme(int ID)
        {

            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            async () =>
            {
                await new ModulesWriteManager().SetCurrentAceEditoThemeIDAsync(ID);
            });

        }

    }
}
