using Minify.WPF.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Minify.WPF.Managers
{
    public class NavigationManager
    {
        private Dictionary<string, Page> _dictionary;

        public T Get<T>() where T : Page 
        {
            return (T)Get(typeof(T).Name);
        }

        public Page Get(string pageName)
        {
            if (_dictionary != null && _dictionary.TryGetValue(pageName, out Page c))
            {
                return c ?? null;
            }

            return null;
        }
        public bool AddRange(params Page[] pages)
        {
            if (pages == null || pages.Length == 0 || _dictionary == null) return false;

            bool success = true;

            foreach (Page c in pages)
            {
                if (Add(c) == false)
                {
                    success = false;
                }
            }

            return success;
        }

        public bool Add(Page page)
        {
            if (_dictionary == null || page == null) return false;

            return _dictionary.TryAdd(page.GetType().Name, page);
        }

        public bool RemoveRange(params Page[] pages)
        {
            if (pages == null || pages.Length == 0 || _dictionary == null) return false;

            bool success = true;

            foreach (Page c in pages)
            {
                if (Remove(c) == false)
                {
                    success = false;
                }
            }

            return success;
        }

        public bool Remove(Page page)
        {
            if (_dictionary == null || page == null) return false;

            return _dictionary.Remove(page.GetType().Name);
        }

        public void Clear()
        {
            if (_dictionary != null)
                _dictionary.Clear();
        }

        public void Terminate()
        {
            if (_dictionary != null)
            {
                _dictionary.Clear();
                _dictionary = null;
            }
        }
    }
}
