using Minify.Core;
using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Models;

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

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for ControlStreamroomList.xaml
    /// </summary>
    public partial class ControlStreamroomList : UserControl
    {
        public List<Streamroom> List { get; set; }

        private readonly StreamroomController controller;

        public event EventHandler<EntitySelectionChangedEventArgs> SelectionChanged;

        public bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor
                    .FromProperty(prop, typeof(FrameworkElement))
                    .Metadata.DefaultValue;
            }
        }

        public ControlStreamroomList()
        {
            InitializeComponent();
            if (IsInDesignMode) return;
            controller = AppManager.Get<StreamroomController>();

            if (Utility.ListIsNullOrEmpty(List))
            {
                Update();
            }

            menu.ItemsSource = List;
        }

        public void Menu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is Streamroom selected)
            {
                menu.SelectedItem = selected;
                SelectionChanged?.Invoke(this, new EntitySelectionChangedEventArgs(selected.Id, e));
            }
        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        public void UpdateSelected(Guid id)
        {
            Update();
            Refresh();

            // set the new item as selected
            foreach (var item in menu.Items)
            {
                // cast ListViewItem to Streamroom and check if the id equals the eventargs id
                if (item is Streamroom streamroom && streamroom.Id.Equals(id))
                {
                    menu.SelectedItem = item;
                }
            }
        }

        public void Update()
        {
            List = controller.GetAll(true);
            menu.ItemsSource = List;
        }

        public void Refresh() => menu.Items.Refresh();

        public void Reset() => menu.SelectedItem = null;
    }
}
