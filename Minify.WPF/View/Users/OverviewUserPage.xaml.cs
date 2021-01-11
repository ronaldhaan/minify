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

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.DAL.Managers;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for OverviewUserPage.xaml
    /// </summary>
    public partial class OverviewUserPage : Page
    {
        public User User { get; set; }

        private readonly UserController _userController;
        public OverviewUserPage()
        {
            _userController = ControllerManager.Get<UserController>();

            User = _userController.Get(AppData.UserId);

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
                var registerController = ControllerManager.Get<RegisterController>();

                if (!registerController.PasswordEqualsConfirmPassword(tbxPassword.Password, tbxConfirmPassword.Password)
                    || !registerController.IsValidPassword(tbxPassword.Password))
                {
                    tbkErrorMatch.Visibility = Visibility.Visible;
                }
                else
                {
                    User user = _userController.Get(AppData.UserId);
                    user.PassWord = PasswordManager.HashPassword(tbxPassword.Password);
                    if( _userController.Update(user))
                    {
                        // message succeeded
                        MessageBox.Show("Password updated!");
                    }
                }
            }
        }
    }
}
