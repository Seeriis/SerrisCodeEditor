function main()
{
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'save_button', label: 'Save', contextMenuGroupId: 'sce', contextMenuOrder: 1.5, run: function(ed) { sceelibs.consoleManager.log('lol'); sceelibs.getWidgetManagerViaID(" + sceelibs.currentID + ").enableButton('save', false); return null; } });");
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