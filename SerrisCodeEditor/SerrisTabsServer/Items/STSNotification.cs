namespace SerrisTabsServer.Items
{
    public enum TypeUpdateTab
    {
        TabUpdated,
        TabNewModifications,
        NewTab,
        SelectTab,
        SelectTabViaNumber,
        TabDeleted,
        NewList,
        ListDeleted,
        RefreshCurrentList,

        OpenTabsCreator,
        OpenNewFiles
    }

    public class STSNotification
    {
        public TypeUpdateTab Type { get; set; }
        public TabID ID { get; set; }
        public int TabNumber { get; set; }
    }
}
