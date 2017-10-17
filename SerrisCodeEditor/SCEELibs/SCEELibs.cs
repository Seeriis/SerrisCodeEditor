using SCEELibs.Editor;
using SCEELibs.Modules;
using SCEELibs.Modules.Type;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace SCEELibs
{

    [AllowForWeb]
    public sealed class SCEELibs
    {
        /* ===========
         * = MODULES =
         * ===========
         */
        Manager _modulesManager = new Manager();
        public Manager modulesManager { get { return _modulesManager; } }

        Addon _addon = new Addon();
        public Addon addon { get { return _addon; } }

        Theme _theme = new Theme();
        public Theme theme { get { return _theme; } }


        /* ==========
         * = EDITOR =
         * ==========
         */
        SheetManager _sheetManager = new SheetManager();
        public SheetManager sheetManager { get { return _sheetManager; } }
    }
}
