using ControlzEx.Theming;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Media;

namespace Minify.WPF
{
    public class Utility : Core.Utility
    {
        public static string ThemeActive { get; set; }

        public static Theme LightTheme { get; set; }

        public static Theme DarkTheme { get; set; }

        public static Application Application { get; set; }

        public static Brush GetColorFromString(string color)
        {
            return (Brush)new BrushConverter().ConvertFrom(color);
        }

        internal static void SetLightTheme()
        {
            ThemeActive = "Light";
            ThemeManager.Current.ChangeTheme(Application, LightTheme);
        }
        internal static void SetDarkTheme()
        {
            ThemeActive = "Dark";
            ThemeManager.Current.ChangeTheme(Application, DarkTheme);
        }
    }
}
