using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Minify.WPF
{
    public static class Utility
    {
        public static Brush GetColorFromString(string color)
        {
            return (Brush)new BrushConverter().ConvertFrom(color);
        }

    }
}
