using System;
using System.ComponentModel.DataAnnotations;

namespace Minify.DAL.Entities
{
    public class Streamroom : BaseEntity
    {
        [Required]
        public Guid HitlistId { get; set; }

        [Required]
        public Guid CurrentSongId { get; set; }

        [Required]
        public TimeSpan CurrentSongPosition { get; set; }

        [Required]
        public bool IsPaused { get; set; }

        public Song Song { get; set; }

        public Hitlist Hitlist { get; set; }

        public Streamroom()
        {
        }

        public Streamroom(Guid hitlistId, Guid currentSongId)
        {
            HitlistId = hitlistId;
            CurrentSongId = currentSongId;
            CurrentSongPosition = new TimeSpan(0, 0, 0);
            IsPaused = false;
        }
    }
}