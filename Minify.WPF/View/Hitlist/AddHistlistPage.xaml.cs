using Minify.Core.Controllers;
using Minify.DAL.Entities;
using Minify.Core.Models;
using System.Windows;
using System.Windows.Controls;
using Minify.Core.Managers;

namespace Minify.WPF.View
{
    public delegate void HitlistAddedEventHandler(object sender, UpdateHitlistMenuEventArgs e);

    /// <summary>
    /// Interaction logic for AddHistlistPage.xaml
    /// </summary>
    public partial class AddHistlistPage : Page
    {
        private readonly HitlistController _controller;

        public event HitlistAddedEventHandler HitlistAdded;
        

        public AddHistlistPage()
        {
            _controller = ControllerManager.Get<HitlistController>();
            InitializeComponent();
        }

        private void Create_Button_Click(object sender, RoutedEventArgs e)
        {
            // get Title and Description from page
            string title = tbxTitle.Text;
            string description = tbxDescription.Text;

            if (!_controller.ValidateTitle(title))
            {
                // display error message for title
                TitleError.Visibility = Visibility.Visible;
            }
            
            if (!_controller.ValidateDescription(description))
            {
                // display error message for description
                DescriptionError.Visibility = Visibility.Visible;
            }

            if(_controller.ValidateTitle(title) && _controller.ValidateDescription(description))
            {
                Hitlist hitlist = new Hitlist(title, description, AppData.UserId);
                hitlist = _controller.Add(hitlist);
                HitlistAdded.Invoke(this, new UpdateHitlistMenuEventArgs(hitlist.Id));

                tbxTitle.Text = string.Empty;
                tbxDescription.Text = string.Empty;
            }
        }
    }
}