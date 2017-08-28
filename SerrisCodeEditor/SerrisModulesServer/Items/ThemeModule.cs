namespace SerrisModulesServer.Items
{
    public struct RGBA
    {
        public byte R { get; set; }
        public byte G { get; set; }
        public byte B { get; set; }
        public byte A { get; set; }
    }

    public class ThemeModule
    {
        public string BackgroundImagePath { get; set; }

        public RGBA MainColor { get; set; }
        public RGBA MainColorFont { get; set; }

        public RGBA SecondaryColor { get; set; }
        public RGBA SecondaryColorFont { get; set; }

        public RGBA ToolbarColor { get; set; }
        public RGBA ToolbarColorFont { get; set; }

        public RGBA ToolbarRoundButtonColor { get; set; }
        public RGBA ToolbarRoundButtonColorFont { get; set; }

        public RGBA AddonDefaultColor { get; set; }
        public RGBA AddonDefaultFontColor { get; set; }

        public RGBA RoundNotificationColor { get; set; }
    }
}
