function main()
{
}

function save()
{
    sceelibs.editorEngine.saveCurrentSelectedTab();
    var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID());
    sceelibs.consoleManager.sendConsoleInformationNotification(tab.pathContent);

    /*if (tab.pathContent === "")
    {
        tab.createTabFile();
    }*/

    tab.saveContentToFile();
    sceelibs.consoleManager.sendConsoleInformationNotification('"' + tab.tabName + '" has been saved at ' + tab.pathContent + ' !');
}

function saveas()
{
    sceelibs.consoleManager.sendConsoleInformationNotification("This function is not available on this build of Serris Code Editor :(");
}

function onEditorViewReady()
{
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'save_button', label: 'Save', contextMenuGroupId: 'sce', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_S ], contextMenuOrder: 1.5, run: function(ed) { sceelibs.editorEngine.saveCurrentSelectedTab(); var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID()); tab.saveContentToFile(); sceelibs.consoleManager.sendConsoleInformationNotification('\"' + tab.tabName + '\" has been saved at ' + tab.pathContent + ' !'); return null; } });");
}