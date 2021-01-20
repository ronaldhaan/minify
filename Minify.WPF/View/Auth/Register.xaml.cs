using Castle.Core.Internal;

using MahApps.Metro.Controls;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.DAL.Entities;
using Minify.DAL.Managers;

using System.Windows;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : MetroWindow
    {
        private readonly UserController _userController;

        public Register()
        {
            InitializeComponent();
            _userController = AppManager.Get<UserController>();
        }

        private void BtnRegister_Click(object sender, RoutedEventArgs e)
        {
            //set error messages on hidden
            errorUserName.Visibility = Visibility.Collapsed;
            errorEmail.Visibility = Visibility.Collapsed;
            errorFirstname.Visibility = Visibility.Collapsed;
            errorPasswordMatch.Visibility = Visibility.Collapsed;
            errorPassword.Visibility = Visibility.Collapsed;

            // get values from register form
            string username = tbxUsername.Text;
            string email = tbxEMail.Text;
            string firstName = tbxFirstname.Text;
            string lastName = tbxLastname.Text;
            string password = tbxPassword.Password;
            string confirmPassword = tbxConfirmPassword.Password;

            // errors standard false
            bool errors = false;
            errorFirstname.Visibility = Visibility.Collapsed;
            errorUserName.Visibility = Visibility.Collapsed;
            errorEmail.Visibility = Visibility.Collapsed;
            errorPasswordMatch.Visibility = Visibility.Collapsed;
            errorPassword.Visibility = Visibility.Collapsed;

            // check if firstName is null or empty
            if (firstName.IsNullOrEmpty())
            {
                errorFirstname.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if username is not unique
            if (!_userController.IsUniqueUsername(username))
            {
                errorUserName.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if email is not valid
            if (!UserManager.IsValidEmail(email))
            {
                errorEmail.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if password does not equals confirmPassword
            if (password != confirmPassword)
            {
                errorPasswordMatch.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if password is not valid
            if (!PasswordManager.IsValidPassword(password))
            {
                errorPassword.Visibility = Visibility.Visible;
                errors = true;
            }

            // add a new user if there are no errors else show errors
            if (!errors)
            {
                _userController.Add(
                     new User(username, email, firstName, lastName, password)
                );
                Login login = new Login();
                login.Show();
                login.OnRegister();
                Close();
            }
            else
            {
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
    }
}