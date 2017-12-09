
var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();

function main() { }

function copy()
{
    try
    {
        sceelibs.editorEngine.injectJSAndReturnResult("window.editor.getModel().getValueInRange(window.editor.getSelection())").then(function (result) {
            dataPackage.setText(result);
            Windows.ApplicationModel.DataTransfer.Clipboard.setContent(dataPackage);
            sceelibs.consoleManager.sendConsoleInformationNotification("Text copied to the clipboard !");
        });
    } catch (e)
    {
        sceelibs.consoleManager.sendConsoleErrorNotification(e.message);
    }

}

function cut()
{
    try {
        sceelibs.editorEngine.injectJSAndReturnResult("window.editor.getModel().getValueInRange(window.editor.getSelection())").then(function (result) {
            dataPackage.setText(result);
            Windows.ApplicationModel.DataTransfer.Clipboard.setContent(dataPackage);
            sceelibs.consoleManager.sendConsoleInformationNotification("Text copied to the clipboard !");

            sceelibs.editorEngine.injectJS("editor.trigger('keyboard', 'type', {text: ''});");
        });
    } catch (e) {
        sceelibs.consoleManager.sendConsoleErrorNotification(e.message);
    }
}

function paste()
{

}