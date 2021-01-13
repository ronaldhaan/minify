using Microsoft.EntityFrameworkCore;

using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.DAL.Repositories;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Minify.Core.Controllers
{
    public class MessageController : IMinifySerializable
    {
        private readonly AppData appData;

        /// <summary>
        /// Initialize an instance of <see cref="MessageController"/> class.
        /// </summary>
        public MessageController()
        {
            appData = AppManager.Get<AppData>();
        }

        /// <summary>
        /// Gets all the messages
        /// </summary>
        /// <param name="streamroom"></param>
        /// <returns></returns>
        public List<Message> GetMessages(Streamroom streamroom)
        {
            return new Repository<Message>()
                            .GetAll()
                                .Include(m => m.User)
                                .OrderBy(m => m.CreatedAt)
                                .Where(message => message.StreamroomId == streamroom.Id)
                                .ToList();
        }

        /// <summary>
        /// Gets one specific message by the Global Unique identifier(GUID)
        /// </summary>
        /// <param name="messageId">the id of the message</param>
        /// <returns>The message found if the message belongs to the signed in user, otherwise null</returns>
        public Message GetMessage(Guid messageId)
        {
            Message message = new Repository<Message>().Find(messageId);

            if (message == null)
            {
                return null;
            }

            return appData.BelongsEntityToUser(message.UserId) ? message : null;
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="message">The message to be created</param>
        /// <returns>True, when the message is created successfully, False otherwise</returns>
        public bool CreateMessage(Message message)
        {
            if (!appData.BelongsEntityToUser(message.UserId))
            {
                return false;
            }

            if (string.IsNullOrEmpty(message.Text) || message.Text.Length > 140)
            {
                return false;
            }

            if (message.StreamroomId == new Guid())
            {
                return false;
            }

            try
            {
                var repo = new Repository<Message>();
                repo.Add(message);
                return repo.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }

        /// <summary>
        /// Deletes a message.
        /// </summary>
        /// <param name="message">The message to be deleted</param>
        /// <returns>True, when the message is deleted successfully, False otherwise</returns>
        public bool DeleteMessage(Message message)
        {
            if (!appData.BelongsEntityToUser(message.UserId))
            {
                return false;
            }

            try
            {
                var repo = new Repository<Message>();
                repo.Remove(message);
                return repo.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                return false;
            }
        }
    }
}