using Minify.Core.Managers;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Minify.Core.Test.Managers
{
    public class UserManagerTest
    {
        [Test]
        public void IsValidEmail_Return_IsTrue()
        {
            // create email
            string email = "test@unittest.com";

            // validate email, outcome should be true
            bool result = UserManager.IsValidEmail(email);
            Assert.IsTrue(result);
        }

        [Test]
        public void IsValidEmail_Return_IsFalse()
        {
            // create email
            string email = "test";

            // validate email, outcome should be false
            bool result = UserManager.IsValidEmail(email);
            Assert.IsFalse(result);
        }
    }
}
