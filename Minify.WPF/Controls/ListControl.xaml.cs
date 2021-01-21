using Minify.Core;
using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Models;
using Minify.WPF.ViewModels;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Minify.WPF.Controls
{
    /// <summary>
    /// Interaction logic for ControlStreamroomList.xaml
    /// </summary>
    public partial class ListControl : UserControl
    {
        public static readonly DependencyProperty ItemsDependency = DependencyProperty.Register(nameof(ItemsSource), 
                                                                            typeof(object), 
                                                                            typeof(ListControl), 
                                                                            new FrameworkPropertyMetadata(FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public static readonly DependencyProperty TextDependency = DependencyProperty.Register(nameof(Title),
                                                                            typeof(string),
                                                                            typeof(ListControl),
                                                                            new PropertyMetadata(default(string)));
        public object ItemsSource { get => GetValue(ItemsDependency); set => SetValue(ItemsDependency, value); }

        public string Title { get => (string)GetValue(TextDependency); set => SetValue(TextDependency, value); }

        public event EventHandler<ListControlSelectionChangedEventArgs> SelectionChanged;

        public ListControl()
        {
            InitializeComponent();
            if (UtilityWpf.IsInDesignMode) return;
        }

        public void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is ListControlViewModel selected)
            {
                menu.SelectedItem = selected;
                SelectionChanged?.Invoke(this, new ListControlSelectionChangedEventArgs(selected.Id, e));
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void UpdateSelected(Guid id)
        {
            Refresh();

            // set the new item as selected
            foreach (var item in menu.Items)
            {
                // cast ListViewItem to Streamroom and check if the id equals the eventargs id
                if (item is ListControlViewModel viewModel && viewModel.Id.Equals(id))
                {
                    menu.SelectedItem = item;
                }
            }
        }

        public void Refresh() => menu.Items.Refresh();

        public void Reset() => menu.SelectedItem = null;
    }
}
