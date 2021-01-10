using Minify.Core.Controllers;
using Minify.DAL.Entities;
using Minify.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Minify.WPF.Managers;
using Minify.Core.Managers;
using System.Threading;
using MahApps.Metro.Controls;

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
        private readonly LoginController _loginController;

        private readonly WpfMediaManager _mediaManager;

        private OverviewSongsPage _overviewSongsPage;
        private OverviewHitlistPage _hitlistPage;
        private OverviewStreamroomPage _overviewStreamroomPage;
        private AddHistlistPage _addHitlistPage;

        public OverviewStreamroomPage OverviewStreamroomPage
        {
            get { return _overviewStreamroomPage; }
            set
            {
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

        public OverviewHitlistPage OverviewHitlistPage
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
                _overviewSongsPage.MediaManager = _mediaManager;
            }
        }

        public Overview()
        {
            _hitlistController = ControllerManager.Get<HitlistController>();
            _messageController = ControllerManager.Get<MessageController>();
            _streamroomController = ControllerManager.Get<StreamroomController>();
            _loginController = ControllerManager.Get<LoginController>();
            _songController = ControllerManager.Get<SongController>();

            _mediaManager = new WpfMediaManager(null);
            _mediaManager.UpdateMediaplayer += UpdateMediaplayer;
            _mediaManager.OnPlay += MediaManager_OnPlay;
            _mediaManager.OnPause += MediaManager_OnPause;

            InitializeComponent();
        }

        #region Events
        private void MediaManager_OnPause(object sender, EventArgs e)
        {
            DisplayPlay();
        }

        private void MediaManager_OnPlay(object sender, UpdateMediaplayerEventArgs e)
        {
            DisplayPause();
            SetLabels(e);
        }

        public void RefreshHitListMenu(object sender, EventArgs e)
        {
            InitializeHitListMenu();
            HitlistMenu.Items.Refresh();

            OverviewSongsPage overviewSongs = new OverviewSongsPage();
            contentFrame.Content = overviewSongs;
        }

        public void UpdateHitlistMenu(object sender, UpdateHitlistMenuEventArgs e)
        {
            InitializeHitListMenu();
            HitlistMenu.Items.Refresh();

            //display current hitlist
            OverviewHitlistPage = new OverviewHitlistPage(e.Id);

            // set the new item as selected
            foreach (var item in HitlistMenu.Items)
            {
                // cast ListViewItem to Hitlist and check if the id equals the eventargs id
                if (((Hitlist)item).Id.Equals(e.Id))
                {
                    HitlistMenu.SelectedItem = item;
                }
            }

            contentFrame.Content = OverviewHitlistPage;
        }

        private void Btn_Add_Hitlist(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            InitializeHitListMenu();
            InitializeStreamroomMenu();

            AddHitlistPage = new AddHistlistPage();
            contentFrame.Content = AddHitlistPage;
        }

        private void HitlistMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeStreamroomMenu();
            CloseStreamRoom();

            if (!Utility.ListIsNullOrEmpty(e.AddedItems))
            {
                Hitlist selected = (Hitlist)e.AddedItems[0];
                HitlistMenu.SelectedItem = selected;
                OverviewHitlistPage = new OverviewHitlistPage(selected.Id);
                contentFrame.Content = OverviewHitlistPage;
            }
        }

        

        private void UpdateMediaplayer(object sender, UpdateMediaplayerEventArgs e)
        {
            //DisplayPause(e.Position > _positionCache);

            SetLabels(e);
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

            timelineSlider.Maximum = e.Duration.TotalMilliseconds;

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
            lblUserName.Content = AppData.UserName;
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Overview overview = new Overview();
            overview.Show();
            Close();
        }

        private void BtnSongs_Click(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            InitializeHitListMenu();
            InitializeStreamroomMenu();

            OverviewSongsPage = new OverviewSongsPage(_mediaManager);
            contentFrame.Content = OverviewSongsPage;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            _loginController.Logout();
            CloseStreamRoom();
            Login login = new Login();
            login.Show();
            Close();
        }
        #endregion Controls
        private void TbxSearchSongs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxSearchSongs.Text) && tbxSearchSongs.Text != SEARCH_STANDARD_VAL)
            {
                var songs = _songController.Search(tbxSearchSongs.Text);
                if (!Utility.ListIsNullOrEmpty(songs))
                {
                    OverviewSongsPage overviewSongs = new OverviewSongsPage(songs);
                    contentFrame.Content = overviewSongs;
                }
                else
                {
                    Label label = new Label
                    {
                        Content = "No songs could be found"
                    };
                    contentFrame.Content = label;
                }
            }
        }

        private void OpenStreamroom(object sender, CreatedStreamRoomEventArgs e)
        {
            CloseStreamRoom();

            MessagePanel.Visibility = Visibility.Visible;

            OverviewStreamroomPage = new OverviewStreamroomPage(e.Streamroom.Id, _mediaManager);
            contentFrame.Content = OverviewStreamroomPage;
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
            
            if (!Utility.ListIsNullOrEmpty(e.AddedItems))
            {
                Streamroom selected = (Streamroom)e.AddedItems[0];
                streamrooms.SelectedItem = selected;
                OverviewStreamroomPage = new OverviewStreamroomPage(selected.Id, _mediaManager);
                MessagePanel.Visibility = Visibility.Visible;
                contentFrame.Content = OverviewStreamroomPage;
            }
        }

        private void CloseStreamRoom()
        {
            MessagePanel.Visibility = Visibility.Hidden;
            OverviewStreamroomPage?.Close();
            _mediaManager.Close();
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
            StackPanel stackPanel = new StackPanel
            {
                HorizontalAlignment = HorizontalAlignment.Stretch
            };

            Label user = new Label();
            Label lblmessage = new Label();
            user.Content = $"{message.User.FirstName} {message.User.LastName} - {message.CreatedAt:t}";
            user.FontWeight = FontWeights.Bold;
            lblmessage.Content = message.Text;
            stackPanel.Children.Add(user);
            stackPanel.Children.Add(lblmessage);

            scrollviewMessages.Children.Add(stackPanel);
        }

        public void InitializeHitListMenu()
        {
            List<Hitlist> hitlists = _hitlistController.GetHitlistsByUserId(AppData.UserId);
            HitlistMenu.ItemsSource = hitlists;
        }


        public void InitializeStreamroomMenu()
        {
            List<Streamroom> streamroom = _streamroomController.GetAll(true);
            streamrooms.ItemsSource = streamroom;
        }

        private void CreateMessage()
        {
            if (OverviewStreamroomPage != null)
            {
                Message message = new Message
                {
                    Text = tbxMessage.Text,
                    UserId = AppData.UserId,
                    StreamroomId = OverviewStreamroomPage.GetStreamroom().Id
                };
                _messageController.CreateMessage(message);
                tbxMessage.Text = string.Empty;
            }
        }

        private void Play()
        {
            OverviewStreamroomPage.Play();
            _mediaManager.Play();
            DisplayPause();
        }

        private void Pause()
        {
            OverviewStreamroomPage.Pause();
            _mediaManager.Pause();
            DisplayPlay();
        }

        private void BtnUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {

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

        private void Song_Progressbar_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> args)
        {
            int SliderValue = (int)timelineSlider.Value;

            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            TimeSpan ts = new TimeSpan(0, 0, 0, 0, SliderValue);
            _mediaManager.Position = ts;
            
            lblDuration.Content = _mediaManager.GetCurrentSong().Duration.ToString(@"mm\:ss");
            lblCurrentTime.Content = _mediaManager.Position.ToString(@"mm\:ss");
        }
    }
}