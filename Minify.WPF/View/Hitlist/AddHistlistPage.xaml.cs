using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;

using System.Windows;
using System.Windows.Controls;

namespace Minify.WPF.View
{
    public delegate void HitlistAddedEventHandler(object sender, UpdateHitlistMenuEventArgs e);

    /// <summary>
    /// Interaction logic for AddHistlistPage.xaml
    /// </summary>
    public partial class AddHistlistPage : Page
    {
        private readonly HitlistController _controller;

        private AppData _appData;

        public event HitlistAddedEventHandler HitlistAdded;


        public AddHistlistPage()
        {
            _controller = AppManager.Get<HitlistController>();
            _appData = AppManager.Get<AppData>();
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

            if (_controller.ValidateTitle(title) && _controller.ValidateDescription(description))
            {
                Hitlist hitlist = new Hitlist(title, description, _appData.UserId);
                hitlist = _controller.Add(hitlist);
                HitlistAdded.Invoke(this, new UpdateHitlistMenuEventArgs(hitlist.Id));

                tbxTitle.Text = string.Empty;
                tbxDescription.Text = string.Empty;
            }
        }
    }
}