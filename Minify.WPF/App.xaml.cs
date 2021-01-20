using ControlzEx.Theming;

using MahApps.Metro.Theming;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;

using System;
using System.Threading;
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
            InitializeControllers();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            SetLanguageDictionary();           

            base.OnStartup(e);

            SetTheme();
        }

        private void InitializeControllers()
        {
            AppData appData = new AppData();
            appData.SaveUserData += AppData_SaveUserData;

            var settings = WPF.Properties.Settings.Default;
            if (settings.LoginDate.AddMilliseconds(settings.ExpireLogin) < DateTime.Now)
            {
                appData.DestroySession();
            }
            else
            {
                appData.LoggedIn = true;
            }

            appData.ExpireLogin = settings.ExpireLogin;
            appData.LoginDate = settings.LoginDate;
            appData.UserName = settings.UserName;
            appData.UserId = settings.UserId;

            AppManager.Initialize();
            AppManager.Add(appData);
            AppManager.AddRange(
                new LoginController(),
                new HitlistController(),
                new MessageController(),
                new SongController(),
                new StreamroomController(),
                new UserController());
        }

        private void SaveData(AppData data = null)
        {
            if (data == null)
            {
                data = AppManager.Get<AppData>();
            }

            var settings = WPF.Properties.Settings.Default;
            settings.UserId = data.UserId;
            settings.UserName = data.UserName;
            settings.LoginDate = data.LoginDate;
            settings.Upgrade();
            settings.Save();
        }

        private void AppData_SaveUserData(object sender, EventArgs e) => SaveData((AppData)sender);

        private void SetLanguageDictionary()
        {
            Resources.MergedDictionaries.Add(new ResourceDictionary
            {
                Source = (Thread.CurrentThread.CurrentCulture.ToString()) switch
                {
                    "en-US" => new Uri(".\\Resources\\StringResources.xaml", UriKind.Relative),
                    "nl-NL" => new Uri(".\\Resources\\StringResources.nl-NL.xaml", UriKind.Relative),
                    _ => new Uri(".\\Resources\\StringResources.xaml", UriKind.Relative),
                }
            });
        }

        private void SetTheme()
        {
            UtilityWpf.DarkTheme = ThemeManager.Current.AddLibraryTheme(new LibraryTheme(
                new Uri("pack://application:,,,/Minify.WPF;component/View/Styles/Dark.Accent1.xaml"),
                MahAppsLibraryThemeProvider.DefaultInstance));

            UtilityWpf.LightTheme = ThemeManager.Current.AddLibraryTheme(new LibraryTheme(
                new Uri("pack://application:,,,/Minify.WPF;component/View/Styles/Light.Accent1.xaml"),
                MahAppsLibraryThemeProvider.DefaultInstance));

            UtilityWpf.Application = this;

            UtilityWpf.SetLightTheme();
        }
    }
}