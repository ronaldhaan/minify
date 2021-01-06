using System;

namespace Minify.Core.Models
{
    public class IsPausedEventArgs : EventArgs
    {
        public bool IsPaused { get; set; }
    }
}