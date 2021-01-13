using Microsoft.Extensions.Configuration;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Minify.Core
{
    public class Utility : DAL.Configuraion
    {
        public void Initialize()
        {
        }

        public static bool GuidIsNullOrEmpty(Guid guid) => guid == null || guid == Guid.Empty;

        public static bool ListIsNullOrEmpty(IList list) => list == null || list.Count == 0;

        public static bool ListIsNullOrEmpty<T>(List<T> list) where T : class => list == null || list.Count == 0;

        public static bool CollectionIsNullOrEmpty<T>(ICollection<T> collection) => collection == null || collection.Count == 0;

        public static T Serialize<T>(T obj, IConfigurationSection section) where T : IMinifySerializable
        {
            foreach (var child in section.GetChildren())
            {
                try
                {
                    var propInfo = typeof(T).GetProperty(child.Key);
                    propInfo.SetValue(obj, child.Value);
                }
                catch (Exception) { continue; }
            }

            return obj;
        }
    }
}