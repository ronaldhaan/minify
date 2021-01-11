using Minify.DAL.Managers;

using NUnit.Framework;

namespace Minify.DAL.Test
{
    public class PasswordManagerTest
    {
        private readonly string testPassword = "Test!123";
        private readonly string testPasswordHashed = "p+eUD89OFO/VOk+Ca1Qq+0w1pyp8A6maF/u/gQrH+Icp3GQp";

        [Test]
        public void HashPassword_Return_NotNull()
        {
            string hash = PasswordManager.HashPassword(testPassword);

            Assert.NotNull(hash);
        }

        [Test]
        public void ValidatePassword_Return_True()
        {
            bool hash = PasswordManager.ValidatePassword(testPassword, testPasswordHashed);

            Assert.IsTrue(hash);
        }

        [Test]
        public void ValidatePassword_Return_False()
        {
            bool hash = PasswordManager.ValidatePassword("AnderWachtwoord:)", testPasswordHashed);

            Assert.IsFalse(hash);
        }

        [Test]
        public void IsValidPassword_Return_IsTrue()
        {
            // create password
            string password = "Welcome01!";

            // validate password, outcome should be true
            bool result = PasswordManager.IsValidPassword(password);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidPassword_Return_IsFalse()
        {
            // create password
            string password = "test";

            // validate password, outcome should be false
            bool result = PasswordManager.IsValidPassword(password);
            Assert.IsFalse(result);
        }
    }
}