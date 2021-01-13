using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Managers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for OverviewStreamroom.xaml
    /// </summary>
    public partial class DetailStreamroomPage : Page
    {
        private readonly Guid _streamroomId;

        private readonly StreamroomManager _streamroomManager;
        private readonly WpfMediaManager _mediaManager;

        private readonly HitlistController _hitlistController;
        private readonly MessageController _messageController;
        private readonly AppData appData;
        private Streamroom _streamroom;
        private List<Song> _songs;

        public event StreamroomRefreshedEventHandler MessagesRefreshed;

        public Guid StreamroomId { get => _streamroomId; }

        public Guid? UserId { get => _streamroom?.Hitlist?.UserId; }

        public DetailStreamroomPage(Guid streamroomId, WpfMediaManager manager)
        {
            _mediaManager = manager;
            _streamroomId = streamroomId;
            _streamroomManager = new StreamroomManager(streamroomId, manager);
            _streamroomManager.StreamroomRefreshed += UpdateLocalStreamroom;

            _hitlistController = AppManager.Get<HitlistController>();
            _messageController = AppManager.Get<MessageController>();
            appData = AppManager.Get<AppData>();

            _streamroom = AppManager.Get<StreamroomController>().Get(streamroomId, true);

            InitializeComponent();

            if (_streamroom.Hitlist != null)
            {
                SetTextBlocks();

                PlaySong();
            }

            CreateMessage($"{appData.UserName} neemt nu deel aan de stream!");
        }

        private void PlaySong()
        {
            if (!Core.Utility.CollectionIsNullOrEmpty(_streamroom.Hitlist.Songs))
            {
                _songs = _hitlistController.GetSongs(_streamroom.Hitlist.Songs);
                _mediaManager.Open(_songs.FirstOrDefault(s => s.Id.Equals(_streamroom.CurrentSongId)));
                _mediaManager.CurrentSongPosition = _streamroom.CurrentSongPosition;
                StreamroomSongs.ItemsSource = _songs;
                StreamroomSongs.Visibility = Visibility.Visible;
                Refresh(_songs.First());
            }
        }

        private void SetTextBlocks()
        {
            StreamroomTitle.Text = _streamroom.Hitlist.Title;
            if (!string.IsNullOrEmpty(_streamroom.Hitlist.Description))
            {
                StreamroomDescription.Text = _streamroom.Hitlist.Description;
                StreamroomDescription.Visibility = Visibility.Visible;
            }

            StreamroomInfo.Text = _hitlistController.GetHitlistInfo(_streamroom.Hitlist);
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
            if (_streamroomId == e.Streamroom.Id)
            {
                _streamroom = e.Streamroom;

                if (_mediaManager.GetCurrentSong()?.Id != _streamroom.Song.Id)
                {
                    _mediaManager.Close();
                }

                //invoken naar overview
                MessagesRefreshed?.Invoke(this, e);
            }
        }

        private void StreamroomSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _mediaManager.Close();

            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is Song selected)
            {
                // Initialize songs
                _mediaManager.Songs = _songs;
                // Open song
                _mediaManager.Open(selected);

                // Play song
                _mediaManager.Play();
            }
        }
        public void Play() => _streamroomManager.Play();

        public void Pause() => _streamroomManager.Pause();

        public void Close()
        {
            _streamroomManager?.Close();

            CreateMessage($"{appData.UserName} heeft de stream verlaten!");
        }

        public override void EndInit()
        {
            base.EndInit();
            // start with reloading the data.
            _streamroomManager.Start();
        }
        private void CreateMessage(string message)
        {
            _messageController.CreateMessage(new Message
            {
                StreamroomId = _streamroomId,
                Text = message,
                UserId = appData.UserId
            });
        }
    }
}