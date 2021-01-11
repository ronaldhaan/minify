using Minify.DAL.Entities;

using System;

namespace Minify.Core.Models
{
    public static class AppData
    {
        public static bool LoggedIn { get; set; }
        public static Guid UserId { get; set; }
        public static string UserName { get; set; }

        public static void SetSession(User user)
        {
            LoggedIn = true;
            UserId = user.Id;
            UserName = user.UserName;
        }

        public static void Initialize()
        {
            LoggedIn = false;
            UserId = Guid.Empty;
            UserName = null;
        }
    }
}