using Castle.Core.Internal;
using Minify.DAL;
using Minify.DAL.Entities;
using Minify.DAL.Repositories;
using Minify.DAL.Managers;
using Minify.Core.Models;
using System;

namespace Minify.Core.Controllers
{
    public class LoginController : IController
    {

        /// <summary>
        /// Create a user repository with the context
        /// </summary>
        public LoginController()
        {
        }

        /// <summary>
        /// Tries to login in with the given credentials.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">the password of the user</param>
        /// <returns>True, when the credentials are correct corresponding with the credentials in the database, False otherwise</returns>
        public bool TryLogin(string username, string password)
        {
            if (Validation(username, password) && !AppData.LoggedIn)
            {
                AppData.LoggedIn = true;
                User user = new Repository<User>().FindOneBy(u => u.UserName == username);
                AppData.UserId = user.Id;
                AppData.UserName = user.UserName;

                return true;
            }

            return false;
        }

        /// <summary>
        /// It validate the credintials
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">the password of the user</param>
        /// <returns>True, when the credentials are correct corresponding with the credentials in the database, False otherwise</returns>
        public bool Validation(string username, string password)
        {
            // check if username is null or empty
            if (username.IsNullOrEmpty())
                return false;

            // check if password is null or empty
            if (password.IsNullOrEmpty())
                return false;

            // check if password is valid
            foreach (User user in new Repository<User>().GetAll())
            {
                if (user.UserName == username && PasswordManager.ValidatePassword(password, user.PassWord))
                {
                    return true;
                }
            }

            return false;
        }

        //Logs out the current user
        public void Logout()
        {
            AppData.LoggedIn = false;
            AppData.UserId = Guid.Empty;
        }
    }
}