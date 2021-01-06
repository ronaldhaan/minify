using Minify.DAL;
using Minify.DAL.Entities;
using Minify.DAL.Repositories;
using Minify.Core.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Minify.Core.Controllers
{
    public class MessageController : IController
    {
        /// <summary>
        /// Initialize an instance of <see cref="MessageController"/> class.
        /// </summary>
        public MessageController() : this(new AppDbContextFactory().CreateDbContext()) { }

        /// <summary>
        /// Initialize an instance of <see cref="MessageController"/> class with an <see cref="AppDbContext"/> instance.
        /// </summary>
        /// <param name="context">The <see cref="AppDbContext"/> instance this controller will work with</param>
        public MessageController(AppDbContext context)
        {
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

            return BelongsEntityToUser(message.UserId) ? message : null;
        }

        /// <summary>
        /// Creates a message.
        /// </summary>
        /// <param name="message">The message to be created</param>
        /// <returns>True, when the message is created successfully, False otherwise</returns>
        public bool CreateMessage(Message message)
        {
            if (!BelongsEntityToUser(message.UserId))
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
            if (!BelongsEntityToUser(message.UserId))
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

        /// <summary>
        /// Checks if the Message belongs to the user that is signed in.
        /// </summary>
        /// <param name="userId">The userid of the message</param>
        /// <returns>True, if the message belongs to the signed in user, false otherwise</returns>
        private bool BelongsEntityToUser(Guid userId)
        {
            return userId != new Guid() && userId == AppData.UserId;
        }
    }
}