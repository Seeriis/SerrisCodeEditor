function main()
{
    sceelibs.editorEngine.injectJS("sceelibs.consoleManager.log('lol');");
    //sceelibs.consoleManager.log("lol");
}

function save()
{
    sceelibs.editorEngine.saveCurrentSelectedTab();
    sceelibs.widgetManager.enableButton("save", false);
    sceelibs.consoleManager.sendConsoleInformationNotification("Current tab has been saved !");

}

function saveas()
{
    sceelibs.editorInjection.injectJS("editor.trigger('whatever...', 'redo')");
}