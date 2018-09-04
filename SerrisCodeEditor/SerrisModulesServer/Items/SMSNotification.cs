namespace SerrisModulesServer.Items
{
    public enum TypeUpdateModule
    {
        UpdateModule,
        NewModule,
        ModuleDeleted,
        CurrentThemeUpdated,
        NewTheme
    }

    public class SMSNotification
    {
        public TypeUpdateModule Type { get; set; }
        public string ID { get; set; }
    }
}
