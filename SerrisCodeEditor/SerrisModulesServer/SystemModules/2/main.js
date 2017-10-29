function main()
{
    try
    {
        //showPopup("Popup titre", "Ceci est du texte lambda");
        var libs = sceelibs.modulesManager;
        var list_ = libs.getAddonsAvailable(true);
        var list_b = libs.getThemesAvailable(true);

        /*var list = [];
        list_.forEach(function (element) {
            list.push(element.ID);
        });*/

        createWindowsNotification("Il y a actuellement " + list_.length + " addon(s) disponible(s) et " + list_b.length + " theme(s) disponible(s) !");
    }
    catch (e)
    {
        console.log(e.message);
    }
}