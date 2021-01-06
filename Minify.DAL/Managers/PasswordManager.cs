using System;
using System.Security.Cryptography;

namespace Minify.DAL.Managers
{
    public class PasswordManager
    {
        /// <summary>
        /// Hashes a password
        /// </summary>
        /// <param name="password"></param>
        /// <returns>Returns hash</returns>
        public static string HashPassword(string password)
        {
            //Create the salt value with a cryptographic PRNG:
            byte[] salt;
            new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

            //Create the Rfc2898DeriveBytes and get the hash value:
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            //Combine the salt and password bytes for later use:
            byte[] hashBytes = new byte[36];
            Array.Copy(salt, 0, hashBytes, 0, 16);
            Array.Copy(hash, 0, hashBytes, 16, 20);

            //Turn the combined salt+hash into a string for storage
            string savedPasswordHash = Convert.ToBase64String(hashBytes);

            return savedPasswordHash;
        }

        /// <summary>
        /// Validate given password with hash
        /// </summary>
        /// <param name="input"></param>
        /// <param name="savedPasswordHash"></param>
        /// <returns>Returns if valid or not</returns>
        public static bool ValidatePassword(string input, string savedPasswordHash)
        {
            /* Extract the bytes */
            byte[] hashBytes = Convert.FromBase64String(savedPasswordHash);

            /* Get the salt */
            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            /* Compute the hash on the password the user entered */
            var pbkdf2 = new Rfc2898DeriveBytes(input, salt, 100000);
            byte[] hash = pbkdf2.GetBytes(20);

            /* Compare the results */
            for (int i = 0; i < 20; i++)
            {
                if (hashBytes[i + 16] != hash[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}