namespace SerrisTabsServer.Items
{
    public enum TypeUpdateTab
    {
        TabUpdated,
        TabNewModifications,
        NewTab,
        SelectTab,
        TabDeleted,
        NewList,
        ListDeleted
    }

    public class STSNotification
    {
        public TypeUpdateTab Type { get; set; }
        public TabID ID { get; set; }
    }
}
