function main()
{
    try
    {
        //var Manager_b = new SCEELibs.Editor.ConsoleManager();
        //sceelibs.consoleManager.sendConsoleInformationNotification("Résultat: " + u_result);

        //Manager = SerrisModulesServer.Items.Lol;
        //Test = Windows.UI.Popups.MessageDialog("test");

        /*var uri = new Windows.UI.Popups.MessageDialog("Bonjour !");
        uri.title = "Ceci est un titre c:";
        uri.showAsync();*/

        //var Manager = new SCEELibs.Editor.SheetManager();
        //var button = new Windows.UI.Xaml.Controls.Button(); button.content = "coucou !";

        sceelibs.sheetManager.createNewSheet("Bouton test", "HTML/content.html");

        //Manager.deleteModule(1);

        //global = JavaScriptValue.GlobalObject;

       /* var U = 0, m; //Définition des variables

        for (m = 1; U <= 5; m++) //Définition des paramètres de la boucle for ("pour")
        {
            U += 1 / m;
        }

        console.log("U est supérieur ou égal à 5 à partir de " + m); //Affichage du résultat dans la console
        */

        //sceelibs.widgetManager.enableButton("osef", false);
        //sceelibs.consoleManager.sendConsoleInformationNotification("Le bouton 'osef' est " + sceelibs.widgetManager.isButtonEnabled("osef"));
    }
    catch (e)
    {
        console.log(e.message);
    }
}

function textBoxAction()
{
    sceelibs.consoleManager.sendConsoleInformationNotification(sceelibs.widgetManager.getTextBoxContent("osef_b"));
    //sceelibs.widgetManager.setTextBoxContent("osef_b", "Ok !");

    if (sceelibs.widgetManager.isButtonEnabled("osef"))
    {
        sceelibs.widgetManager.enableButton("osef", !sceelibs.widgetManager.isButtonEnabled("osef"));
        sceelibs.widgetManager.openFlyout("osef_b", "HTML/content.html");
    }
    else
    {
        sceelibs.widgetManager.openFlyout("osef_b", "");

    }

}