using Minify.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace Minify.WPF.ViewModels
{
    public class ListControlViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public static  List<ListControlViewModel> GetViewModel(List<Streamroom> streamrooms)
        {
            List<ListControlViewModel> list = new List<ListControlViewModel>();
            foreach (var room in streamrooms)
            {
                list.Add(new ListControlViewModel
                {
                    Id = room.Id,
                    Title = room.Hitlist.Title
                });
            }

            return list;
        }

        public static  List<ListControlViewModel> GetViewModel(List<Hitlist> hitlists)
        {
            List<ListControlViewModel> list = new List<ListControlViewModel>();
            foreach (var hitlist in hitlists)
            {
                list.Add(new ListControlViewModel
                {
                    Id = hitlist.Id,
                    Title = hitlist.Title
                });
            }

            return list;
        }
    }
}
