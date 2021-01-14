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

using Minify.Core;
using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Models;

namespace Minify.WPF.View
{

    /// <summary>
    /// Interaction logic for ControlHitlistPage.xaml
    /// </summary>
    public partial class ControlHitlistList : UserControl
    { 
        public List<Hitlist> Hitlists { get; set; }

        private readonly HitlistController controller;
        private readonly AppData data;

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

        public ControlHitlistList()
        {
            InitializeComponent();
            if (IsInDesignMode) { return; }
            controller = AppManager.Get<HitlistController>();
            data = AppManager.Get<AppData>();

            if (Utility.ListIsNullOrEmpty(Hitlists))
            {
                Update();
            }

            HitlistMenu.ItemsSource = Hitlists;
        }

        public void HitlistMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is Hitlist selected)
            {
                HitlistMenu.SelectedItem = selected;
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
            foreach (var item in HitlistMenu.Items)
            {
                // cast ListViewItem to Hitlist and check if the id equals the eventargs id
                if (item is Hitlist hitlist && hitlist.Id.Equals(id))
                {
                    HitlistMenu.SelectedItem = item;
                }
            }
        }

        private void Update()
        {
            Hitlists = controller.GetHitlistsByUserId(data.UserId);
            HitlistMenu.ItemsSource = Hitlists;
        }

        public void Refresh() => HitlistMenu.Items.Refresh();

        public void Reset() => HitlistMenu.SelectedItem = null;
    }
}
