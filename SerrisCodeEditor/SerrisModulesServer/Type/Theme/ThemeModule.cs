using System;
using System.IO;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace SerrisModulesServer.Type.Theme
{

    public class ThemeModule
    {
        public string BackgroundImagePath { get; set; }

        public Color MainColor { get; set; }
        public Color MainColorFont { get; set; }

        public Color SecondaryColor { get; set; }
        public Color SecondaryColorFont { get; set; }

        public Color ToolbarColor { get; set; }
        public Color ToolbarColorFont { get; set; }

        public Color ToolbarRoundButtonColor { get; set; }
        public Color ToolbarRoundButtonColorFont { get; set; }

        public Color AddonDefaultColor { get; set; }
        public Color AddonDefaultFontColor { get; set; }

        public Color RoundBorderNotificationColor { get; set; }
        public Color RoundNotificationColor { get; set; }
    }

    public class ThemeModuleBrush
    {
        public BitmapImage BackgroundImage { get; set; }

        public SolidColorBrush MainColor { get; set; }
        public SolidColorBrush MainColorFont { get; set; }

        public SolidColorBrush SecondaryColor { get; set; }
        public SolidColorBrush SecondaryColorFont { get; set; }

        public SolidColorBrush ToolbarColor { get; set; }
        public SolidColorBrush ToolbarColorFont { get; set; }

        public SolidColorBrush ToolbarRoundButtonColor { get; set; }
        public SolidColorBrush ToolbarRoundButtonColorFont { get; set; }

        public SolidColorBrush AddonDefaultColor { get; set; }
        public SolidColorBrush AddonDefaultFontColor { get; set; }

        public SolidColorBrush RoundBorderNotificationColor { get; set; }
        public SolidColorBrush RoundNotificationColor { get; set; }

        public void SetBrushsAndImageViaThemeModule(ThemeModule theme_content, string path_module)
        {
            MainColor = new SolidColorBrush(theme_content.MainColor);
            MainColorFont = new SolidColorBrush(theme_content.MainColorFont);

            SecondaryColor = new SolidColorBrush(theme_content.SecondaryColor);
            SecondaryColorFont = new SolidColorBrush(theme_content.SecondaryColorFont);

            ToolbarColor = new SolidColorBrush(theme_content.ToolbarColor);
            ToolbarColorFont = new SolidColorBrush(theme_content.ToolbarColorFont);

            ToolbarColor = new SolidColorBrush(theme_content.ToolbarColor);
            ToolbarColorFont = new SolidColorBrush(theme_content.ToolbarColorFont);

            ToolbarRoundButtonColor = new SolidColorBrush(theme_content.ToolbarRoundButtonColor);
            ToolbarRoundButtonColorFont = new SolidColorBrush(theme_content.ToolbarRoundButtonColorFont);

            ToolbarRoundButtonColor = new SolidColorBrush(theme_content.ToolbarRoundButtonColor);
            ToolbarRoundButtonColorFont = new SolidColorBrush(theme_content.ToolbarRoundButtonColorFont);

            AddonDefaultColor = new SolidColorBrush(theme_content.AddonDefaultColor);
            AddonDefaultFontColor = new SolidColorBrush(theme_content.AddonDefaultFontColor);

            RoundBorderNotificationColor = new SolidColorBrush(theme_content.RoundBorderNotificationColor);
            RoundNotificationColor = new SolidColorBrush(theme_content.RoundNotificationColor);

            BackgroundImage = new BitmapImage(new Uri(Path.Combine(path_module, theme_content.BackgroundImagePath)));
        }
    }

}
