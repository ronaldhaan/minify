using Minify.Core.Models;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Minify.Core
{
    public static class Utility
    {
        public const string DefaultColor = "#FF821BB2";
        public const string DefaultForegroundColor = "#FFFFFF"; 
        public static bool GuidIsNullOrEmpty(Guid guid)
        {
            return guid == null || guid == Guid.Empty;
        }
        public static bool ListIsNullOrEmpty(IList list)
        {
            return list == null || list.Count == 0;
        }
        public static bool ListIsNullOrEmpty<T>(List<T> list) where T : class
        {
            return list == null || list.Count == 0;
        }

        /// <summary>
        /// Checks if the Message belongs to the user that is signed in.
        /// </summary>
        /// <param name="userId">The userid of the message</param>
        /// <returns>True, if the message belongs to the signed in user, false otherwise</returns>
        public static bool BelongsEntityToUser(Guid userId)
        {
            return userId != new Guid() && userId == AppData.UserId;
        }
    }
}