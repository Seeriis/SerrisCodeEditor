namespace SCEELibs.Editor.Notifications
{
    public enum ContactTypeSCEE
    {
        GetCodeForTab,
        SetCodeForEditor,
        SetCodeForEditorWithoutUpdate,
        ReloadLanguage
    }

    public sealed class TabSelectedNotification
    {
        public int tabID { get; set; }
        public int tabsListID { get; set; }

        public ContactTypeSCEE contactType { get; set; }
        public string code { get; set; }
        public string typeCode { get; set; }
        public string typeLanguage { get; set; }
        public string monacoModelID { get; set; }

        public int cursorPositionLineNumber { get; set; }
        public int cursorPositionColumn { get; set; }

        public string tabName { get; set; }
    }
}
