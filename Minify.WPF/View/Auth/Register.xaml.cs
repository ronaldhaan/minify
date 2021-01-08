using Castle.Core.Internal;

using MahApps.Metro.Controls;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.DAL.Entities;

using System.Windows;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : MetroWindow
    {
        private readonly RegisterController _controller;

        public Register()
        {
            InitializeComponent();
            _controller = ControllerManager.Get<RegisterController>();
        }

        private void Register_Button_Click(object sender, RoutedEventArgs e)
        {
            //set error messages on hidden
            UsernameErrorMessage.Visibility = Visibility.Collapsed;
            EmailErrorMessage.Visibility = Visibility.Collapsed;
            FirstNameErrorMessage.Visibility = Visibility.Collapsed;
            PasswordEqualsErrorMessage.Visibility = Visibility.Collapsed;
            PasswordErrorMessage.Visibility = Visibility.Collapsed;

            // get values from register form
            string username = tBox_Username.Text;
            string email = tBox_Email.Text;
            string firstName = tBox_First_name.Text;
            string lastName = tBox_Last_Name.Text;
            string password = tBox_Password.Password;
            string confirmPassword = tBox_Confirm_Password.Password;

            // errors standard false
            bool errors = false;

            // check if firstName is null or empty
            if (firstName.IsNullOrEmpty())
            {
                FirstNameErrorMessage.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if username is not unique
            if (!_controller.IsUniqueUsername(username))
            {
                UsernameErrorMessage.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if email is not valid
            if (!_controller.IsValidEmail(email))
            {
                EmailErrorMessage.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if password does not equals confirmPassword
            if (!_controller.PasswordEqualsConfirmPassword(password, confirmPassword))
            {
                PasswordEqualsErrorMessage.Visibility = Visibility.Visible;
                errors = true;
            }

            // check if password is not valid
            if (!_controller.IsValidPassword(password))
            {
                PasswordErrorMessage.Visibility = Visibility.Visible;
                PasswordErrorMessage2.Visibility = Visibility.Visible;
                errors = true;
            }

            // add a new user if there are no errors else show errors
            if (!errors)
            {
                _controller.Add(
                     new User(username, email, firstName, lastName, password)
                );
                Login login = new Login();
                login.Show();
                login.OnRegister();
                Close();
            }
            else
            {
                Errors.Visibility = Visibility.Visible;
            }
        }

        private void Cancel_Button_Click(object sender, RoutedEventArgs e)
        {
            Login login = new Login();
            login.Show();
            Close();
        }
    }
}