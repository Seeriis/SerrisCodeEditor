function main()
{
    try
    {
        var libs = sceelibs.modulesManager;
        var list_ = libs.getAddonsAvailable(true);
        var list_b = libs.getThemesAvailable(true);

        createWindowsNotification("There are currently " + list_.length + " addon(s) and " + list_b.length + " theme(s) available !");
    }
    catch (e)
    {
        console.log(e.message);
    }
}