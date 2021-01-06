using System;

namespace Minify.Core.Models
{
    public class UpdateHitlistMenuEventArgs : EventArgs
    {
        public Guid Id { get; }

        public UpdateHitlistMenuEventArgs(Guid id)
        {
            Id = id;
        }
    }
}