using MahApps.Metro.Controls;

using Minify.Core.Controllers;
using Minify.Core.Managers;

using System.Windows;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : MetroWindow
    {
        private readonly LoginController _controller;

        public Login()
        {
            LoginController controller = AppManager.Get<LoginController>();
            if (controller.IsSessionActive())
            {
                OpenOverview();
            }

            _controller = controller;

            InitializeComponent();
        }

        private void Create_Account_Button_click(object sender, RoutedEventArgs e)
        {
            Register register = new Register();
            register.Show();
            Close();
        }

        private void Login_Button_Click(object sender, RoutedEventArgs e) => TryLogin();

        private void Cancel_Button_Click(object sender, RoutedEventArgs e) => Close();

        public void OnRegister()
        {
            Messages.Visibility = Visibility.Visible;
            RegisteredMessage.Visibility = Visibility.Visible;
        }

        private void Password_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                TryLogin();
            }
        }

        private void TryLogin()
        {

            // display error message
            Messages.Visibility = Visibility.Collapsed;
            LoginErrorMessage.Visibility = Visibility.Collapsed;

            // get username and password from page
            string username = Username.Text;
            string password = Password.Password;

            // try to login with the values
            if (_controller.TryLogin(username, password))
            {
                OpenOverview();
            }
            else
            {
                // display error message
                Messages.Visibility = Visibility.Visible;
                LoginErrorMessage.Visibility = Visibility.Visible;
            }
        }

        private void OpenOverview()
        {
            MainWindow overview = new MainWindow();
            overview.Show();
            Close();
        }
    }
}