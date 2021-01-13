using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;

using NUnit.Framework;

using System;
using System.Collections.Generic;

namespace Minify.Core.Test
{
    public class MessageControllerTest
    {
        private MessageController messageController;
        private Guid streamRoomIdTest, userIdTest, messageIdTest;
        private Streamroom streamroomTest;
        private AppData appData;


        [SetUp]
        public void SetUp()
        {
            messageController = new MessageController();
            streamRoomIdTest = new Guid("{197a232b-4bb7-4961-9153-81349df9d785}");
            userIdTest = new Guid("{aa5ab627-3b64-5d22-8cc3-cca5fd57c896}");
            messageIdTest = new Guid("{197a232b-4bb8-4961-9264-81349df9d785}");

            streamroomTest = new Streamroom { Id = streamRoomIdTest };
            appData = new AppData { UserId = userIdTest };

            AppManager.Initialize();
            AppManager.Add(appData);
        }

        [Test]
        public void GetAll_NotNull()
        {
            List<Message> messages = messageController.GetMessages(streamroomTest);

            Assert.NotNull(messages);
        }

        [Test]
        public void Find__Random_Id_IsNull()
        {
            Guid randomId = new Guid();
            Message message = messageController.GetMessage(randomId);

            Assert.IsNull(message);
        }

        [Test]
        public void Get_By_Id_Return_IsNotNull()
        {
            Message message = messageController.GetMessage(messageIdTest);

            Assert.IsNotNull(message);
        }

        [Test]
        public void CreateMessage_Successfull_Return_True()
        {
            Message message = new Message()
            {
                Text = "Test",
                StreamroomId = streamRoomIdTest,
                UserId = userIdTest,
            };

            Assert.IsTrue(messageController.CreateMessage(message));
        }

        [Test]
        public void CreateMessage_StreamRoomId_Is_Null_Return_False()
        {
            Message message = new Message()
            {
                Text = "Test",
                UserId = userIdTest,
            };

            Assert.IsFalse(messageController.CreateMessage(message));
        }

        [Test]
        public void CreateMessage_UserId_Is_Null_Return_False()
        {
            Message message = new Message()
            {
                Text = "Test",
                StreamroomId = streamRoomIdTest,
            };

            Assert.IsFalse(messageController.CreateMessage(message));
        }

        [Test]
        public void CreateMessage_Text_Is_Null_Or_Empty_Return_False()
        {
            Message message = new Message()
            {
                UserId = userIdTest,
                StreamroomId = streamRoomIdTest,
            };

            Assert.IsFalse(messageController.CreateMessage(message));
        }
    }
}