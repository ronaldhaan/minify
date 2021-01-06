using Minify.Core.Controllers;
using NUnit.Framework;

namespace Minify.Core.Test
{
    public class RegisterControllerTest
    {
        private RegisterController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new RegisterController();
        }

        [Test]
        public void IsUniqueUsername_Return_IsTrue()
        {
            // create username
            string username = "uniqueunittestuser";

            // validate username, outcome should be true
            bool result = _controller.IsUniqueUsername(username);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsUniqueUsername_Return_IsFalse()
        {
            // create username
            string username = "testuser";

            // validate username, outcome should be false
            bool result = _controller.IsUniqueUsername(username);
            Assert.IsFalse(result);
        }

        [Test]
        public void IsValidEmail_Return_IsTrue()
        {
            // create email
            string email = "test@unittest.com";

            // validate email, outcome should be true
            bool result = _controller.IsValidEmail(email);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidEmail_Return_IsFalse()
        {
            // create email
            string email = "test";

            // validate email, outcome should be false
            bool result = _controller.IsValidEmail(email);
            Assert.IsFalse(result);
        }

        [Test]
        public void PasswordEqualsConfirmPassword_Return_IsTrue()
        {
            // create matching passwords
            string password = "Welcome01!";
            string confirmPassword = "Welcome01!";

            // check if passwords are equal, outcome should be true
            Assert.IsTrue(_controller.PasswordEqualsConfirmPassword(password, confirmPassword));
        }

        [Test]
        public void PasswordEqualsConfirmPassword_Return_IsFalse()
        {
            // create matching passwords
            string password = "Welcome01!";
            string confirmPassword = "Welcome02!";

            // check if passwords are equal, outcome should be false
            Assert.IsFalse(_controller.PasswordEqualsConfirmPassword(password, confirmPassword));
        }

        [Test]
        public void IsValidPassword_Return_IsTrue()
        {
            // create password
            string password = "Welcome01!";

            // validate password, outcome should be true
            bool result = _controller.IsValidPassword(password);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidPassword_Return_IsFalse()
        {
            // create password
            string password = "test";

            // validate password, outcome should be false
            bool result = _controller.IsValidPassword(password);
            Assert.IsFalse(result);
        }
    }
}