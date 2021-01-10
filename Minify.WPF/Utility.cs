using System;
using System.Collections;
using System.Collections.Generic;

namespace Minify.WPF
{
    public static class Utility
    {
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
    }
}