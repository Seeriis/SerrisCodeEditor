using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisModulesServer.Type.Templates
{
    public class TemplatesTabInfos
    {
        public string TabName { get; set; }
        public Encoding Encoding { get; set; }

        //If file...
        public string FileContent { get; set; }

        //If folder...
        public bool FolderTab { get; set; }
        public List<TemplatesTabInfos> FolderContent { get; set; }
    }
}
