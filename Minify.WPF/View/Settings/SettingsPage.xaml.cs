using Minify.Core.Managers;
using Minify.Core.Models;

using System.Windows.Controls;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly AppData _data;

        public SettingsPage()
        {
            InitializeComponent();

            _data = AppManager.Get<AppData>();

            ThemeSetting.SelectedValue = _data.DefaultTheme == "Light" ? cbxLight : cbxDark;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems))
            {
                ComboBoxItem item = (ComboBoxItem)e.AddedItems[0];
                if (item.Content.ToString() == "Light")
                {
                    Utility.SetLightTheme();
                }
                else
                {
                    Utility.SetDarkTheme();
                }

                string theme = item.Content.ToString();
                if (_data.DefaultTheme != theme)
                {
                    _data.DefaultTheme = theme;
                    var settings = Properties.Settings.Default;
                    settings.DefaultTheme = _data.DefaultTheme;
                    settings.Upgrade();
                    settings.Save();
                }
            }
        }
    }
}
