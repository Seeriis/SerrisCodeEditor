function main()
{
    sceelibs.sheetManager.createNewSheet("Keyboard shortcuts", "HTML/shortcuts.html");
}

function onEditorViewReady() {

    //Shortcuts for tabs
    for (var i = 1; i <= 10; i++) {
        var keyNmb = 0;

        if (i == 10) {
            keyNmb = 0;
        } else {
            keyNmb = i;
        }

        sceelibs.editorEngine.injectJS("editor.addAction({ id: 'tab-" + i + "', label: 'Focus on the tab " + i + "', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_" + keyNmb + " ], run: function(ed) { sceelibs.tabsManager.focusTabViaPosition(" + i + "); } });");
    }

    //Shortcut for opening tabs creator
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'newtabs_button', label: 'Create new tabs', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_N ], contextMenuGroupId: 'creator', contextMenuOrder: 1.5, run: function(ed) { sceelibs.tabsManager.openTabsCreator(); } });");

    //Shortcut for opening new files
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'openingfiles_button', label: 'Open files', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_O ], contextMenuGroupId: 'creator', contextMenuOrder: 1.5, run: function(ed) { sceelibs.tabsManager.openFilesDialog(); } });");

}