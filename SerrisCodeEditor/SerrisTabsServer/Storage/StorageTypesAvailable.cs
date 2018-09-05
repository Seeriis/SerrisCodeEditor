using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources;
using Windows.System.Profile;

namespace SerrisTabsServer.Storage
{
    public static class StorageTypesAvailable
    {
        public static List<StorageTypeDefinition> StorageTypes = new List<StorageTypeDefinition>
        {
            new StorageTypeDefinition { StorageTypeName = new ResourceLoader().GetString("storages-localstorage"), StorageTypeIcon = "", Type = StorageListTypes.LocalStorage, DevicesCompatible = new List<DevicesAvailable> { DevicesAvailable.Desktop, DevicesAvailable.Mobile, DevicesAvailable.Hololens } },
            new StorageTypeDefinition { StorageTypeName = new ResourceLoader().GetString("storages-onedrive"), StorageTypeIcon = "", Type = StorageListTypes.OneDrive, DevicesCompatible = new List<DevicesAvailable> { DevicesAvailable.Desktop, DevicesAvailable.Mobile, DevicesAvailable.Hololens, DevicesAvailable.Xbox } }
        };

        public static List<StorageTypeDefinition> GetStorageTypesAvailable()
        {
            List<StorageTypeDefinition> TypesAvailable = new List<StorageTypeDefinition>();

            //MOBILE
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.Phone.UI.Input.HardwareButtons"))
            {
                foreach(StorageTypeDefinition Def in StorageTypes)
                {
                    if(Def.DevicesCompatible.Contains(DevicesAvailable.Mobile))
                    {
                        TypesAvailable.Add(Def);
                    }
                }

                return TypesAvailable;
            }
            
            //HOLOLENS
            if (AnalyticsInfo.VersionInfo.DeviceFamily == "Windows.Holographic")
            {
                foreach (StorageTypeDefinition Def in StorageTypes)
                {
                    if (Def.DevicesCompatible.Contains(DevicesAvailable.Hololens))
                    {
                        TypesAvailable.Add(Def);
                    }
                }

                return TypesAvailable;
            }

            //PC
            foreach (StorageTypeDefinition Def in StorageTypes)
            {
                if (Def.DevicesCompatible.Contains(DevicesAvailable.Desktop))
                {
                    TypesAvailable.Add(Def);
                }
            }

            return TypesAvailable;
        }
    }

    public class StorageTypeDefinition
    {
        public string StorageTypeName { get; set; }
        public string StorageTypeIcon { get; set; }
        public StorageListTypes Type { get; set; }

        public List<DevicesAvailable> DevicesCompatible { get; set; }
    }

    public enum DevicesAvailable
    {
        Desktop,
        Hololens,
        Mobile,
        Xbox
    }
}
