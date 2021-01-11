using Microsoft.EntityFrameworkCore;

using Minify.DAL.Entities;
using Minify.DAL.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minify.Core.Controllers
{
    public class UserController : IController
    {

        public UserController()
        {

        }

        /// <summary>
        /// Gets all the hitlists available.
        /// </summary>
        /// <param name="withRelations">If true, all the songs and the user data will be included in the list, false otherwise</param>
        /// <returns>A list with all the hitlists</returns>
        public List<User> GetAll()
        {
            return new Repository<User>().GetAll().ToList();
        }

        /// <summary>
        /// Get hitlist by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="withRelations"></param>
        /// <returns></returns>
        public User Get(Guid id)
        {
            if (Utility.GuidIsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            return new Repository<User>().GetAll().Where(x => x.Id == id).FirstOrDefault();
        }

        public bool Update(User user)
        {
            var repo = new Repository<User>();
            repo.Update(user);
            return repo.SaveChanges() > 0;
        }
    }
}
