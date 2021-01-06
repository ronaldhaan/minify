using System;
using System.Collections.Generic;

namespace Minify.DAL.Entities
{
    public class Song : BaseEntity
    {
        public string Name { get; set; }

        public string Genre { get; set; }

        public string Artist { get; set; }

        public TimeSpan Duration { get; set; }

        public string Path { get; set; }

        public ICollection<HitlistSong> Hitlists { get; set; }
    }
}