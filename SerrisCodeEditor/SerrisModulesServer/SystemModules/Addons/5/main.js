function main() { }

function copy()
{
    try
    {
        var result = sceelibs.editorEngine.injectJSAndReturnResult("window.editor.getModel().getValueInRange(window.editor.getSelection())");

        SCEELibs.Helpers.Clipboard.setContent(result);
        sceelibs.consoleManager.sendConsoleInformationNotification("Text copied to the clipboard !");
    } catch (e)
    {
        sceelibs.consoleManager.sendConsoleErrorNotification(e.message);
    }

}

function cut()
{
    try {
        var result = sceelibs.editorEngine.injectJSAndReturnResult("window.editor.getModel().getValueInRange(window.editor.getSelection())");

        SCEELibs.Helpers.Clipboard.setContent(result);
        sceelibs.editorEngine.injectJSAndReturnResult("var selection = window.editor.getSelection(); var range = new monaco.Range(selection.startLineNumber, selection.startColumn, selection.endLineNumber, selection.endColumn); var id = { major: 1, minor: 1 }; editor.executeEdits('source', [ {identifier: id, range: range, text: '', forceMoveMarkers: true} ]);");
        sceelibs.consoleManager.sendConsoleInformationNotification("Text has been cutted !");
    } catch (e) {
        sceelibs.consoleManager.sendConsoleErrorNotification(e.message);
    }
}

function paste()
{
    try {
        sceelibs.editorEngine.injectJSAndReturnResult("var selection = window.editor.getSelection(); var range = new monaco.Range(selection.startLineNumber, selection.startColumn, selection.endLineNumber, selection.endColumn); var id = { major: 1, minor: 1 }; editor.executeEdits('source', [ {identifier: id, range: range, text: '" + SCEELibs.Helpers.Clipboard.getContent() + "', forceMoveMarkers: true} ]);");
        sceelibs.consoleManager.sendConsoleInformationNotification("Text has been pasted from the clipboard !");
    } catch (e) {
        sceelibs.consoleManager.sendConsoleErrorNotification(e.message);
    }
}
