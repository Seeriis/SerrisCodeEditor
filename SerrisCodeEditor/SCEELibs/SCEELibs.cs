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
        public SCEELibs(int id)
        {
            _currentID = id;

            _sheetManager = new SheetManager(id);
            _widgetManager = new WidgetManager(id);
        }

        public SCEELibs() { }

        int _currentID = -1;
        public int currentID { get { return _currentID; } } 


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


        /* ========
         * = TABS =
         * ========
         */
        Tabs.Manager _tabsManager = new Tabs.Manager();
        public Tabs.Manager tabsManager { get => _tabsManager; }


        /* ==========
         * = EDITOR =
         * ==========
         */
        SheetManager _sheetManager;
        public SheetManager sheetManager { get { return _sheetManager; } }

        ConsoleManager _consoleManager = new ConsoleManager();
        public ConsoleManager consoleManager { get { return _consoleManager; } }

        WidgetManager _widgetManager;
        public WidgetManager widgetManager { get => _widgetManager; }

        Editor.EditorEngine _editorEngine = new Editor.EditorEngine();
        public EditorEngine editorEngine { get => _editorEngine; }



        /* ===============================================
         * = FUNCTIONS FOR WEBVIEW (SCEELIBS WITHOUT ID) =
         * ===============================================
         */
        public WidgetManager getWidgetManagerViaID(int id)
        {
            return new WidgetManager(id);
        }

        public SheetManager getSheetManagerViaID(int id)
        {
            return new SheetManager(id);
        }
    }
}
