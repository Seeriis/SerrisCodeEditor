
var dataPackage = new Windows.ApplicationModel.DataTransfer.DataPackage();

function main() { }

function copy()
{
    try
    {
        var result = sceelibs.editorEngine.injectJSAndReturnResult("window.editor.getModel().getValueInRange(window.editor.getSelection())");

        dataPackage.setText(result);
        Windows.ApplicationModel.DataTransfer.Clipboard.setContent(dataPackage);
        sceelibs.consoleManager.sendConsoleInformationNotification("Text copied to the clipboard !");
    } catch (e)
    {
        sceelibs.consoleManager.sendConsoleErrorNotification(e.message);
    }

}

function cut()
{
    try {
        sceelibs.consoleManager.sendConsoleInformationNotification("This function is not available on this build of Serris Code Editor :(");

        /*var result = sceelibs.editorEngine.injectJSAndReturnResult("window.editor.getModel().getValueInRange(window.editor.getSelection())");

        dataPackage.setText(result);
        Windows.ApplicationModel.DataTransfer.Clipboard.setContent(dataPackage);
        sceelibs.consoleManager.sendConsoleInformationNotification("Text copied to the clipboard !");
        sceelibs.editorEngine.injectJS("editor.trigger('keyboard', 'type', {text: ''});");*/

    } catch (e) {
        sceelibs.consoleManager.sendConsoleErrorNotification(e.message);
    }
}

function paste()
{
    sceelibs.consoleManager.sendConsoleInformationNotification("This function is not available on this build of Serris Code Editor :(");
}