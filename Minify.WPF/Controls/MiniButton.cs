using Minify.Core;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Minify.WPF.Controls
{
    public class MiniButton : Button
    {
        public MiniButton()
        {
            Background = Utility.GetColorFromString(Core.Utility.DefaultColor);
            Foreground = Utility.GetColorFromString(Core.Utility.DefaultForegroundColor);
            Cursor = Cursors.Hand;
        }
    }
}
