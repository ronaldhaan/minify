using System;

namespace Minify.DAL.Entities
{
    public class HitlistSong : BaseEntity
    {
        public Guid SongId { get; set; }

        public Guid HitlistId { get; set; }
        public Hitlist Hitlist { get; set; }

        public Song Song { get; set; }
    }
}