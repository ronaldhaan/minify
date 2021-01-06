using Minify.DAL.Entities;
using Minify.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using Minify.Core.Managers;

namespace Minify.WPF.Managers
{
    public class WpfMediaManager : MediaManager
    {
        private readonly MediaPlayer _mediaPlayer = new MediaPlayer();

        private DispatcherTimer _timer;

        /// <summary>
        /// Initialize the Songs variable
        /// </summary>
        /// <param name="songs"></param>
        public WpfMediaManager(List<Song> songs) : base(songs)
        {
            _mediaPlayer.Volume = 0;
        }

        /// <summary>
        /// Initializes the local mediaplayer
        /// </summary>
        /// <param name="currentSong"></param>
        protected override void InitializePlayer(Song currentSong, TimeSpan currentPosition = default)
        {
            _mediaPlayer.Open(new Uri(_currentSong.Path, UriKind.RelativeOrAbsolute));

            _currentSongPosition = currentPosition;
            _mediaPlayer.Position = _currentSongPosition;

            _mediaPlayer.MediaOpened += MediaOpened;
            _mediaPlayer.MediaEnded += MediaEnded;
            _mediaPlayer.Play();
        }

        /// <summary>
        /// Initializes the timer for the local mediaplayer
        /// </summary>
        protected override void InitializeTimer()
        {
            DispatcherTimer _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(1);
            _timer.Tick += Update;
            _timer.Start();
        }

        /// <summary>
        /// Starts playing a song in the mediaplayer
        /// </summary>
        /// <param name="song"></param>
        public override void Play()
        {
            base.Play();

            _mediaPlayer.Play();
        }

        /// <summary>
        /// Pauses a song in the mediaplayer
        /// </summary>
        public override void Pause()
        {
            _mediaPlayer.Pause();
        }

        /// <summary>
        /// Replays a song in the mediaplayer
        /// </summary>
        public override void Replay()
        {
            _mediaPlayer.Stop();
            _mediaPlayer.Play();
        }

        /// <summary>
        /// Closes the underlying media
        /// </summary>
        public override void Close()
        {
            _mediaPlayer.Close();
        }

        /// <summary>
        /// Returns the mediaplayer's current source
        /// </summary>
        /// <returns>Mediaplayer's source</returns>
        public override Uri GetSource()
        {
            return _mediaPlayer.Source;
        }

        /// <summary>
        /// Gets the local mediaplayer position.
        /// </summary>
        /// <returns></returns>
        protected override TimeSpan GetPlayerPosition()
        {
            return _mediaPlayer.Position;
        }

        /// <summary>
        /// Checks if the Natural duration of the local mediaplayer has a timespan
        /// </summary>
        /// <returns>True, if there is a timespan, False otherwise</returns>
        protected override bool NaturalDurationHasTimeSpan()
        {
            return _mediaPlayer.NaturalDuration.HasTimeSpan;
        }

        /// <summary>
        /// Gets the natural duration of the local MediaPlayer
        /// </summary>
        /// <returns></returns>
        protected override TimeSpan GetNaturalDuration()
        {
            return _mediaPlayer.NaturalDuration.TimeSpan;
        }

        /// <summary>
        /// Updates the position of the local mediaplayer.
        /// </summary>
        protected override void UpdateMediaPlayerPosition()
        {
            _currentSongPosition = _mediaPlayer.Position;
        }
    }
}