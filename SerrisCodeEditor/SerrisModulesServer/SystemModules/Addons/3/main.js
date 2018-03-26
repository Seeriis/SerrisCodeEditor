function main()
{ }

function undo()
{
    sceelibs.editorEngine.injectJS("editor.trigger('whatever...', 'undo')");
}

function redo()
{
    sceelibs.editorEngine.injectJS("editor.trigger('whatever...', 'redo')");
}