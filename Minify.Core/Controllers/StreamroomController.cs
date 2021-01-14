using Microsoft.EntityFrameworkCore;

using Minify.DAL.Entities;
using Minify.DAL.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Minify.Core.Controllers
{
    public class StreamroomController : IMinifySerializable
    {
        /// <summary>
        /// Create a streamroom repository with the context
        /// </summary>
        public StreamroomController()
        {
        }

        /// <summary>
        /// Get streamroom by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="withRelations"></param>
        /// <returns></returns>
        public Streamroom Get(Guid id, bool withRelations = false)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var query = new Repository<Streamroom>().GetAll();

            if (withRelations)
            {
                query = query
                    .Include(s => s.Song)
                    .Include(s => s.Hitlist)
                        .ThenInclude(h => h.User)
                            .ThenInclude(u => u.Person)
                    .Include(s => s.Hitlist)
                        .ThenInclude(h => h.Songs)
                            .ThenInclude(hs => hs.Song);
            }

            return query.Where(x => x.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Gets all the streamrooms available.
        /// </summary>
        /// <param name="withRelations">If true, the song and the user data will be included in the list, false otherwise</param>
        /// <returns>A list with all the hitlists</returns>
        public List<Streamroom> GetAll(bool withRelations = false)
        {
            var query = new Repository<Streamroom>().GetAll();

            if (withRelations)
            {
                query = query
                    .Include(s => s.Song)
                    .Include(s => s.Hitlist)
                        .ThenInclude(h => h.User)
                    .Include(s => s.Hitlist)
                        .ThenInclude(h => h.Songs)
                            .ThenInclude(hs => hs.Song);
            }

            return query.ToList();
        }

        /// <summary>
        /// Get streamroom by hitlist id
        /// </summary>
        /// <param name="hitlistId"></param>
        /// <param name="withRelations"></param>
        /// <returns></returns>
        public Streamroom GetStreamroomByHitlistId(Guid hitlistId, bool withRelations = false)
        {
            if (hitlistId == Guid.Empty)
            {
                throw new ArgumentException(nameof(hitlistId));
            }

            var query = new Repository<Streamroom>()
                            .GetAll()
                            .Where(x => x.HitlistId == hitlistId);

            if (withRelations)
            {
                query = query
                    .Include(s => s.Song)
                    .Include(s => s.Hitlist);
            }

            return query.Where(x => x.HitlistId == hitlistId).FirstOrDefault();
        }

        /// <summary>
        /// Adds a streamroom to the database
        /// </summary>
        /// <param name="streamroom"></param>
        public bool Add(Streamroom streamroom)
        {
            if (streamroom.Id == null)
                throw new ArgumentNullException("id");

            var repo = new Repository<Streamroom>();
            repo.Add(streamroom);
            return repo.SaveChanges() > 0;
        }

        /// <summary>
        /// Adds a streamroom to the database
        /// </summary>
        /// <param name="streamroom"></param>
        public bool Update(Streamroom streamroom)
        {
            if (streamroom.Id == null)
                throw new ArgumentNullException("id");

            var repo = new Repository<Streamroom>();
            repo.Update(streamroom);
            return repo.SaveChanges() > 0;
        }

        public void Pause(Streamroom streamroom)
        {
            if (streamroom.Id == null)
                throw new ArgumentNullException("id");

            streamroom.IsPaused = true;
            Update(streamroom);
        }

        public void Play(Streamroom streamroom)
        {
            if (streamroom.Id == null)
                throw new ArgumentNullException("id");

            streamroom.IsPaused = false;
            Update(streamroom);
        }

        public bool IsPaused(Streamroom streamroom)
        {
            if (streamroom.Id == null)
                throw new ArgumentNullException("id");

            return Get(streamroom.Id).IsPaused;
        }

        /// <summary>
        /// Check if streamroom with given hitlist id already exist
        /// </summary>
        /// <param name="hitlistId"></param>
        /// <returns>Returns true ot false</returns>
        public bool DoesRoomAlreadyExist(Guid hitlistId)
        {
            return GetStreamroomByHitlistId(hitlistId) != null;
        }
    }
}