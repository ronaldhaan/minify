using Minify.Core.Models;
using Minify.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Minify.Core.Managers
{

    /// <summary>
    /// 
    /// </summary>
    public abstract class MediaManager
    {
        protected Song _currentSong;
        protected TimeSpan _currentSongPosition;
        private bool _paused;

        public List<Song> Songs { get; set; }

        public bool Paused { get => _paused; private set => _paused = value; }

        public virtual event EventHandler<UpdateMediaplayerEventArgs> UpdateMediaplayer;
        public virtual event EventHandler<UpdateMediaplayerEventArgs> OnPlay;
        public virtual event EventHandler OnPause;

        /// <summary>
        /// Updates the current position variable
        /// </summary>
        public virtual TimeSpan CurrentSongPosition { get => _currentSongPosition; set => _currentSongPosition = value; }

        /// <summary>
        /// Initialize the Songs variable
        /// </summary>
        /// <param name="songs"></param>
        public MediaManager(List<Song> songs)
        {
            Songs = songs;
            Paused = false;
        }

        /// <summary>
        /// Moves the current song to the next or the previous.
        /// </summary>
        /// <param name="up"></param>
        /// <returns></returns>
        private bool MoveTo(bool up = true)
        {
            Stop();

            if (Songs == null)
            {
                return false;
            }

            Song song = up ? Songs.Last() : Songs.First();

            if (song == _currentSong)
            {
                Stop();
                return false;
            }

            int index = Songs.FindIndex(x => x == _currentSong);
            int newIndex = up ? index + 1 : index - 1;
            _currentSong = Songs[newIndex];
            Play();

            return true;
        }

        #region Virtual Methods

        /// <summary>
        /// Opens a song in the mediaplayer
        /// </summary>
        /// <param name="song"></param>
        public virtual void Open(Song song, TimeSpan currentPosition = default)
        {
            _currentSong = song;

            if (_currentSong != null)
            {
                InitializePlayer(_currentSong, currentPosition);

                InitializeTimer();
            }
        }

        /// <summary>
        /// Starts playing a song in the mediaplayer
        /// </summary>
        /// <param name="song"></param>
        public virtual void Play()
        {
            if (_currentSong != null)
            {
                Paused = false;
                OnPlay?.Invoke(this, new UpdateMediaplayerEventArgs(_currentSong, CurrentSongPosition));
            }
        }

        /// <summary>
        /// Pauses a song in the mediaplayer
        /// </summary>
        public virtual void Pause()
        {
            Paused = true;
            OnPause?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Stops the mediaplayer.
        /// </summary>
        public virtual void Stop()
        {
            Paused = false;
        }

        /// <summary>
        /// Closes the underlying media
        /// </summary>
        public virtual void Close()
        {
            Paused = false;
        }

        /// <summary>
        /// Starts playing the next song in the mediaplayer
        /// </summary>
        /// <returns>Returns true if there is a next song and false if there is no next song</returns>
        public virtual bool Next()
        {
            return MoveTo();
        }

        /// <summary>
        /// Starts playing the previous song in the mediaplayer
        /// </summary>
        /// <returns>Returns true if there is a previous song and false if there is no previous song</returns>
        public virtual bool Previous()
        {
            return MoveTo(false);
        }

        /// <summary>
        /// Returns the mediaplayer's current song
        /// </summary>
        /// <returns>Mediaplayer's song</returns>
        public virtual Song GetCurrentSong()
        {
            return _currentSong;
        }

        /// <summary>
        /// Invoke the event handler for updating the mediaplayer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Update(object sender, EventArgs e)
        {
            if (NaturalDurationHasTimeSpan())
            {
                _currentSongPosition = GetPlayerPosition();

                UpdateMediaplayer?.Invoke(this,
                    new UpdateMediaplayerEventArgs(
                        _currentSong.Name,
                        _currentSong.Artist,
                        _currentSongPosition,
                        GetNaturalDuration()
                    )
                );
            }
        }

        /// <summary>
        /// Event handler when media is opened
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MediaOpened(object sender, EventArgs e)
        {
            if (NaturalDurationHasTimeSpan())
            {
                UpdateMediaPlayerPosition();
                UpdateMediaplayer?.Invoke(this,
                    new UpdateMediaplayerEventArgs(
                        _currentSong.Name,
                        _currentSong.Artist,
                        CurrentSongPosition,
                        GetNaturalDuration()
                    )
                );
            }
        }

        /// <summary>
        /// Event handler to start the next song if the song ended
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void MediaEnded(object sender, EventArgs e)
        {
            Close();

            if (Songs.Last() != _currentSong)
                Next();
            else
                _currentSong = Songs.First();

            UpdateMediaplayer?.Invoke(this,
                new UpdateMediaplayerEventArgs()
            );
        }

        #endregion Virtual Methods

        #region Abstract Methods

        /// <summary>
        /// Initializes the timer for the local mediaplayer
        /// </summary>
        protected abstract void InitializeTimer();

        /// <summary>
        /// Initializes the local mediaplayer
        /// </summary>
        /// <param name="currentSong"></param>
        protected abstract void InitializePlayer(Song currentSong, TimeSpan currentPosition = default);

        /// <summary>
        /// Replays a song in the mediaplayer
        /// </summary>
        public abstract void Replay();

        /// <summary>
        /// Returns the mediaplayer's current source
        /// </summary>
        /// <returns>Mediaplayer's source</returns>
        public abstract Uri GetSource();

        /// <summary>
        /// Gets the local mediaplayer position.
        /// </summary>
        /// <returns></returns>
        protected abstract TimeSpan GetPlayerPosition();

        /// <summary>
        /// Checks if the Natural duration of the local mediaplayer has a timespan
        /// </summary>
        /// <returns>True, if there is a timespan, False otherwise</returns>
        protected abstract bool NaturalDurationHasTimeSpan();

        /// <summary>
        /// Gets the natural duration of the local MediaPlayer
        /// </summary>
        /// <returns></returns>
        protected abstract TimeSpan GetNaturalDuration();

        /// <summary>
        /// Updates the position of the local mediaplayer.
        /// </summary>
        protected abstract void UpdateMediaPlayerPosition();
        #endregion Abstract Methods
    }
}