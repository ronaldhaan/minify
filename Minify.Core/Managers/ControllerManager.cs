using System;
using System.Collections.Generic;
using System.Text;

namespace Minify.Core.Managers
{
    public static class ControllerManager
    {
        public static Dictionary<string, IController> Controllers { get => _controllers; private set => _controllers = value; }
        private static Dictionary<string, IController> _controllers;

        public static void Initialize()
        {
            Controllers = new Dictionary<string, IController>();
        }

        public static T Get<T>() where T : IController
        {
            return (T)Get(typeof(T).Name);
        }

        public static IController Get(string controllerName)
        {
            if(Controllers != null && Controllers.TryGetValue(controllerName, out IController c))
            {
                return c ?? null;
            }

            return null;
        }

        public static bool AddRange(params IController[] controllers)
        {
            if (controllers == null || controllers.Length == 0 || Controllers == null) return false;

            bool success = true;

            foreach (IController c in controllers)
            {
                if(Add(c) == false)
                {
                    success = false;
                }
            }

            return success;
        }

        public static bool Add(IController controller)
        {
            if (Controllers == null || controller == null) return false;

            return Controllers.TryAdd(controller.GetType().Name, controller);
        }

        public static bool RemoveRange(params IController[] controllers)
        {
            if (controllers == null || controllers.Length == 0 || Controllers == null) return false;

            bool success = true;

            foreach (IController c in controllers)
            {
                if(Remove(c) == false)
                {
                    success = false;
                }
            }
            
            return success;
        }

        public static bool Remove(IController controller)
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
