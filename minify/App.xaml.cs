using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using System.Windows;

namespace Minify.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppData.Initialize();
            InitializeControllers();
        }

        private void InitializeControllers()
        {
            ControllerManager.Initialize();
            ControllerManager.AddRange(
                new LoginController(),
                new RegisterController(),
                new HitlistController(),
                new MessageController(),
                new SongController(),
                new StreamroomController());
        }
    }
}