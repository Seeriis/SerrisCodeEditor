using Newtonsoft.Json;
using SerrisModulesServer.Items;
using SerrisModulesServer.Type;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Windows.Storage;

namespace SerrisModulesServer.Manager
{
    public enum PackageVerificationCode
    {
        Passed,
        InfosJsonNotFound,
        MainJsNotFound,
        NoThemeFiles,
        LogoNotFound,
        OldSceClient
    }

    public class ModulesVerifyAssistant
    {
        StorageFile Package;

        public ModulesVerifyAssistant(StorageFile ZipPackage)
        {
            Package = ZipPackage;
        }

        public Task<PackageVerificationCode> VerifyPackageAsync()
        {
            return Task.Run(() => 
            {
                ModuleTypesList type; float MinimalVersion;
                using (ZipArchive zip_content = ZipFile.OpenRead(Package.Path))
                {

                    //Verify "infos.json" and if the file exist, get the content for the others verification !
                    try
                    {
                        ZipArchiveEntry InfosJson = zip_content.GetEntry("infos.json");

                        if (InfosJson == null)
                        {
                            return PackageVerificationCode.InfosJsonNotFound;
                        }
                        else
                        {

                            using (var reader = new StreamReader(InfosJson.Open()))
                            using (JsonReader JsonReader = new JsonTextReader(reader))
                            {
                                InfosModule module = new JsonSerializer().Deserialize<InfosModule>(JsonReader);
                                if (module != null)
                                {
                                    type = module.ModuleType;
                                    MinimalVersion = module.SceMinimalVersionRequired;
                                }
                                else
                                {
                                    return PackageVerificationCode.InfosJsonNotFound;
                                }
                            }

                        }
                    }
                    catch
                    {
                        return PackageVerificationCode.InfosJsonNotFound;
                    }


                    //Verify if the logo exist or not
                    try
                    {
                        if (zip_content.GetEntry("logo.png") == null)
                        {
                            return PackageVerificationCode.LogoNotFound;
                        }
                    }
                    catch
                    {
                        return PackageVerificationCode.LogoNotFound;
                    }


                    switch (type)
                    {
                        case ModuleTypesList.Addon:
                            //Verify if the toolbar icon of the addon exist or not
                            try
                            {
                                if (zip_content.GetEntry("icon.png") == null)
                                {
                                    return PackageVerificationCode.LogoNotFound;
                                }
                            }
                            catch
                            {
                                return PackageVerificationCode.LogoNotFound;
                            }

                            //Verify if the "main.js" exist or not
                            try
                            {
                                if (zip_content.GetEntry("main.js") == null)
                                {
                                    return PackageVerificationCode.MainJsNotFound;
                                }
                            }
                            catch { return PackageVerificationCode.MainJsNotFound; }

                            break;

                        case ModuleTypesList.Theme:
                            //Verify if the "theme.js" or "theme_ace.js" exist or not
                            bool themejs = true, themeacejs = true;

                            try
                            {
                                if (zip_content.GetEntry("theme.json") == null)
                                {
                                    themejs = false;
                                }
                            }
                            catch
                            {
                                themejs = false;
                            }

                            /*try
                            {
                                if (zip_content.GetEntry("theme_ace.js") == null)
                                {
                                    themeacejs = false;
                                }
                            }
                            catch
                            {
                                themeacejs = false;
                            }*/

                            if (!themejs /*&& !themeacejs*/)
                            {
                                return PackageVerificationCode.NoThemeFiles;
                            }

                            break;
                    }


                }

                return PackageVerificationCode.Passed;
            });
        }

        public Task<InfosModule> GetPackageInfosAsync()
        {
            return Task.Run(() => 
            {
                using (ZipArchive zip_content = ZipFile.OpenRead(Package.Path))
                {
                    //Verify "infos.json" and if the file exist, get the content !
                    try
                    {
                        ZipArchiveEntry InfosJson = zip_content.GetEntry("infos.json");

                        if (InfosJson == null)
                        {
                            return null;
                        }
                        else
                        {
                            using (var reader = new StreamReader(InfosJson.Open()))
                            using (JsonReader JsonReader = new JsonTextReader(reader))
                            {
                                InfosModule module = new JsonSerializer().Deserialize<InfosModule>(JsonReader);
                                if (module != null)
                                {
                                    return module;
                                }
                                else
                                {
                                    return null;
                                }
                            }
                        }
                    }
                    catch
                    {
                        return null;
                    }
                }

            });
        }

    }
}
