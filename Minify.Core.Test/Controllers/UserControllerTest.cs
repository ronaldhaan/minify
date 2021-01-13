using Minify.Core.Controllers;

using NUnit.Framework;

namespace Minify.Core.Test
{
    public class UserControllerTest
    {
        private UserController _controller;

        [SetUp]
        public void Setup()
        {
            _controller = new UserController();
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
    }
}