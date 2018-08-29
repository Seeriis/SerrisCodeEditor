function main()
{
    sceelibs.webBrowserManager.openWebBrowser();
}

function openBrowser()
{
    sceelibs.webBrowserManager.openWebBrowser();
}

function onEditorViewReady() {
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'qwant_button', label: 'Search on Qwant...', contextMenuGroupId: 'searching', contextMenuOrder: 1.5, run: function(ed) { sceelibs.webBrowserManager.openWebBrowserWithURL('https://qwant.com/?b=0&q=' + editor.getModel().getValueInRange(window.editor.getSelection())); } });");
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'webbrowser_button', label: 'Open the web browser', keybindings: [ monaco.KeyMod.Shift | monaco.KeyCode.Tab ], contextMenuGroupId: 'searching', contextMenuOrder: 1.5, run: function(ed) { sceelibs.webBrowserManager.openWebBrowser(); } });");
}