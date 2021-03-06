﻿using Minify.DAL.Entities;

using System;

namespace Minify.Core.Models
{
    public class UpdateMediaplayerEventArgs : EventArgs
    {
        public string SongName { get; }
        public string Artist { get; }
        public TimeSpan Position { get; }
        public TimeSpan Duration { get; }

        public UpdateMediaplayerEventArgs()
        {
        }

        public UpdateMediaplayerEventArgs(Song song, TimeSpan currentSongPostion) : this(song.Name, song.Artist, currentSongPostion, song.Duration) { }

        public UpdateMediaplayerEventArgs(string songName, string artist, TimeSpan position, TimeSpan duration)
        {
            SongName = songName;
            Artist = artist;
            Position = position;
            Duration = duration;
        }
    }
}