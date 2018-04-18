using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SerrisModulesServer.Type.Templates
{
    public class TemplatesFileInfos
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string TemplateFileModulePath { get; set; }
        public string SuggestedTemplateName { get; set; }

        public string Type { get; set; }
        public string Content { get; set; }
    }
}
