using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
            ThemeSetting.SelectedValue = Utility.ThemeActive;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems))
            {
                ComboBoxItem item = (ComboBoxItem)e.AddedItems[0];
                if(item.Content.ToString() == "Light")
                {
                    Utility.SetLightTheme();
                }
                else
                {
                    Utility.SetDarkTheme();
                }
            }
        }
    }
}
