using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Minify.WPF.Models
{
    public class ListControlSelectionChangedEventArgs : SelectionChangedEventArgs
    {
        public Guid Id { get; set; }

        public ListControlSelectionChangedEventArgs(Guid Id, SelectionChangedEventArgs e) : base(e.RoutedEvent, e.RemovedItems, e.AddedItems)
        {
            this.Id = Id;
        }
    }
}
