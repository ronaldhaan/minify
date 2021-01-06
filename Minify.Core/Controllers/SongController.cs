using Microsoft.EntityFrameworkCore;
using Minify.DAL;
using Minify.DAL.Entities;
using Minify.DAL.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Minify.Core.Controllers
{
    public class SongController : IController
    {
        /// <summary>
        /// Create a sog repository with the context
        /// </summary>
        public SongController()
        {
        }

        /// <summary>
        /// Get all songs
        /// </summary>
        /// <returns>returns list of songs</returns>
        public List<Song> GetAll()
        {
            var query = new Repository<Song>().GetAll().AsNoTracking();
            return query.ToList();
        }

        /// <summary>
        /// Gets all songs by name, artist or genre
        /// </summary>
        /// <returns></returns>
        public List<Song> Search(string searchquery)
        {
            var query = new Repository<Song>().GetAll();

            var likeSearch = $"%{searchquery}%";

            query = query.Where(s =>
                EF.Functions.Like(s.Name.ToUpper(), likeSearch.ToUpper()) ||
                EF.Functions.Like(s.Artist.ToUpper(), likeSearch.ToUpper()) ||
                EF.Functions.Like(s.Genre.ToUpper(), likeSearch.ToUpper())
            );

            return query.ToList();
        }

        /// <summary>
        /// Gets a <see cref="Song"/> by the <see cref="Guid"/> id.
        /// </summary>
        /// <param name="id">The <see cref="Guid"/> id of the song</param>
        /// <returns>The song found, or null</returns>
        public Song Get(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return new Repository<Song>().Find(id);
        }
    }
}