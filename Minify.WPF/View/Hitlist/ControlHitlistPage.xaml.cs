using System;
using System.Collections.Generic;
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

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for ControlHitlistPage.xaml
    /// </summary>
    public partial class ControlHitlistPage : UserControl
    { 
        public List<Hitlist> Hitlists { get; set; }

        private readonly HitlistController controller;
        private readonly AppData data;
        public ControlHitlistPage(List<Hitlist> hitlists = null)
        {
            controller = AppManager.Get<HitlistController>();
            data = AppManager.Get<AppData>();

            if (Utility.ListIsNullOrEmpty(hitlists))
            {
                hitlists = controller.GetHitlistsByUserId(data.UserId);
            }

            Hitlists = hitlists;
            InitializeComponent();
        }

        private void HitlistMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
