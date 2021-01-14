using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace Minify.WPF.Models
{
    public class EntitySelectionChangedEventArgs : SelectionChangedEventArgs
    {
        public Guid Id { get; set; }

        public EntitySelectionChangedEventArgs(Guid Id, SelectionChangedEventArgs e) : base(e.RoutedEvent, e.RemovedItems, e.AddedItems)
        {
            this.Id = Id;
        }
    }
}
