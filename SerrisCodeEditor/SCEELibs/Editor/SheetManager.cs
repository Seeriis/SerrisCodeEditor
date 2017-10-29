using GalaSoft.MvvmLight.Messaging;
using SCEELibs.Editor.Components;
using SCEELibs.Editor.Notifications;
using SerrisModulesServer.Type.Addon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace SCEELibs.Editor
{
    [AllowForWeb]
    public sealed class SheetManager
    {
        int id;

        public SheetManager(int ID)
        {
            id = ID;
        }


        public async void createNewSheet(string sheetName, string pathHTMLPage)
        {
            ModuleHTMLView view = new ModuleHTMLView();
            view.LoadPage(pathHTMLPage, id);
            Messenger.Default.Send(new ModuleSheetNotification { id = id, sheetName = sheetName, type = ModuleSheetNotificationType.NewSheet, sheetContent = view, sheetIcon = await new AddonReader(id).GetAddonIconViaIDAsync(), sheetSystem = false });
        }

        public void closeSheet(int id)
        {
            Messenger.Default.Send(new ModuleSheetNotification { id = id, type = ModuleSheetNotificationType.RemoveSheet, sheetSystem = false });
        }

    }
}
