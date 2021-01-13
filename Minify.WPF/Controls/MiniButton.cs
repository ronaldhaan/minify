
using System.Windows.Controls;
using System.Windows.Input;

namespace Minify.WPF.Controls
{
    public class MiniButton : Button
    {
        public MiniButton()
        {
            Background = Utility.GetColorFromString("#FF821BB2");
            Foreground = Utility.GetColorFromString("#FFFFFF");
            Cursor = Cursors.Hand;
        }
    }
}
