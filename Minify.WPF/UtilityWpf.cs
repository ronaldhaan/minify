using ControlzEx.Theming;

using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace Minify.WPF
{
    public class UtilityWpf
    {
        public static Theme LightTheme { get; set; }

        public static Theme DarkTheme { get; set; }

        public static Application Application { get; set; }

        public static Brush GetColorFromString(string color)
        {
            return (Brush)new BrushConverter().ConvertFrom(color);
        }

        internal static void SetLightTheme()
        {
            ThemeManager.Current.ChangeTheme(Application, LightTheme);
        }
        internal static void SetDarkTheme()
        {
            ThemeManager.Current.ChangeTheme(Application, DarkTheme);
        }

        public static bool IsInDesignMode
        {
            get => (bool)DependencyPropertyDescriptor
                    .FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement))
                    .Metadata.DefaultValue;
        }
    }
}
