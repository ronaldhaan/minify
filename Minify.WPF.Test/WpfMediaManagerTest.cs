using Minify.Core.Controllers;
using Minify.DAL.Entities;
using Minify.WPF.Managers;

using NUnit.Framework;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Minify.WPF.Test
{
    [TestFixture]
    public class WpfMediaManagerTest
    {
        private HitlistController _hitlistController;
        private SongController _songController;
        private Hitlist _hitlist;

        [SetUp]
        public void SetUp()
        {
            _hitlistController = new HitlistController();
            _songController = new SongController();
            _hitlist = _hitlistController.Get(new Guid("aa4cb653-3c62-5e22-5cc3-cca5fd57c846"), true);
        }

        [Test]
        public void Initialize_Songs_NotNull()
        {
            List<Song> songs = _hitlistController.GetSongs(_hitlist.Songs);
            WpfMediaManager _mediaManager = new WpfMediaManager(songs);
            Assert.NotNull(_mediaManager.Songs);
        }

        [Test]
        public void Play_ValidSong_ShouldOpen()
        {
            WpfMediaManager _mediaManager = new WpfMediaManager(null);
            _mediaManager.Close();
            Song song = _songController.Get(new Guid("aa5ab627-3b64-4c22-9cc3-cca5fd57c896"));
            _mediaManager.Open(song);
            _mediaManager.Play();
            Assert.IsTrue(_mediaManager.GetSource() != null);
        }

        [Test]
        public void Play_InvalidSong_ShouldNotOpen()
        {
            Song song = _songController.Get(new Guid("{aa5ab677-3b64-4c22-9cc3-cca5fd57c896}"));
            WpfMediaManager _mediaManager = new WpfMediaManager(new List<Song> { song });
            _mediaManager.Play();
            Assert.IsTrue(_mediaManager.GetSource() == null);
        }

        [Test]
        public void Next_SongsEmpty_IsFalse()
        {
            var manager = new WpfMediaManager(null);
            Assert.IsFalse(manager.Next());
        }

        [Test]
        public void Next_SongAvailable_IsTrue()
        {
            var manager = new WpfMediaManager(new List<Song> { _songController.Get(new Guid("{aa5ab677-3b64-4c22-9cc3-cca5fd57c896}")) });

            Assert.IsTrue(manager.Next());
        }

        [Test]
        public void Next_SongUnavailable_IsFalse()
        {
            var manager = new WpfMediaManager(new List<Song> { _songController.Get(new Guid("{aa5ab677-3b64-4c22-9cc3-cca5fd57c896}")) });

            manager.Next();
            Assert.IsFalse(manager.Next());
        }

        [Test]
        public void Previous_SongsEmpty_IsFalse()
        {
            var manager = new WpfMediaManager(null);
            Assert.IsFalse(manager.Previous());
        }

        [Test]
        public void Previous_SongAvailable_IsTrue()
        {
            List<Song> songs = _hitlistController.GetSongs(_hitlist.Songs);
            WpfMediaManager manager = new WpfMediaManager(songs);
            manager.Open(songs.First());
            manager.Play();
            manager.Next();
            Assert.IsTrue(manager.Previous());
        }

        [Test]
        public void Previous_SongUnavailable_IsFalse()
        {
            List<Song> songs = _hitlistController.GetSongs(_hitlist.Songs);
            WpfMediaManager manager = new WpfMediaManager(songs);
            manager.Open(songs.First());
            manager.Play();
            Assert.IsFalse(manager.Previous());
        }

        [Test]
        public void GetCurrentSong_SongAvailable_IsNotNull()
        {
            List<Song> songs = _hitlistController.GetSongs(_hitlist.Songs);
            WpfMediaManager manager = new WpfMediaManager(songs);
            manager.Open(songs.First());
            manager.Play();
            Assert.IsNotNull(manager.GetCurrentSong());
        }

        [Test]
        public void GetCurrentSong_SongUnavailable_IsNull()
        {
            List<Song> songs = _hitlistController.GetSongs(_hitlist.Songs);
            WpfMediaManager manager = new WpfMediaManager(songs);
            manager.Open(null);
            Assert.IsNull(manager.GetCurrentSong());
        }
    }
}