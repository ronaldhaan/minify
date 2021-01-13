using Minify.DAL.Entities;
using Minify.DAL.Repositories;

using NUnit.Framework;

namespace Minify.DAL.Test
{
    public class RepositoryTest
    {
        private Repository<User> _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = new Repository<User>();
        }

        [Test]
        public void GetAll_Return_NotNull()
        {
            Assert.NotNull(_repository.GetAll());
        }

    }
}
