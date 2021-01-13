using System;
using System.Collections.Generic;
using System.Text;

namespace Minify.Core.Managers
{
    public static class AppManager
    {
        public static Dictionary<string, IMinifySerializable> Controllers { get => _controllers; private set => _controllers = value; }
        private static Dictionary<string, IMinifySerializable> _controllers;

        public static void Initialize()
        {
            Controllers = new Dictionary<string, IMinifySerializable>();
        }

        public static T Get<T>() where T : IMinifySerializable
        {
            return (T)Get(typeof(T).Name);
        }

        public static IMinifySerializable Get(string controllerName)
        {
            if(Controllers != null && Controllers.TryGetValue(controllerName, out IMinifySerializable c))
            {
                return c ?? null;
            }

            return null;
        }

        public static bool AddRange(params IMinifySerializable[] controllers)
        {
            if (controllers == null || controllers.Length == 0 || Controllers == null) return false;

            bool success = true;

            foreach (IMinifySerializable c in controllers)
            {
                if(Add(c) == false)
                {
                    success = false;
                }
            }

            return success;
        }

        public static bool Add(IMinifySerializable controller)
        {
            if (Controllers == null || controller == null) return false;

            return Controllers.TryAdd(controller.GetType().Name, controller);
        }

        public static bool RemoveRange(params IMinifySerializable[] controllers)
        {
            if (controllers == null || controllers.Length == 0 || Controllers == null) return false;

            bool success = true;

            foreach (IMinifySerializable c in controllers)
            {
                if(Remove(c) == false)
                {
                    success = false;
                }
            }
            
            return success;
        }

        public static bool Remove(IMinifySerializable controller)
        {
            if (Controllers == null || controller == null) return false;

            return Controllers.Remove(controller.GetType().Name);
        }

        public static void Clear()
        {
            if (Controllers != null)
                Controllers.Clear();
        }

        public static void Terminate()
        {
            if (Controllers != null)
            {
                Controllers.Clear();
                Controllers = null;
            }
        }
    }
}
