function main()
{
    try
    {
        //showPopup("Popup titre", "Ceci est du texte lambda");
        createWindowsNotification("Coucou, je suis l'addon qui envois une notification !");
    }
    catch (e)
    {
        console.log(e.message);
    }
}