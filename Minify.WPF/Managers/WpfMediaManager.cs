using Minify.DAL.Entities;
using Minify.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using System.Windows.Threading;
using Minify.Core.Managers;
using System.Timers;
using System.Threading;

namespace Minify.WPF.Managers
{
    public class WpfMediaManager : MediaManager
    {
        private const double INTERVAL = 1000;
        private readonly MediaPlayer _mediaPlayer;

        private DispatcherTimer _timer;

        public double Volume { get => _mediaPlayer.Volume; set => _mediaPlayer.Volume = value; }
        public TimeSpan Position { get => _mediaPlayer.Position; set => _mediaPlayer.Position = value; }

        /// <summary>
        /// Initialize the Songs variable
        /// </summary>
        /// <param name="songs"></param>
        public WpfMediaManager(List<Song> songs) : base(songs)
        {
            _mediaPlayer = new MediaPlayer();
            _mediaPlayer.MediaOpened += MediaOpened;
            _mediaPlayer.MediaEnded += MediaEnded;
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

            Play();
        }

        /// <summary>
        /// Initializes the timer for the local mediaplayer
        /// </summary>
        protected override void InitializeTimer()
        {
            _timer = new DispatcherTimer() { Interval = TimeSpan.FromMilliseconds(INTERVAL) };
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
            _mediaPlayer.Dispatcher.Invoke(new ThreadStart(() => _mediaPlayer.Play()));
        }

        /// <summary>
        /// Pauses a song in the mediaplayer
        /// </summary>
        public override void Pause()
        {
            base.Pause();
            _mediaPlayer.Dispatcher.Invoke(new ThreadStart(() => _mediaPlayer.Pause()));
        }

        /// <summary>
        /// Replays a song in the mediaplayer
        /// </summary>
        public override void Replay()
        {
            _mediaPlayer.Dispatcher.Invoke(new ThreadStart(() =>
            {
                Stop();
                Play();
            }));
        }

        /// <summary>
        /// Closes the underlying media
        /// </summary>
        public override void Close()
        {
            base.Close();
            _mediaPlayer.Dispatcher.Invoke(new ThreadStart(()=> _mediaPlayer.Close()));         
        }

        /// <summary>
        /// Stops the mediaplayer
        /// </summary>
        public override void Stop()
        {
            base.Stop();
            _mediaPlayer.Dispatcher.Invoke(new ThreadStart(() => _mediaPlayer.Stop()));
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
            _mediaPlayer.Dispatcher.Invoke(new ThreadStart(() => _currentSongPosition = _mediaPlayer.Position));
        }
    }
}