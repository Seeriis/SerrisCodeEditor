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
    if (sceelibs.modulesStorageManager.checkAppSettingAvailable("build_version")) {

        if (sceelibs.modulesStorageManager.readAppSettingContent("build_version") != sceelibs.versionName) {
            generatePopup();
            sceelibs.modulesStorageManager.writeAppSetting("build_version", sceelibs.versionName);
        }

    }
    else {
        generatePopup();
        sceelibs.modulesStorageManager.writeAppSetting("build_version", sceelibs.versionName);
    }
}

function generatePopup() {
    createWindowsNotification('Welcome to the version "' + sceelibs.versionName + '" of Serris Code Editor ! Of course, the editor is still in work in progress !');
}