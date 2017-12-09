function main()
{
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'save_button', label: 'Save', contextMenuGroupId: 'sce', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_S ], contextMenuOrder: 1.5, run: function(ed) { sceelibs.editorEngine.saveCurrentSelectedTab(); sceelibs.consoleManager.sendConsoleInformationNotification('Current tab has been saved !'); /*sceelibs.getWidgetManagerViaID(" + sceelibs.currentID + ").enableButton('save', false);*/ return null; } });");
}

function save()
{
    sceelibs.editorEngine.saveCurrentSelectedTab();
    //sceelibs.widgetManager.enableButton("save", false);
    sceelibs.consoleManager.sendConsoleInformationNotification("Current tab has been saved !");
}

function saveas()
{
    sceelibs.editorInjection.injectJS("editor.trigger('whatever...', 'redo')");
}