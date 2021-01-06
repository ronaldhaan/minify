using Minify.Core.Controllers;
using Minify.DAL.Entities;
using Minify.Core.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Minify.WPF.Managers;
using Minify.Core.Managers;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for OverviewStreamroom.xaml
    /// </summary>
    public partial class OverviewStreamroomPage : Page
    {
        private readonly Guid _streamroomId;
        private Streamroom _streamroom;
        private List<Song> _songs;
        private StreamroomManager _streamroomManager;
        private WpfMediaManager _mediaManager;

        public event StreamroomRefreshedEventHandler MessagesRefreshed;


        public OverviewStreamroomPage(Guid streamroomId, WpfMediaManager manager)
        {
            _mediaManager = manager;
            _streamroomId = streamroomId;
            _streamroomManager = new StreamroomManager(streamroomId, manager);
            _streamroomManager.StreamroomRefreshed += UpdateLocalStreamroom;
            InitializeComponent();

            var hitlistController = ControllerManager.Get<HitlistController>();
            var streamroomController = ControllerManager.Get<StreamroomController>();

            _streamroom = streamroomController.Get(streamroomId, true);

            if (_streamroom.Hitlist != null)
            {
                StreamroomTitle.Content = _streamroom.Hitlist.Title;
                if (!string.IsNullOrEmpty(_streamroom.Hitlist.Description))
                {
                    StreamroomDescription.Content = _streamroom.Hitlist.Description;
                    StreamroomDescription.Visibility = Visibility.Visible;
                }

                StreamroomInfo.Content = hitlistController.GetHitlistInfo(_streamroom.Hitlist);

                if (_streamroom.Hitlist.Songs != null && _streamroom.Hitlist.Songs.Count > 0)
                {
                    _songs = hitlistController.GetSongs(_streamroom.Hitlist.Songs);
                    _mediaManager.Open(_songs.FirstOrDefault(s => s.Id.Equals(_streamroom.CurrentSongId)));
                    _mediaManager.UpdatePosition(_streamroom.CurrentSongPosition);
                    StreamroomSongs.ItemsSource = _songs;
                    StreamroomSongs.Visibility = Visibility.Visible;
                    Refresh(_songs.First());
                }
            }

            new MessageController().CreateMessage(new Message
            {
                StreamroomId = streamroomId,
                Text = $"{AppData.UserName} neemt nu deel aan de stream!",
                UserId = AppData.UserId
            });
        }

        public void Refresh(Song song)
        {
            StreamroomSongs.ItemsSource = _songs;

            foreach (var item in StreamroomSongs.Items)
            {
                if (((Song)item).Equals(song))
                    StreamroomSongs.SelectedItem = item;
            }

            StreamroomSongs.Visibility = Visibility.Visible;
        }

        private void UpdateLocalStreamroom(object sender, LocalStreamroomUpdatedEventArgs e)
        {
            // Get data from the updates per second from the manager.
            _streamroom = e.Streamroom;

            if (_mediaManager.GetCurrentSong()?.Id != _streamroom.Song.Id)
            {
                _mediaManager.Close();
            }

            //invoken naar overview
            MessagesRefreshed?.Invoke(this, e);
        }

        public override void EndInit()
        {
            base.EndInit();
            // start with reloading the data.
            _streamroomManager.Start();
        }

        private void StreamroomSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mediaManager.Close();

            if (e.AddedItems.Count > 0)
            {
                // Get song
                Song selectedSong = (Song)e.AddedItems[0];

                // Initialize songs
                _mediaManager.Songs = _songs;

                // Open song
                _mediaManager.Open(selectedSong);

                // Play song
                _mediaManager.Play();
            }
        }

        public void Play()
        {
            _streamroomManager.Play();
        }

        public void Pause()
        {
            _streamroomManager.Pause();
        }



        public void Close()
        {
            _streamroomManager?.Close();

            ControllerManager.Get<MessageController>().CreateMessage(new Message
            {
                StreamroomId = _streamroomId,
                Text = $"{AppData.UserName} heeft de stream verlaten!",
                UserId = AppData.UserId
            });
        }

        public Guid GetStreamroomId()
        {
            return _streamroomId;
        }
    }
}