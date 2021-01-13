﻿using Microsoft.Extensions.Configuration;

using Minify.Core.Models;

using System;
using System.Collections;
using System.Collections.Generic;

namespace Minify.Core
{
    public class Utility : DAL.Utility
    {
        public void Initialize()
        {
        }

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

        public static T Serialize<T>(T obj, IConfigurationSection section) where T : IMinifySerializable 
        {
            foreach(var child in section.GetChildren())
            {
                var propInfo = typeof(T).GetProperty(child.Key);
                propInfo.SetValue(obj, child.Value);
            }

            return obj;
        }
    }
}