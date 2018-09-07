function main()
{
    openSheetInformation();
}

function openSheetInformation()
{
    sceelibs.sheetManager.createNewSheet("SCE Marne-la-Vallée Informations", "HTML/infos.html");
}

function onEditorStart()
{
    if (sceelibs.preReleaseVersion) {
        if (sceelibs.modulesStorageManager.checkAppSettingAvailable("build_version")) {

            if (sceelibs.modulesStorageManager.readAppSettingContent("build_version") !== sceelibs.versionName) {
                generatePopup();
                sceelibs.modulesStorageManager.writeAppSetting("build_version", sceelibs.versionName);
            }

        }
        else {
            generatePopup();
            sceelibs.modulesStorageManager.writeAppSetting("build_version", sceelibs.versionName);
        }
    }
}

function generatePopup() {
    createWindowsNotification('Welcome to the pre-release "' + sceelibs.versionName + '" of Serris Code Editor !');
}