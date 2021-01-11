using ControlzEx.Theming;

using MahApps.Metro.Theming;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;

using System;
using System.IO;
using System.Windows;
using System.Windows.Media;

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

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set the application theme to Dark.Green
            //ThemeManager.Current.ChangeTheme(this, "Dark.Red");
        }

        private void InitializeControllers()
        {
            ControllerManager.Initialize();
            ControllerManager.AddRange(
                new LoginController(),
                new HitlistController(),
                new MessageController(),
                new SongController(),
                new StreamroomController(),
                new UserController());
        }
    }
}