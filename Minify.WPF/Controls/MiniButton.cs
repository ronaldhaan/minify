
using System.Windows.Controls;
using System.Windows.Input;

namespace Minify.WPF.Controls
{
    public class MiniButton : Button
    {
        public MiniButton()
        {
            Background = UtilityWpf.GetColorFromString("#FF821BB2");
            Foreground = UtilityWpf.GetColorFromString("#FFFFFF");
            Cursor = Cursors.Hand;
        }
    }
}
