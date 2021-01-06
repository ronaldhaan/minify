using Minify.DAL;
using Minify.DAL.Entities;
using Minify.DAL.Repositories;
using Minify.DAL.Managers;

using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Minify.Core.Controllers
{
    public class RegisterController : IController
    {
        /// <summary>
        /// Create a user repository with the context
        /// </summary>
        public RegisterController()
        {
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

        /// <summary>
        /// Check if username is unique
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns if given username is unique</returns>
        public bool IsUniqueUsername(string username)
        {
            return !new Repository<User>().Any(u => u.UserName.Equals(username));
        }

        /// <summary>
        /// Check if email is valid (offical function from docs.microsoft.com)
        /// </summary>
        /// <param name="email"></param>
        /// <returns>Returns if given email is valid</returns>
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                static string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }

        /// <summary>
        /// Check if password equals confirm password
        /// </summary>
        /// <param name="password"></param>
        /// <param name="confirmPassword"></param>
        /// <returns>Returns if password equals confirmPassword</returns>
        public bool PasswordEqualsConfirmPassword(string password, string confirmPassword)
        {
            return password.Equals(confirmPassword);
        }

        /// <summary>
        /// Check if password is valid
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Returns if given password is valid</returns>
        public bool IsValidPassword(string password)
        {
            // check if minimal lenght is 8
            var hasLenght = new Regex(@".{8,}");

            // check if password contains a number
            var hasNumber = new Regex(@"(?=.*?[0-9])");

            // check if password contains a special char
            var hasSpecialChar = new Regex(@"(?=.*?[#?!@$%^&*-])");

            // return true or false
            return hasLenght.IsMatch(password) && hasNumber.IsMatch(password) && hasSpecialChar.IsMatch(password);
        }
    }
}