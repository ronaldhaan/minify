using Minify.Core.Controllers;
using Minify.DAL.Entities;

using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Minify.Core.Test
{
    public class SongControlerTest
    {
        private SongController _controller;
        private Guid testId;

        [SetUp]
        public void Setup()
        {
            _controller = new SongController();
            testId = new Guid("{aa5ab627-3b64-4c22-9cc3-cca5fd57c896}");
        }

        [Test]
        public void GetAll_NotNull()
        {
            List<Song> songs = _controller.GetAll();

            Assert.NotNull(songs);
        }

        [Test]
        public void Find__Random_Id_IsNull()
        {
            Guid randomId = new Guid();
            Song song = _controller.Get(randomId);

            Assert.IsNull(song);
        }

        [Test]
        public void Get_By_Id_Return_IsNotNull()
        {
            Song song = _controller.Get(testId);

            Assert.IsNotNull(song);
        }

        [Test]
        public void Search_Succesful()
        {
            List<Song> songs = _controller.Search("him");
            Assert.IsNotNull(songs);
        }

        [Test]
        public void Search_Fail()
        {
            List<Song> songs = _controller.Search("xxxxx");
            Assert.IsEmpty(songs);
        }
    }
}