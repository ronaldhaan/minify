﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Minify.DAL.Entities
{
    public class Message : BaseEntity
    {
        [Required]
        public Guid StreamroomId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        [Required, MaxLength(140)]
        public string Text { get; set; }

        public User User { get; set; }
    }
}