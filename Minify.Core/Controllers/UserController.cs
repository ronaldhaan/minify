using Microsoft.EntityFrameworkCore;

using Minify.DAL.Entities;
using Minify.DAL.Managers;
using Minify.DAL.Repositories;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Minify.Core.Controllers
{
    public class UserController : IMinifySerializable
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

        /// <summary>
        /// Adds a user to the database
        /// </summary>
        /// <param name="user"></param>
        public void Add(User user)
        {
            if (user.Id == null)
                throw new ArgumentNullException("id");

            user.PassWord = PasswordManager.HashPassword(user.PassWord);

            var repo = new Repository<User>();
            repo.Add(user);
            repo.SaveChanges();
        }

        public bool Update(User user)
        {
            var repo = new Repository<User>();
            repo.Update(user);
            return repo.SaveChanges() > 0;
        }


        /// <summary>
        /// Check if username is unique
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns if given username is unique</returns>
        public bool IsUniqueUsername(string username)
        {
            return !new Repository<User>().Any(u => u.UserName.Equals(username));
        }
    }
}
