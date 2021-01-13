using Minify.Core.Controllers;
using Minify.DAL.Entities;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Minify.Core.Test
{
    public class StreamroomControllerTest
    {
        private StreamroomController _controller;
        private Guid testId;

        [SetUp]
        public void Setup()
        {
            _controller = new StreamroomController();
            testId = new Guid("{197a232b-4bb7-4961-9153-81349df9d785}");
        }

        [Test]
        public void GetAll_NotNull()
        {
            List<Streamroom> streamrooms = _controller.GetAll();

            Assert.NotNull(streamrooms);
        }

        [Test]
        public void GetAll_Count_Greater_Than_Or_Equal_To_Zero()
        {
            List<Streamroom> streamrooms = _controller.GetAll();

            Assert.GreaterOrEqual(streamrooms.Count, 0);
        }

        [Test]
        public void Find__Random_Id_IsNull()
        {
            Guid randomId = Guid.NewGuid();
            Streamroom streamroom = _controller.Get(randomId);

            Assert.IsNull(streamroom);
        }

        [Test]
        public void Find_Return_IsNotNull()
        {
            Streamroom streamroom = _controller.Get(testId);

            Assert.IsNotNull(streamroom);
        }

        [Test]
        public void Find_WithRelation_False_Song_IsNull()
        {
            Streamroom streamroom = _controller.Get(testId);

            Assert.IsNull(streamroom.Song);
        }

        [Test]
        public void Find_WithRelation_False_Hitlist_IsNull()
        {
            Streamroom streamroom = _controller.Get(testId);

            Assert.IsNull(streamroom.Hitlist);
        }

        [Test]
        public void Find_WithRelation_True_Song_IsNotNull()
        {
            Streamroom streamroom = _controller.Get(testId, true);

            Assert.IsNotNull(streamroom.Song);
        }

        [Test]
        public void Find_WithRelation_True_Hitlist_IsNotNull()
        {
            Streamroom streamroom = _controller.Get(testId, true);

            Assert.IsNotNull(streamroom.Hitlist);
        }
    }
}