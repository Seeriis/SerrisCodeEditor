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

    //Shortcut for swap lines
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'swaplines_button', label: 'Swap lines', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_T ], run: function(ed) { var selection = window.editor.getSelection(); var content_a = editor.getModel().getLineContent(editor.getPosition().lineNumber); var content_b = editor.getModel().getLineContent((editor.getPosition().lineNumber) - 1); var range = new monaco.Range(selection.startLineNumber, 0, selection.startLineNumber, (content_a.length + 1)); var range_b = new monaco.Range((selection.startLineNumber - 1), 0, (selection.startLineNumber - 1), (content_b.length + 1)); var id = { major: 1, minor: 1 }; editor.executeEdits('source', [ {identifier: id, range: range, text: content_b, forceMoveMarkers: false} ]); editor.executeEdits('source', [ {identifier: id, range: range_b, text: content_a, forceMoveMarkers: false} ]); } });")

    //Shortcut for duplicate current line
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'duplicatelines_button', label: 'Duplicate current line', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_D ], run: function(ed) { var selection = window.editor.getSelection(); var content_a = editor.getModel().getLineContent(editor.getPosition().lineNumber); var range = new monaco.Range(selection.startLineNumber, (content_a.length + 1), selection.startLineNumber, (content_a.length + 1)); var id = { major: 1, minor: 1 }; editor.executeEdits('source', [ {identifier: id, range: range, text: '\\n' + content_a, forceMoveMarkers: true} ]); } });")

    //Shortcut for opening tabs creator
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'newtabs_button', label: 'Create new tabs', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_N ], contextMenuGroupId: 'creator', contextMenuOrder: 1.5, run: function(ed) { sceelibs.tabsManager.openTabsCreator(); } });");

    //Shortcut for opening new files
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'openingfiles_button', label: 'Open files', keybindings: [ monaco.KeyMod.CtrlCmd | monaco.KeyCode.KEY_O ], contextMenuGroupId: 'creator', contextMenuOrder: 1.5, run: function(ed) { sceelibs.tabsManager.openFilesDialog(); } });");
}