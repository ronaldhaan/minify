using System;
using System.Collections.ObjectModel;

using MahApps.Metro.IconPacks;


using Minify.WPF.View;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Minify.WPF.ViewModels
{
    [DataContract]
    public abstract class BindableBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class ShellViewModel : BindableBase
    {
        private static readonly ObservableCollection<MenuItem> AppMenu = new ObservableCollection<MenuItem>();
        private static readonly ObservableCollection<MenuItem> AppOptionsMenu = new ObservableCollection<MenuItem>();

        public ObservableCollection<MenuItem> Menu => AppMenu;

        public ObservableCollection<MenuItem> OptionsMenu => AppOptionsMenu;

        public ShellViewModel()
        {
            // Build the menus
            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.MusicSolid },
                Label = "Songs",
                NavigationType = typeof(OverviewSongsPage),
                NavigationDestination = new Uri("/Views/Song/OverviewSongsPage.xaml", UriKind.RelativeOrAbsolute)
            });
            Menu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.UserSolid },
                Label = "User",
                //NavigationType = typeof(UserPage),
                //NavigationDestination = new Uri("Views/UserPage.xaml", UriKind.RelativeOrAbsolute)
            });

            OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.CogsSolid },
                Label = "Settings",
                //NavigationType = typeof(SettingsPage),
                //NavigationDestination = new Uri("Views/SettingsPage.xaml", UriKind.RelativeOrAbsolute)
            });
            OptionsMenu.Add(new MenuItem()
            {
                Icon = new PackIconFontAwesome() { Kind = PackIconFontAwesomeKind.InfoCircleSolid },
                Label = "About",
                //NavigationType = typeof(AboutPage),
                //NavigationDestination = new Uri("Views/AboutPage.xaml", UriKind.RelativeOrAbsolute)
            });
        }
    }
}