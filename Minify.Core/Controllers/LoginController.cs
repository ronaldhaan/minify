using Castle.Core.Internal;
using Minify.DAL;
using Minify.DAL.Entities;
using Minify.DAL.Repositories;
using Minify.DAL.Managers;
using Minify.Core.Models;
using System;
using Minify.Core.Managers;

namespace Minify.Core.Controllers
{
    public class LoginController : IMinifySerializable
    {
        private AppData appData;

        /// <summary>
        /// Create a user repository with the context
        /// </summary>
        public LoginController()
        {
            appData = AppManager.Get<AppData>();
        }

        public bool IsSessionActive() => appData.IsSessionActive();

        /// <summary>
        /// Tries to login in with the given credentials.
        /// </summary>
        /// <param name="username">The username of the user</param>
        /// <param name="password">the password of the user</param>
        /// <returns>True, when the credentials are correct corresponding with the credentials in the database, False otherwise</returns>
        public bool TryLogin(string username, string password)
        {
            if (Validation(username, password) && !appData.LoggedIn)
            {
                User user = new Repository<User>().FindOneBy(u => u.UserName == username);
                
                if(user != null)
                {
                    appData.SetSession(user);
                    return true;
                }
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
            if (username.IsNullOrEmpty() || password.IsNullOrEmpty())
            {
                return false;
            }

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
        public void Logout() => appData.DestroySession();
    }
}