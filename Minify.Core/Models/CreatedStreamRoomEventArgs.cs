using Minify.DAL.Entities;

using System;

namespace Minify.Core.Models
{
    public class CreatedStreamRoomEventArgs : EventArgs
    {
        public Streamroom Streamroom { get; set; }
    }
}