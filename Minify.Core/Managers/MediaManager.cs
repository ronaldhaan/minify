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
        private Song _currentSong;
        protected TimeSpan _currentSongPosition;
        private bool _paused;

        public List<Song> Songs { get; protected set; }

        public bool Paused { get => _paused; private set => _paused = value; }

        public virtual event EventHandler<UpdateMediaplayerEventArgs> UpdateMediaplayer;
        public virtual event EventHandler<UpdateMediaplayerEventArgs> OnPlay;
        public virtual event EventHandler OnPause;


        public Song CurrentSong
        {
            get => _currentSong;
            protected set
            {
                _currentSong = value;
                AppManager.Get<AppData>().CurrentSong = value;
            }
        }

        /// <summary>
        /// Updates the current position variable
        /// </summary>
        public virtual TimeSpan CurrentSongPosition
        {
            get => _currentSongPosition;
            protected set
            {
                _currentSongPosition = value;
                AppManager.Get<AppData>().CurrentSongPosition = value;
            }
        }

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

            if (song == CurrentSong)
            {
                return false;
            }

            int index = Songs.FindIndex(x => x.Id.Equals(CurrentSong.Id));
            int newIndex = up ? index + 1 : index - 1;
            CurrentSong = Songs[newIndex];
            Open(CurrentSong);
            Play();

            return true;
        }

        #region Virtual Methods

        public virtual void Open(List<Song> songs)
        {
            if (!Utility.ListIsNullOrEmpty(songs))
            {
                Songs = songs;
                Open(songs.FirstOrDefault());
            }
        }


        /// <summary>
        /// Opens a song in the mediaplayer
        /// </summary>
        /// <param name="song"></param>
        public virtual void Open(Song song, TimeSpan currentPosition = default)
        {
            CurrentSong = song;

            if (CurrentSong != null)
            {
                InitializePlayer(CurrentSong, currentPosition);

                InitializeTimer();
            }
        }

        /// <summary>
        /// Starts playing a song in the mediaplayer
        /// </summary>
        /// <param name="song"></param>
        public virtual void Play()
        {
            if (CurrentSong != null)
            {
                Paused = false;
                OnPlay?.Invoke(this, new UpdateMediaplayerEventArgs(CurrentSong, CurrentSongPosition));
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
        public virtual bool Next() => MoveTo(true);

        /// <summary>
        /// Starts playing the previous song in the mediaplayer
        /// </summary>
        /// <returns>Returns true if there is a previous song and false if there is no previous song</returns>
        public virtual bool Previous() => MoveTo(false);

        /// <summary>
        /// Invoke the event handler for updating the mediaplayer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Update(object sender, EventArgs e)
        {
            if (NaturalDurationHasTimeSpan() && CurrentSong != null)
            {
                _currentSongPosition = GetPlayerPosition();

                UpdateMediaplayer?.Invoke(this,
                    new UpdateMediaplayerEventArgs(
                        CurrentSong.Name,
                        CurrentSong.Artist,
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
            if (NaturalDurationHasTimeSpan() && CurrentSong != null)
            {
                UpdateMediaPlayerPosition();
                UpdateMediaplayer?.Invoke(this,
                    new UpdateMediaplayerEventArgs(
                        CurrentSong.Name,
                        CurrentSong.Artist,
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
            if (CurrentSong != null)
            {
                Close();

                if (Songs.Last() != CurrentSong)
                    Next();
                else
                    CurrentSong = Songs.First();

                UpdateMediaplayer?.Invoke(this,
                    new UpdateMediaplayerEventArgs()
                );
            }
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