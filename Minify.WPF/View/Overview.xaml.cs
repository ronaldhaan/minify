using MahApps.Metro.Controls;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Managers;

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
    public partial class Overview : MetroWindow
    {
        private bool _autoScroll = true;
        private const string SEARCH_STANDARD_VAL = "Search...";

        private readonly HitlistController _hitlistController;
        private readonly StreamroomController _streamroomController;
        private readonly MessageController _messageController;
        private readonly SongController _songController;
        private readonly AppData appData;
        private readonly LoginController _loginController;

        private readonly WpfMediaManager _mediaManager;
        #region Pages
        private OverviewSongsPage _overviewSongsPage;
        private DetailHitlistPage _hitlistPage;
        private DetailStreamroomPage _overviewStreamroomPage;
        private AddHistlistPage _addHitlistPage;

        public DetailStreamroomPage OverviewStreamroomPage
        {
            get { return _overviewStreamroomPage; }
            set
            {
                if (value != null)
                    value.MessagesRefreshed += OverviewStreamroom_MessagesRefreshed;
                _overviewStreamroomPage = value;
            }
        }

        public AddHistlistPage AddHitlistPage
        {
            get { return _addHitlistPage; }
            set
            {
                value.HitlistAdded += UpdateHitlistMenu;
                _addHitlistPage = value;
            }
        }

        public DetailHitlistPage OverviewHitlistPage
        {
            get { return _hitlistPage; }
            set
            {
                value.MediaManager = _mediaManager;
                value.StreamroomCreated += OpenStreamroom;
                value.RefreshHitlistOverview += RefreshHitListMenu;
                _hitlistPage = value;
            }
        }

        public OverviewSongsPage OverviewSongsPage
        {
            get
            {
                return _overviewSongsPage;
            }
            set
            {
                _overviewSongsPage = value;
                _overviewSongsPage.SongSelected += PlaySong;
            }
        }
        #endregion Pages

        public Overview()
        {
            _hitlistController = AppManager.Get<HitlistController>();
            _messageController = AppManager.Get<MessageController>();
            _streamroomController = AppManager.Get<StreamroomController>();
            _loginController = AppManager.Get<LoginController>();
            _songController = AppManager.Get<SongController>();
            appData = AppManager.Get<AppData>();

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

        #region Events
        public void RefreshHitListMenu(object sender, EventArgs e)
        {
            InitializeHitListMenu();
            HitlistMenu.Items.Refresh();

            OverviewSongsPage = new OverviewSongsPage();
            contentFrame.Navigate(OverviewSongsPage);
        }

        public void UpdateHitlistMenu(object sender, UpdateHitlistMenuEventArgs e)
        {
            InitializeHitListMenu();
            HitlistMenu.Items.Refresh();

            // set the new item as selected
            foreach (var item in HitlistMenu.Items)
            {
                // cast ListViewItem to Hitlist and check if the id equals the eventargs id
                if (((Hitlist)item).Id.Equals(e.Id))
                {
                    HitlistMenu.SelectedItem = item;
                }
            }

            //display current hitlist
            OverviewHitlistPage = new DetailHitlistPage(e.Id);
            contentFrame.Navigate(OverviewHitlistPage); ;
        }

        private void Btn_Add_Hitlist(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            InitializeHitListMenu();
            InitializeStreamroomMenu();

            AddHitlistPage = new AddHistlistPage();
            contentFrame.Navigate(AddHitlistPage);
        }

        private void HitlistMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeStreamroomMenu();
            CloseStreamRoom();

            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is Hitlist selected)
            {
                HitlistMenu.SelectedItem = selected;
                OverviewHitlistPage = new DetailHitlistPage(selected.Id);
                contentFrame.Navigate(OverviewHitlistPage);
            }
        }



        private void MediaManager_UpdateMediaplayer(object sender, UpdateMediaplayerEventArgs e)
        {
            SetLabels(e);
        }

        private void PlaySong(object sender, PlaySongEventArgs e)
        {
            //DisplayPause(e.Position > _positionCache);
            _mediaManager.Open(e.Song);
            _mediaManager.Play();
        }

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

        private void SetLabels(UpdateMediaplayerEventArgs e)
        {
            if ((string)lblSongName.Content != e.SongName)
            {
                lblSongName.Content = e.SongName;
            }
            if ((string)lblArtist.Content != e.Artist)
            {
                lblArtist.Content = e.Artist;
            }
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
                RefreshPages();
            }
        }

        #region Controls

        private void Window_Initialized(object sender, EventArgs e)
        {
            InitializeHitListMenu();
            InitializeStreamroomMenu();
            lblUserName.Content = appData.UserName;
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Overview overview = new Overview();
            overview.Show();
            CloseStreamRoom();
            _mediaManager.Close();
            Close();
        }

        private void BtnSongs_Click(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            InitializeHitListMenu();
            InitializeStreamroomMenu();

            OverviewSongsPage = new OverviewSongsPage();
            contentFrame.Navigate(OverviewSongsPage);
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            InitializeHitListMenu();
            InitializeStreamroomMenu();

            contentFrame.Navigate(new DetailUserPage());

        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            InitializeHitListMenu();
            InitializeStreamroomMenu();

            contentFrame.Navigate(new SettingsPage());
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e) => Play();

        private void BtnPause_Click(object sender, RoutedEventArgs e) => Pause();

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            DisplayPause(_mediaManager.Next());

            RefreshPages();
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _mediaManager.Replay();
            RefreshPages();
        }

        private void BtnBack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DisplayPause(_mediaManager.Previous());
            RefreshPages();
        }

        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _mediaManager.Volume = (double)volumeSlider.Value;
        }

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
                    OverviewSongsPage = new OverviewSongsPage(songs);
                    contentFrame.Navigate(OverviewSongsPage);
                }
                else
                {
                    contentFrame.Navigate(new Label
                    {
                        Content = "No songs could be found"
                    });
                }
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                _autoScroll = ScrollViewer.VerticalOffset == ScrollViewer.ScrollableHeight;
            }

            if (_autoScroll && e.ExtentHeightChange != 0)
            {
                ScrollViewer.ScrollToVerticalOffset(ScrollViewer.ExtentHeight);
            }
        }

        private void TbxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CreateMessage();
            }
        }

        private void Streamroom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeHitListMenu();
            CloseStreamRoom();

            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is Streamroom selected)
            {
                streamrooms.SelectedItem = selected;
                OverviewStreamroomPage = new DetailStreamroomPage(selected.Id, _mediaManager);
                MessagePanel.Visibility = Visibility.Visible;
                contentFrame.Navigate(OverviewStreamroomPage);
            }
        }

        #endregion Controls
        private void MediaManager_OnPause(object sender, EventArgs e)
        {
            DisplayPlay();
        }

        private void MediaManager_OnPlay(object sender, UpdateMediaplayerEventArgs e)
        {
            DisplayPause();
            SetLabels(e);
        }

        private void OverviewStreamroom_MessagesRefreshed(object sender, LocalStreamroomUpdatedEventArgs e)
        {
            // e.Messages to your beautiful chat view
            Dispatcher.BeginInvoke(new ThreadStart(() =>
            {
                scrollviewMessages.Children.Clear();
                LoadMessages(e.Messages);
            }));
        }

        private void OpenStreamroom(object sender, CreatedStreamRoomEventArgs e)
        {
            CloseStreamRoom();

            MessagePanel.Visibility = Visibility.Visible;

            OverviewStreamroomPage = new DetailStreamroomPage(e.Streamroom.Id, _mediaManager);
            contentFrame.Navigate(OverviewStreamroomPage);

            timelineSlider.IsEnabled = appData.UserId == OverviewStreamroomPage.UserId;
        }

        private void CloseStreamRoom()
        {
            if (OverviewStreamroomPage != null)
            {
                MessagePanel.Visibility = Visibility.Hidden;
                OverviewStreamroomPage?.Close();
                OverviewStreamroomPage = null;
                _mediaManager.Stop();

                timelineSlider.IsEnabled = true;
            }
        }

        #endregion Events

        private void RefreshPages()
        {
            OverviewHitlistPage?.Refresh(_mediaManager.GetCurrentSong());

            OverviewSongsPage?.Refresh(_mediaManager.GetCurrentSong());

            OverviewStreamroomPage?.Refresh(_mediaManager.GetCurrentSong());
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


        public void LoadMessages(List<Message> messages) => messages.ForEach(m => LoadMessage(m));

        /// <summary>
        /// Sends chat message into the chatbox
        /// </summary>
        /// <param name="message"></param>
        public void LoadMessage(Message message)
        {
            StackPanel stackPanel = new StackPanel();
            TextBlock user = new TextBlock();
            TextBlock lblmessage = new TextBlock();

            stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            user.Text = $"{message.User.Person.FirstName} {message.User.Person.LastName} - {message.CreatedAt:t}";
            user.FontWeight = FontWeights.Bold;
            lblmessage.Text = message.Text;
            stackPanel.Children.Add(user);
            stackPanel.Children.Add(lblmessage);

            scrollviewMessages.Children.Add(stackPanel);
        }

        public void InitializeHitListMenu()
        {
            List<Hitlist> hitlists = _hitlistController.GetHitlistsByUserId(appData.UserId);
            HitlistMenu.ItemsSource = hitlists;
        }

        public void InitializeStreamroomMenu()
        {
            List<Streamroom> streamroom = _streamroomController.GetAll(true);
            streamrooms.ItemsSource = streamroom;
        }

        private void CreateMessage()
        {
            if (OverviewStreamroomPage != null && !string.IsNullOrEmpty(tbxMessage.Text))
            {
                Message message = new Message
                {
                    Text = tbxMessage.Text,
                    UserId = appData.UserId,
                    StreamroomId = OverviewStreamroomPage.StreamroomId
                };

                _messageController.CreateMessage(message);
                tbxMessage.Text = string.Empty;
            }
        }

        private void Play()
        {
            OverviewStreamroomPage?.Play();
            _mediaManager.Play();
            DisplayPause();
        }

        private void Pause()
        {
            OverviewStreamroomPage?.Pause();
            _mediaManager.Pause();
            DisplayPlay();
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