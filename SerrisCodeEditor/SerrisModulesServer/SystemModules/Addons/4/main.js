function main()
{
}

function save()
{
    var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID());

    sceelibs.editorEngine.saveCurrentSelectedTab();
    tab.saveContentToFile();
    sceelibs.consoleManager.sendConsoleInformationNotification('"' + tab.tabName + '" has been saved at ' + tab.pathContent + ' !');
}

function saveas()
{
    var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID());
    sceelibs.tabsManager.createNewTabInTheCurrentList("Copy_" + tab.tabName, tab.getContent());
}

function onEditorViewReady()
{
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'save_button', label: 'Save', contextMenuGroupId: 'sce', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_S ], contextMenuOrder: 1.5, run: function(ed) { sceelibs.editorEngine.saveCurrentSelectedTab(); var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID()); tab.saveContentToFile(); sceelibs.consoleManager.sendConsoleInformationNotification('\"' + tab.tabName + '\" has been saved at ' + tab.pathContent + ' !'); return null; } });");
}