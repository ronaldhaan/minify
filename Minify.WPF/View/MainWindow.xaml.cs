using MahApps.Metro.Controls;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Managers;
using Minify.WPF.Models;
using Minify.WPF.ViewModels;

using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for Overview.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private const string SEARCH_STANDARD_VAL = "Search...";

        private readonly SongController _songController;
        private readonly AppData appData;
        private readonly LoginController _loginController;

        private readonly Navigation _navigation;

        public MainWindow()
        {
            _loginController = AppManager.Get<LoginController>();
            _songController = AppManager.Get<SongController>();
            appData = AppManager.Get<AppData>();

            _navigation = new Navigation(this);

            InitializeComponent();

            var user = AppManager.Get<UserController>().Get(appData.UserId);

            if (user.DefaultTheme == DAL.Models.DefaultTheme.Light)
                UtilityWpf.SetLightTheme();
            else
                UtilityWpf.SetDarkTheme();
        }

        public override void EndInit()
        {
            base.EndInit();
            UpdateHitlistMenu();
            UpdateStreamroomMenu();

            mediaControl.OnPlay += MediaControl_OnPlay;
            mediaControl.OnPause += MediaControl_OnPause;
        }


        #region Events

        private void MediaControl_OnPause(object sender, EventArgs e)
        {
            _navigation.DetailStreamroomPage?.Pause();
        }

        internal void PlaySong(object sender, PlaySongEventArgs e)
        {
            mediaControl.PlaySong(this, e);
        }

        private void MediaControl_OnPlay(object sender, EventArgs e)
        {
            _navigation.DetailStreamroomPage?.Play();
        }

        
        #region HitlistMenu
        public void HitlistSongSelectedChanged(object sender, PlaySongEventArgs e)
        {
            mediaControl.PlaySong(sender, e);
        }

        private void UpdateHitlistMenu()
        {
            var hitlists = AppManager.Get<HitlistController>().GetHitlistsByUserId(appData.UserId);

            HitlistMenu.ItemsSource = ListControlViewModel.GetViewModel(hitlists);
        }

        public void RefreshHitListMenu(object sender, EventArgs e)
        {
            ResetSelectedHitlist();
            UpdateHitlistMenu();

            Navigate(_navigation.CreateOverviewSongsPage()); ;
        }

        public void AddHitlistPage_UpdateHitlistMenu(object sender, UpdateHitlistMenuEventArgs e)
        {
            UpdateHitlistMenu();
            // set the new item as selected
            HitlistMenu.UpdateSelected(e.Id);

            //display current hitlist
            Navigate(_navigation.CreateDetailHitlistPage(e.Id));
        }

        private void ResetSelectedHitlist() => HitlistMenu.Reset();

        private void HitlistMenu_SelectionChanged(object sender, ListControlSelectionChangedEventArgs e)
        {
            ResetSelectedStreamroom();
            CloseStreamRoom();
            Navigate(_navigation.CreateDetailHitlistPage(e.Id));
        }
        #endregion HitlistMenu

        #region Streamroom Control
        private void UpdateStreamroomMenu()
        {
            var streamrooms = AppManager.Get<StreamroomController>().GetAll(true);

            streamroomMenu.ItemsSource = ListControlViewModel.GetViewModel(streamrooms);

        }

        public void Streamroom_SelectionChanged(object sender, ListControlSelectionChangedEventArgs e)
        {
            CloseStreamRoom();
            ResetSelectedHitlist();
            OpenStreamroom(e.Id);
        }

        public void OverviewStreamroom_MessagesRefreshed(object sender, LocalStreamroomUpdatedEventArgs e)
        {
            // e.Messages to your beautiful chat view
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                chatControl.Clear();
                chatControl.LoadMessages(e.Messages);
            }));
        }

        public void DetailHitlistPage_StreamroomCreated(object sender, CreatedStreamRoomEventArgs e)
        {
            CloseStreamRoom();
            ResetSelectedHitlist();
            // set the new item as selected
            UpdateStreamroomMenu();
            streamroomMenu.UpdateSelected(e.Streamroom.Id);

            OpenStreamroom(e.Streamroom.Id);
        }

        public void OpenStreamroom(Guid id)
        {
            MessagePanel.Visibility = Visibility.Visible;
            chatControl.StreamroomId = id;

            Navigate(_navigation.CreateDetailStreamroomPage(id, mediaControl.MediaManager));

            mediaControl.EnableSlider(_navigation.DetailStreamroomPage.UserId is Guid userId && appData.BelongsEntityToUser(userId));
        }

        private void CloseStreamRoom()
        {
            if (_navigation.DetailStreamroomPage != null)
            {
                MessagePanel.Visibility = Visibility.Collapsed;
                _navigation.DetailStreamroomPage?.Close();
                _navigation.DetailStreamroomPage = null;
                mediaControl.Stop();

                mediaControl.EnableSlider(true);
            }
        }
        #endregion Streamroom Control


        #region Controls

        private void Window_Initialized(object sender, EventArgs e)
        {
            ResetSelectedHitlist();
            ResetSelectedStreamroom();
            lblUserName.Content = appData.UserName;
        }

        #region Navigation Buttons
        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            MainWindow overview = new MainWindow();
            overview.Show();
            CloseStreamRoom();
            //_mediaManager.Close();
            Close();
        }

        private void BtnSongs_Click(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            ResetSelectedHitlist();
            ResetSelectedStreamroom();

           Navigate(_navigation.CreateOverviewSongsPage());
        }

        private void Btn_Add_Hitlist(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            ResetSelectedHitlist();
            ResetSelectedStreamroom();

            Navigate(_navigation.CreateAddHilistPage());
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            ResetSelectedHitlist();
            ResetSelectedStreamroom();

           Navigate(new DetailUserPage());

        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            ResetSelectedHitlist();
            ResetSelectedStreamroom();

           Navigate(new SettingsPage());
        }
        #endregion Navigation Buttons

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _loginController.Logout();
            CloseStreamRoom();
            mediaControl.Close();
            Login login = new Login();
            login.Show();
            Close();
        }

        private void TbxSearchSongs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxSearchSongs.Text) && tbxSearchSongs.Text != SEARCH_STANDARD_VAL)
            {
                var songs = _songController.Search(tbxSearchSongs.Text);
                if (!Core.Utility.ListIsNullOrEmpty(songs))
                {
                   Navigate(_navigation.CreateOverviewSongsPage(songs));
                }
                else
                {
                   Navigate(new Label { Content = "No songs could be found" });
                }
            }
        }           
        #endregion Controls
        #endregion Events

        private void Navigate(object obj) => contentFrame?.Navigate(obj);

        public void ResetSelectedStreamroom()
        {
            streamroomMenu.Reset();
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                mediaControl.ToggleMediaPlayer();
            }
        }

        private void Hitlist_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // open overview hitlists page
        }

        private void Streamroom_MouseDown(object sender, MouseButtonEventArgs e)
        {
            // open overview streamrooms page
        }
    }
}