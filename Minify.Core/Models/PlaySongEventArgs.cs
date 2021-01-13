using Minify.DAL.Entities;

using System;

namespace Minify.Core.Models
{
    public class PlaySongEventArgs : EventArgs
    {
        public Song Song { get; set; }
        public PlaySongEventArgs(Song selectedSong)
        {
            Song = selectedSong;
        }
    }
}
