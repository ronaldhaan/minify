using Minify.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Text;

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
