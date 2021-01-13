using Minify.DAL.Entities;

using System;

namespace Minify.Core.Models
{
    public class AppData : IMinifySerializable
    {
        public bool LoggedIn { get; set; }

        public Guid UserId { get; set; }

        public string UserName { get; set; }

        public int ExpireLogin { get; set; }

        public DateTime LoginDate { get; set; }

        public string AppName { get; set; }

        public string CompanyName { get; set; }

        public string DefaultThemeColor { get; set; }

        public string DefaultForegroundColor { get; set; }

        public event EventHandler SaveUserData;

        public AppData()
        {
            Utility.Serialize(this, DAL.Configuraion.Root.GetSection("AppSettings"));

            LoggedIn = false;
            UserId = Guid.Empty;
            UserName = string.Empty;
        }

        public bool IsSessionActive()
        {
            DateTime dateTime = new DateTime(LoginDate.Ticks);

            return DateTime.Now < dateTime.AddMilliseconds(ExpireLogin);
        }

        public void SetSession(User user)
        {
            LoggedIn = true;
            UserId = user.Id;
            UserName = user.UserName;
            LoginDate = DateTime.UtcNow;

            SaveUserData?.Invoke(this, new EventArgs());
        }

        public void DestroySession()
        {
            LoggedIn = false;
            UserId = Guid.Empty;
            UserName = string.Empty;
            LoginDate = new DateTime(1, 1, 1);

            SaveUserData?.Invoke(this, new EventArgs());
        }

        /// <summary>
        /// Checks if the Message belongs to the user that is signed in.
        /// </summary>
        /// <param name="userId">The userid of the message</param>
        /// <returns>True, if the message belongs to the signed in user, false otherwise</returns>
        public bool BelongsEntityToUser(Guid userId)
        {
            return userId != new Guid() && userId == UserId;
        }
    }
}