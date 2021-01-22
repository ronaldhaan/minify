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

        private readonly HitlistController _hitlistController;
        private readonly MessageController _messageController;
        private readonly AppData appData;
        private Streamroom _streamroom;
        private List<Song> _songs;

        public event StreamroomRefreshedEventHandler MessagesRefreshed;

        public event EventHandler<PlaySongEventArgs> OnPlaySong;

        public Guid StreamroomId { get => _streamroomId; }

        public Guid? UserId { get => _streamroom?.Hitlist?.UserId; }

        public DetailStreamroomPage(Guid streamroomId)
        {
            _streamroomId = streamroomId;
            _streamroomManager = new StreamroomManager(streamroomId);
            _streamroomManager.StreamroomRefreshed += UpdateLocalStreamroom;

            _hitlistController = AppManager.Get<HitlistController>();
            _messageController = AppManager.Get<MessageController>();
            appData = AppManager.Get<AppData>();

            _streamroom = AppManager.Get<StreamroomController>().Get(streamroomId, true);

            InitializeComponent();

            if(!appData.BelongsEntityToUser(_streamroom.Hitlist.UserId))
            {
                var style = new Style(typeof(ListView));
                //style.Setters.Add(new Setter(IsEnabledProperty, false));
                streamroomSongs.ItemContainerStyle.Setters.Add(new Setter(IsEnabledProperty, false));
            }

            if (_streamroom.Hitlist != null)
            {
                SetTextBlocks();

                PlaySong();
            }

            CreateMessage($"{appData.UserName} neemt nu deel aan de stream!");
            _streamroomManager.Start();
        }

        private void PlaySong()
        {
            if (!Core.Utility.CollectionIsNullOrEmpty(_streamroom.Hitlist.Songs))
            {
                _songs = _hitlistController.GetSongs(_streamroom.Hitlist.Songs);
                var song = _songs.FirstOrDefault(s => s.Id.Equals(_streamroom.CurrentSongId));

                if (song != null)
                {
                    OnPlaySong?.Invoke(this, new PlaySongEventArgs(song) { CurrentSongPosition = _streamroom.CurrentSongPosition });
                    streamroomSongs.ItemsSource = _songs;
                    streamroomSongs.Visibility = Visibility.Visible;
                }
            }
        }

        private void SetTextBlocks()
        {
            StreamroomTitle.Content = _streamroom.Hitlist.Title;
            if (!string.IsNullOrEmpty(_streamroom.Hitlist.Description))
            {
                StreamroomDescription.Content = _streamroom.Hitlist.Description;
                StreamroomDescription.Visibility = Visibility.Visible;
            }

            StreamroomInfo.Content = _hitlistController.GetHitlistInfo(_streamroom.Hitlist);
        }

        public bool Refresh(Song song)
        {
            if (!appData.BelongsEntityToUser(_streamroom.Hitlist.UserId)) return false;

            Play(song);

            return true;
        }

        private void Play(Song song)
        {
            streamroomSongs.ItemsSource = _songs;

            foreach (var item in streamroomSongs.Items)
            {
                if (item is Song s && s.Equals(song))
                    streamroomSongs.SelectedItem = item;
            }

            streamroomSongs.Visibility = Visibility.Visible;
        }

        private void UpdateLocalStreamroom(object sender, LocalStreamroomUpdatedEventArgs e)
        {
            // Get data from the updates per second from the manager.
            if (_streamroomId == e.Streamroom.Id)
            {
                _streamroom = e.Streamroom;

                #warning implementation needed
                //if (_mediaManager.GetCurrentSong()?.Id != _streamroom.Song.Id)
                //{
                //    _mediaManager.Close();
                //}

                //invoken naar overview
                MessagesRefreshed?.Invoke(this, e);
            }
        }

        private void StreamroomSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is Song selected)
            {
                OnPlaySong?.Invoke(this, new PlaySongEventArgs(selected, _songs));
            }
        }

        public void Play() => _streamroomManager.Play();

        public void Pause() => _streamroomManager.Pause();

        public void Close()
        {
            _streamroomManager?.Close();

            CreateMessage($"{appData.UserName} heeft de stream verlaten!");
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