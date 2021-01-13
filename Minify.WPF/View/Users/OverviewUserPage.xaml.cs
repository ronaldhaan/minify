using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.DAL.Managers;

using System.Windows;
using System.Windows.Controls;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for OverviewUserPage.xaml
    /// </summary>
    public partial class OverviewUserPage : Page
    {
        public User User { get; set; }

        private readonly UserController _userController;
        private readonly AppData appData;

        public OverviewUserPage()
        {
            _userController = AppManager.Get<UserController>();
            appData = AppManager.Get<AppData>();

            User = _userController.Get(appData.UserId);

            InitializeComponent();
        }

        private void MiniButton_Click(object sender, RoutedEventArgs e)
        {
            tbkErrorCriteria.Visibility = Visibility.Collapsed;
            tbkErrorMatch.Visibility = Visibility.Collapsed;

            if (string.IsNullOrEmpty(tbxPassword.Password) || string.IsNullOrEmpty(tbxConfirmPassword.Password))
            {
                tbkErrorCriteria.Visibility = Visibility.Visible;
                tbkErrorMatch.Visibility = Visibility.Visible;
            }
            else
            {

                if (tbxPassword.Password != tbxConfirmPassword.Password
                    || !PasswordManager.IsValidPassword(tbxPassword.Password))
                {
                    tbkErrorMatch.Visibility = Visibility.Visible;
                }
                else
                {
                    User user = _userController.Get(appData.UserId);
                    user.PassWord = PasswordManager.HashPassword(tbxPassword.Password);
                    if (_userController.Update(user))
                    {
                        // message succeeded
                        MessageBox.Show("Password updated! :)");
                    }
                }
            }
        }
    }
}
