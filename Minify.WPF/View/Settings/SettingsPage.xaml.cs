using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Models;

using System.Windows.Controls;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        private readonly UserController controller;
        private readonly AppData data;


        public SettingsPage()
        {
            InitializeComponent();

            controller = AppManager.Get<UserController>();
            data = AppManager.Get<AppData>();

            var user = controller.Get(data.UserId);

            ThemeSetting.SelectedValue = user.DefaultTheme == DefaultTheme.Light ? cbxLight : cbxDark;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is ComboBoxItem item)
            {
                DefaultTheme theme;
                if (item.Content.ToString() == "Light")
                {
                    UtilityWpf.SetLightTheme();
                    theme = DefaultTheme.Light;
                }
                else
                {
                    UtilityWpf.SetDarkTheme();
                    theme = DefaultTheme.Dark;
                }

                var user = controller.Get(data.UserId);
                user.DefaultTheme = theme;
                controller.Update(user);
            }
        }
    }
}
