using Minify.Core.Controllers;
using Minify.DAL.Entities;

using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Minify.Core.Test
{
    public class HitlistControllerTest
    {
        private HitlistController _controller;
        private Guid testId;

        [SetUp]
        public void Setup()
        {
            _controller = new HitlistController();
            testId = new Guid("{aa4cb653-3c62-5e22-5cc3-cca5fd57c846}");
        }

        [Test]
        public void GetAll_NotNull()
        {
            List<Hitlist> hitlists = _controller.GetAll();

            Assert.NotNull(hitlists);
        }

        [Test]
        public void GetAll_Count_Greater_Than_Or_Equal_To_Zero()
        {
            List<Hitlist> hitlists = _controller.GetAll();

            Assert.GreaterOrEqual(hitlists.Count, 0);
        }

        [Test]
        public void Find__Random_Id_IsNull()
        {
            Guid randomId = Guid.NewGuid();
            Hitlist hitlist = _controller.Get(randomId);

            Assert.IsNull(hitlist);
        }

        [Test]
        public void Find_Return_IsNotNull()
        {
            Hitlist hitlist = _controller.Get(testId);

            Assert.IsNotNull(hitlist);
        }

        [Test]
        public void GetHitlistInfo_WithSongsReturn_AreEqual()
        {
            // arrange
            Hitlist hitlist = _controller.Get(new Guid("aa4cb653-3c62-5e22-5cc3-cca5fd57c846"), true);
            string expected = $"Created by 1140207 at {hitlist.CreatedAt:dd/MM/yyyy} - 2 songs, 8 min 10 sec"; ;

            // act
            string result = _controller.GetHitlistInfo(hitlist);

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetHitlistInfo_WithoutSongsReturn_AreEqual()
        {
            // arrange
            Hitlist hitlist = _controller.Get(new Guid("aa4cb653-3c62-5522-5cc3-cca5fd57c846"), true);
            string expected = $"Created by testuser at {hitlist.CreatedAt:dd/MM/yyyy} - This hitlist doesn't contain any songs yet";

            // act
            string result = _controller.GetHitlistInfo(hitlist);

            // assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Find_WithRelation_False_User_IsNull()
        {
            Hitlist hitlist = _controller.Get(testId);

            Assert.IsNull(hitlist.User);
        }

        [Test]
        public void Find_WithRelation_False_Songs_IsNull()
        {
            Hitlist hitlist = _controller.Get(testId);

            Assert.IsNull(hitlist.Songs);
        }

        [Test]
        public void Find_WithRelation_True_User_IsNotNull()
        {
            Hitlist hitlist = _controller.Get(testId, true);

            Assert.IsNotNull(hitlist.User);
        }

        [Test]
        public void Find_WithRelation_True_Songs_IsNotNull()
        {
            Hitlist hitlist = _controller.Get(testId, true);

            Assert.IsNotNull(hitlist.Songs);
        }

        [Test]
        public void Validation_Tiltle_ReturnFalse()
        {
            // Assemble
            string title = "";
            // Act
            var ValidationTitle = _controller.ValidateTitle(title);

            // Assert
            Assert.IsFalse(ValidationTitle);
        }

        [Test]
        public void Validation_Tiltle_ReturnTrue()
        {
            // Assemble
            string title = "HuhHuh";
            // Act
            var ValidationTitle = _controller.ValidateTitle(title);

            // Assert
            Assert.IsTrue(ValidationTitle);
        }

        [Test]
        public void Validation_Description_ReturnTrue()
        {
            // Assemble
            string description = "HuhHuh";
            // Act
            var ValidationDescription = _controller.ValidateDescription(description);

            // Assert
            Assert.IsTrue(ValidationDescription);
        }

        [Test]
        public void Validation_Description_ReturnFalse()
        {
            // Assemble
            string description = "HuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhH" +
                "HuhHuhHuhHHuhHuhHuhHHuhHuhHuhHHuhHuhHuhH1"
                ;
            // Act
            var ValidationDescription = _controller.ValidateDescription(description);

            // Assert
            Assert.IsFalse(ValidationDescription);
        }
    }
}