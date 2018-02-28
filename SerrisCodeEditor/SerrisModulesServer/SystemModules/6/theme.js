/*
    ================
    = MONACO THEME =
    ================
*/

monaco.editor.defineTheme('darkTheme', {
    base: 'vs', // can also be vs-dark or hc-black
    inherit: true, // can also be false to completely replace the builtin rules
    rules: [
        { background: '3d3d3d' }
        /*{ token: 'comment', foreground: 'ffa500', fontStyle: 'bold' },
        { token: 'comment.js', foreground: '008800', fontStyle: 'bold' },
        { token: 'comment.css', foreground: '0000ff' }*/
    ],
    colors: {
        'editor.foreground': '#eaeaea',
        'editor.background': '#424242',
        'editorCursor.foreground': '#c6c6c6',
        'editor.lineHighlightBackground': '#424242',
        'editorLineNumber.foreground': '#c6c6c6',
        'editor.selectionBackground': '#88000030',
        'editor.inactiveSelectionBackground': '#88000015'
    }
});