function main()
{
}

function save()
{
    var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID());

    sceelibs.editorEngine.saveCurrentSelectedTab();
    tab.saveContentToFile();
    if (tab.pathContent !== '') {
        sceelibs.consoleManager.sendConsoleInformationNotification('"' + tab.tabName + '" has been saved at ' + tab.pathContent + ' !');
    }
}

function saveas()
{
    var ids = sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID();

    var tab = sceelibs.tabsManager.getTabViaID(ids);
    var id = sceelibs.tabsManager.createNewTabInTheCurrentList("Copy_" + tab.tabName, sceelibs.editorEngine.injectJSAndReturnResult("editor.getValue()"));

    var tab_b = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.createTabIDs(id, ids.listID));
    tab_b.saveContentToFile();
}

function clone() {
    var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID());
    sceelibs.tabsManager.createNewTabInTheCurrentList("Copy_" + tab.tabName, sceelibs.editorEngine.injectJSAndReturnResult("editor.getValue()"));
}

function onEditorViewReady()
{
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'save_button', label: 'Save', contextMenuGroupId: 'sce', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_S ], contextMenuOrder: 1.5, run: function(ed) { sceelibs.editorEngine.saveCurrentSelectedTab(); var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID()); tab.saveContentToFile(); if (tab.pathContent !== '') { sceelibs.consoleManager.sendConsoleInformationNotification('\"' + tab.tabName + '\" has been saved at ' + tab.pathContent + ' !'); } return null; } });");
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'saveas_button', label: 'Save as', contextMenuGroupId: 'sce', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyMod.Shift | monaco.KeyCode.KEY_S ], contextMenuOrder: 1.5, run: function(ed) { var tab = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID()); var id = sceelibs.tabsManager.createNewTabInTheCurrentList('Copy_' + tab.tabName, editor.getValue()); var tab_b = sceelibs.tabsManager.getTabViaID(sceelibs.tabsManager.createTabIDs(id, sceelibs.tabsManager.getCurrentSelectedTabAndTabsListID().listID)); tab_b.saveContentToFile(); return null; } });");
}