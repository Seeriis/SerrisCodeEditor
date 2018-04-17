using SerrisModulesServer.Items;
using SerrisModulesServer.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace SerrisModulesServer.Type.Templates
{
    public class TemplatesReader : ModuleReader
    {
        public InfosModule ModuleContent;

        public TemplatesReader(int ID) : base(ID)
        {
            ModuleContent = ModulesAccessManager.GetModuleViaID(ID);
        }

        public async Task<List<TemplatesTabInfos>> GetProjectTemplateContentAsync()
        {
            List<TemplatesTabInfos> ProjectTemplateContent = new List<TemplatesTabInfos>();

            StorageFile JSFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(ModuleFolderPath + "main.js"));
            StorageFolder ModuleRootFolder = await JSFile.GetParentAsync();
            StorageFolder ProjectTemplateFolder = await ModuleRootFolder.GetFolderAsync("ProjectTemplate");

            foreach (var Item in await ProjectTemplateFolder.GetItemsAsync())
            {
                object FileType = Item.GetType();

                if(FileType == typeof(StorageFile))
                {
                    ProjectTemplateContent.Add(new TemplatesTabInfos { Encoding = Encoding.UTF8, TabName = Item.Name, FolderTab = false, FileContent = await FileIO.ReadTextAsync((StorageFile)Item) });
                }
                else
                {

                }
            }

            return ProjectTemplateContent;
        }

        public List<TemplatesFileInfos> GetTemplatesFiles()
        {
            return ModuleContent.TemplateFilesInfos;
        }
    }
}
