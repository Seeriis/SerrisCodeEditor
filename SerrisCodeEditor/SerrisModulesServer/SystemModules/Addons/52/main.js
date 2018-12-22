function main()
{
    sceelibs.webBrowserManager.openWebBrowserWithURL("https://www.stackoverflow.com");
}

function stackoverflowSearch(content)
{
    sceelibs.webBrowserManager.openWebBrowserWithURL("https://stackoverflow.com/search?q=" + content);
}

function stackoverflowTextbox() {
    stackoverflowSearch(sceelibs.widgetManager.getTextBoxContent("stackoverflowSearch"));
}

function onEditorViewReady() {
    sceelibs.editorEngine.injectJS("editor.addAction({ id: 'stackoverflow_button', label: 'Search on StackOverflow...', contextMenuGroupId: 'searching', contextMenuOrder: 1.5, run: function(ed) { sceelibs.webBrowserManager.openWebBrowserWithURL('https://stackoverflow.com/search?q=' + editor.getModel().getValueInRange(window.editor.getSelection())); } });");
}