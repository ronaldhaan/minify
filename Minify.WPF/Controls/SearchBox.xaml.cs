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

namespace Minify.WPF.Controls
{
    /// <summary>
    /// Interaction logic for SearchBox.xaml
    /// </summary>
    public partial class SearchBox : UserControl
    {
        private readonly string defaultSearchValue;
        public string Text { get => tbxSearchSongs.Text; set => tbxSearchSongs.Text = value; }

        public event EventHandler<TextChangedEventArgs> TextChanged;

        public SearchBox()
        {
            InitializeComponent();

            defaultSearchValue = tbxSearchSongs.Text;
        }

        private void TbxSearchSongs_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbxSearchSongs.Text == defaultSearchValue)
            {
                tbxSearchSongs.Text = string.Empty;
            }
        }

        private void TbxSearchSongs_LostFocus(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(tbxSearchSongs.Text))
            {
                tbxSearchSongs.Text = defaultSearchValue;
            }
        }

        private void TbxSearchSongs_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxSearchSongs.Text != defaultSearchValue)
            {
                TextChanged?.Invoke(this, e);
            }
        }
    }
}
