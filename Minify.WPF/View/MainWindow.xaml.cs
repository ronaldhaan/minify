using MahApps.Metro.Controls;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Managers;
using Minify.WPF.Models;

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

        private readonly WpfMediaManager _mediaManager;

        public WpfMediaManager MediaManager { get => _mediaManager;  }

        public MainWindow()
        {
            _loginController = AppManager.Get<LoginController>();
            _songController = AppManager.Get<SongController>();
            appData = AppManager.Get<AppData>();

            _navigation = new Navigation(this);

            _mediaManager = new WpfMediaManager(null);
            _mediaManager.UpdateMediaplayer += MediaManager_UpdateMediaplayer;
            _mediaManager.OnPlay += MediaManager_OnPlay;
            _mediaManager.OnPause += MediaManager_OnPause;

            InitializeComponent();

            _mediaManager.Volume = (double)volumeSlider.Value;


            var user = AppManager.Get<UserController>().Get(appData.UserId);

            if (user.DefaultTheme == DAL.Models.DefaultTheme.Light)
                UtilityWpf.SetLightTheme();
            else
                UtilityWpf.SetDarkTheme();
        }

        public void HitlistSongSelectedChanged(object sender, PlaySongEventArgs e)
        {
            _mediaManager.Songs = e.Songs;
            _mediaManager.Open(e.Song);
            _mediaManager.Play();
        }

        #region Events

        #region HitlistMenu
        public void RefreshHitListMenu(object sender, EventArgs e)
        {
            ResetSelectedHitlist();
            HitlistMenu.Refresh();

            Navigate(_navigation.CreateOverviewSongsPage()); ;
        }

        public void UpdateHitlistMenu(object sender, UpdateHitlistMenuEventArgs e)
        {
            // set the new item as selected
            HitlistMenu.UpdateSelected(e.Id);

            //display current hitlist
            Navigate(_navigation.CreateDetailHitlistPage(e.Id));
        }

        private void ResetSelectedHitlist() => HitlistMenu.Reset();

        private void HitlistMenu_SelectionChanged(object sender, EntitySelectionChangedEventArgs e)
        {
            ResetSelectedStreamroom();
            CloseStreamRoom();
            Navigate(_navigation.CreateDetailHitlistPage(e.Id));
        }
        #endregion HitlistMenu

        #region Streamroom Control
        public void Streamroom_SelectionChanged(object sender, EntitySelectionChangedEventArgs e)
        {
            ResetSelectedHitlist();
            CloseStreamRoom();
            Navigate(_navigation.CreateDetailStreamroomPage(e.Id, _mediaManager));
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

        public void OpenStreamroom(object sender, CreatedStreamRoomEventArgs e)
        {
            CloseStreamRoom();


            MessagePanel.Visibility = Visibility.Visible;

            // set the new item as selected
            streamroomMenu.UpdateSelected(e.Streamroom.Id);
            Navigate(_navigation.CreateDetailStreamroomPage(e.Streamroom.Id, _mediaManager));

            timelineSlider.IsEnabled = _navigation.DetailStreamroomPage.UserId is Guid id && appData.BelongsEntityToUser(id);
        }

        private void CloseStreamRoom()
        {
            if (_navigation.DetailStreamroomPage != null)
            {
                MessagePanel.Visibility = Visibility.Collapsed;
                _navigation.DetailStreamroomPage?.Close();
                _navigation.DetailStreamroomPage = null;
                _mediaManager.Stop();

                timelineSlider.IsEnabled = true;
            }
        }
        #endregion Streamroom Control

        private void MediaManager_UpdateMediaplayer(object sender, UpdateMediaplayerEventArgs e) => SetSongData(e);

        public void PlaySong(object sender, PlaySongEventArgs e)
        {
            _mediaManager.Songs = e.Songs;
            _mediaManager.Open(e.Song);
            _mediaManager.Play();
        }

        private void SetSongData(UpdateMediaplayerEventArgs e)
        {
            if ((string)lblSongName.Content != e.SongName)
            {
                lblSongName.Content = e.SongName;
            }
            if ((string)lblArtist.Content != e.Artist)
            {
                lblArtist.Content = e.Artist;
            }

            double duration = e.Duration.TotalMilliseconds;
            if (timelineSlider.Maximum != duration)
            {
                timelineSlider.Maximum = e.Duration.TotalMilliseconds;
            }

            timelineSlider.Value = e.Position.TotalMilliseconds;


            if (e.SongName == null)
            {
                CanRefresh();
            }
        }

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
            _mediaManager.Close();
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

        private void BtnPlay_Click(object sender, RoutedEventArgs e) => Play();

        private void BtnPause_Click(object sender, RoutedEventArgs e) => Pause();

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if(CanRefresh())
                DisplayPause(_mediaManager.Next());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            if(CanRefresh())
                _mediaManager.Replay();
        }

        private void BtnBack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CanRefresh())
                DisplayPause(_mediaManager.Previous());
        }

        private bool CanRefresh() => _navigation.Refresh(_mediaManager.GetCurrentSong());

        private void TimelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            double minBetween = 2000;
            if (e.OldValue - e.NewValue > minBetween || e.NewValue - e.OldValue > minBetween)
            {
                int SliderValue = (int)timelineSlider.Value;
                _mediaManager.Position = new TimeSpan(0, 0, 0, 0, SliderValue);
            }

            var duration = _mediaManager.GetCurrentSong().Duration.ToString(@"mm\:ss");
            if (lblDuration.Content != null && (string)lblDuration.Content != duration)
            {
                lblDuration.Content = duration;
            }

            var position = _mediaManager.Position.ToString(@"mm\:ss");
            if (lblCurrentTime.Content != null && (string)lblCurrentTime.Content != position)
            {
                lblCurrentTime.Content = position;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _loginController.Logout();
            CloseStreamRoom();
            _mediaManager.Close();
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
                   Navigate(new Label
                    {
                        Content = "No songs could be found"
                    });
                }
            }
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _mediaManager.Volume = (double)volumeSlider.Value;
        }   

        #endregion Controls
        private void MediaManager_OnPause(object sender, EventArgs e)
        {
            DisplayPlay();
        }

        private void MediaManager_OnPlay(object sender, UpdateMediaplayerEventArgs e)
        {
            DisplayPause();
            SetSongData(e);
        }
        #endregion Events
        private void DisplayPause(bool condition)
        {
            if (condition)
            {
                DisplayPause();
            }
            else
            {
                DisplayPlay();
            }
        }

        private void DisplayPlay()
        {
            Dispatcher.Invoke(() =>
            {
                btnPause.Visibility = Visibility.Collapsed;
                btnPlay.Visibility = Visibility.Visible;
            });
        }

        private void DisplayPause()
        {
            Dispatcher.Invoke(() =>
            {
                btnPlay.Visibility = Visibility.Collapsed;
                btnPause.Visibility = Visibility.Visible;
            });
        }

        private void Navigate(object obj) => contentFrame.Navigate(obj);

        public void ResetSelectedStreamroom()
        {
            streamroomMenu.Reset();
            streamroomMenu.Update();
        }

        private void Play()
        {
            if (CanRefresh())
            {
                _navigation.DetailStreamroomPage?.Play();
                _mediaManager.Play();
            }
        }

        private void Pause()
        {
            if (CanRefresh())
            {
                _navigation.DetailStreamroomPage?.Pause();
                _mediaManager.Pause();
            }
        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (_mediaManager.Paused)
                {
                    Play();
                }
                else
                {
                    Pause();
                }
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