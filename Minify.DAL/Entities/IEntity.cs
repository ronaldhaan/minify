using System;

namespace Minify.DAL.Entities
{
    public interface IEntity
    {
        public Guid Id { get; set; }

        public DateTimeOffset CreatedAt { get; }

        public DateTimeOffset UpdatedAt { get; set; }
    }
}