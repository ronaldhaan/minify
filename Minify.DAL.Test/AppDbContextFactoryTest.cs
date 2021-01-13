using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Text;

namespace Minify.DAL.Test
{
    public class AppDbContextFactoryTest
    {
        private AppDbContextFactory appDbContextFactory;

        [SetUp]
        public void SetUp()
        {
            appDbContextFactory = new AppDbContextFactory();
        }

        [Test]
        public void CreateDbContext_Return_NotNull()
        {
            Assert.NotNull(appDbContextFactory.CreateDbContext());
        }

        [Test]
        public void GetConnectionString_Return_NotNullOrEmpty()
        {
            Assert.IsFalse(string.IsNullOrEmpty(Utility.GetConnectionString()));
        }
    }
}
