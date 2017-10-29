function whenModuleIsPinned()
{
    var widget = new SCEELibs.Editor.Components.Widget(sceelibs.currentID);

    widget.addButtonWithIcon("osef", "", "main");
    widget.addButtonWithIcon("osef_b", "", "main");
    widget.addTextBox("osef_b", "Search...", "main");

    //Nom du bouton dans l'interface - icone du bouton (MDL2 Assets) - Nom de la fonction a executer

    widget.sendWidget();
}