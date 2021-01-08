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
        private TimeSpan _positionCache;
        private bool _autoScroll = true;

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
                value.StreamroomCreated += OpenStreamroom;
                value.RefreshHitlistOverview += RefreshHitListMenu;
                _hitlistPage = value;
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

            InitializeComponent();

        }

        #region Events
        public void RefreshHitListMenu(object sender, EventArgs e)
        {
            InitializeHitListMenu();
            HitlistMenu.Items.Refresh();

            OverviewSongsPage overviewSongs = new OverviewSongsPage(_mediaManager);
            contentFrame.Content = overviewSongs;
        }

        public void UpdateHitlistMenu(object sender, UpdateHitlistMenuEventArgs e)
        {
            InitializeHitListMenu();
            HitlistMenu.Items.Refresh();

            //display current hitlist
            OverviewHitlistPage = new OverviewHitlistPage(e.Id, _mediaManager);

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
            OverviewStreamroomPage?.Close();
            _mediaManager.Close();

            if (e.AddedItems.Count > 0)
            {
                Hitlist selected = (Hitlist)e.AddedItems[0];
                HitlistMenu.SelectedItem = selected;
                OverviewHitlistPage = new OverviewHitlistPage(selected.Id, _mediaManager);
                contentFrame.Content = OverviewHitlistPage;
            }
        }

        private void DisplayPlay()
        {
            Dispatcher.Invoke(() =>
            {
                btn_Pause.Visibility = Visibility.Collapsed;
                btn_Play.Visibility = Visibility.Visible;
            });
        }

        private void DisplayPause()
        {
            Dispatcher.Invoke(() =>
            {
                btn_Play.Visibility = Visibility.Collapsed;
                btn_Pause.Visibility = Visibility.Visible;
            });
        }

        private void OnMouseDownPlay(object sender, MouseButtonEventArgs e)
        {
            _overviewStreamroomPage.Play();
            _mediaManager.Play();
            DisplayPause();
        }

        private void OnMouseDownPause(object sender, MouseButtonEventArgs e)
        {
            _overviewStreamroomPage.Pause();
            _mediaManager.Pause();
            DisplayPlay();
        }

        private void OnMouseDownBack(object sender, MouseButtonEventArgs e)
        {
            lbl_Current_Time.Content = "00:00";
            Song_Progressbar.Value = Song_Progressbar.Minimum;

            if (e.ClickCount == 1)
                _mediaManager.Replay();
            else
            {
                if (_mediaManager.Previous())
                    DisplayPause();
                else
                    DisplayPlay();
            }

            if (OverviewHitlistPage != null)
                OverviewHitlistPage.Refresh(_mediaManager.GetCurrentSong());

            if (_overviewSongsPage != null)
                _overviewSongsPage.Refresh(_mediaManager.GetCurrentSong());

            if (OverviewStreamroomPage != null)
                OverviewStreamroomPage.Refresh(_mediaManager.GetCurrentSong());
        }

        private void OnMouseDownNext(object sender, MouseButtonEventArgs e)
        {
            lbl_Current_Time.Content = lbl_Song_Duration.Content;
            Song_Progressbar.Value = Song_Progressbar.Maximum;

            if (_mediaManager.Next())
                DisplayPause();
            else
                DisplayPlay();

            if (OverviewHitlistPage != null)
                OverviewHitlistPage.Refresh(_mediaManager.GetCurrentSong());

            if (_overviewSongsPage != null)
                _overviewSongsPage.Refresh(_mediaManager.GetCurrentSong());

            if (OverviewStreamroomPage != null)
                OverviewStreamroomPage.Refresh(_mediaManager.GetCurrentSong());
        }

        private void UpdateMediaplayer(object sender, UpdateMediaplayerEventArgs e)
        {
            if (e.Position > _positionCache)
                DisplayPause();
            else
                DisplayPlay();

            _positionCache = e.Position;

            Dispatcher.Invoke(() => {
                lbl_Song_Name.Content = e.SongName;
                lbl_Artist.Content = e.Artist;
                lbl_Current_Time.Content = e.Position.ToString(@"mm\:ss");
                lbl_Song_Duration.Content = e.Duration.ToString(@"mm\:ss");
                Song_Progressbar.Maximum = e.Duration.TotalMilliseconds;
                Song_Progressbar.Value = e.Position.TotalMilliseconds;
            });

            if (e.SongName == null)
            {
                if (OverviewHitlistPage != null)
                    OverviewHitlistPage.Refresh(_mediaManager.GetCurrentSong());

                if (_overviewSongsPage != null)
                    _overviewSongsPage.Refresh(_mediaManager.GetCurrentSong());
            }
        }

        private void Btn_home(object sender, RoutedEventArgs e)
        {
            Overview overview = new Overview();
            overview.Show();
            Close();
        }

        private void Btn_songs(object sender, RoutedEventArgs e)
        {
            // Reset selected items
            InitializeHitListMenu();
            InitializeStreamroomMenu();

            _overviewSongsPage = new OverviewSongsPage(_mediaManager);
            contentFrame.Content = _overviewSongsPage;
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            InitializeHitListMenu();
            InitializeStreamroomMenu();
            label_Username.Content = AppData.UserName;
        }

        private void Btn_Logout(object sender, RoutedEventArgs e)
        {
            _loginController.Logout();
            OverviewStreamroomPage?.Close();
            _mediaManager.Close();
            Login login = new Login();
            login.Show();
            Close();
        }

        private void SearchBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (Search.Text != "Search..." && Search.Text != "")
            {
                var songs = _songController.Search(Search.Text);
                if (songs != null && songs.Count > 0)
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

        private void Search_MouseEnter(object sender, MouseEventArgs e)
        {
            if (Search.Text == "Search...")
            {
                Search.Text = "";
            }
        }

        private void OpenStreamroom(object sender, CreatedStreamRoomEventArgs e)
        {
            OverviewStreamroomPage?.Close();
            _mediaManager.Close();

            MessagePanel.Visibility = Visibility.Visible;

            _overviewStreamroomPage = new OverviewStreamroomPage(e.Streamroom.Id, _mediaManager);
            contentFrame.Content = _overviewStreamroomPage;
        }

        private void OverviewStreamroom_MessagesRefreshed(object sender, LocalStreamroomUpdatedEventArgs e)
        {
            // e.Messages to your beautiful chat view
            var messages = e.Messages;

            Dispatcher.BeginInvoke(new ThreadStart(() => scrollviewMessages.Children.Clear()));

            Dispatcher.BeginInvoke(new ThreadStart(() => LoadMessages(messages)));
        }

        private void ScrollViewer_ScrollChanged(object sender, System.Windows.Controls.ScrollChangedEventArgs e)
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

        private void Chat_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Message message = new Message
                {
                    Text = Chat.Text,
                    UserId = AppData.UserId,
                    //StreamroomId = new Guid("{197a232b-4bb7-4961-9153-81349df9d785}")
                    StreamroomId = OverviewStreamroomPage.GetStreamroomId()
                };
                _messageController.CreateMessage(message);
                Chat.Text = "";
            }
        }

        private void Streamroom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            InitializeHitListMenu();
            MessagePanel.Visibility = Visibility.Hidden;
            OverviewStreamroomPage?.Close();
            _mediaManager.Close();

            if (e.AddedItems.Count > 0)
            {
                Streamroom selected = (Streamroom)e.AddedItems[0];
                streamrooms.SelectedItem = selected;
                OverviewStreamroomPage = new OverviewStreamroomPage(selected.Id, _mediaManager);
                MessagePanel.Visibility = Visibility.Visible;
                contentFrame.Content = OverviewStreamroomPage;
            }
        }

        #endregion Events
        
        public void LoadMessages(List<Message> messages)
        {
            foreach (Message message in messages)
            {
                Chatmessage(message);
            }
        }

        /// <summary>
        /// Sends chat message into the chatbox
        /// </summary>
        /// <param name="message"></param>
        public void Chatmessage(Message message)
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


    }
}
