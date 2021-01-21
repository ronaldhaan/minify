using Minify.DAL.Entities;

using System;
using System.Collections.Generic;

namespace Minify.Core.Models
{
    public class PlaySongEventArgs : EventArgs
    {
        public Song Song { get; set; }

        public TimeSpan CurrentSongPosition { get; set; }
        public List<Song> Songs { get; set; }

        public PlaySongEventArgs(Song song) : this(song, new List<Song> { song }) { }

        public PlaySongEventArgs(Song song, List<Song> songs)
        {
            Song = song;
            Songs = songs;
        }
    }
}
