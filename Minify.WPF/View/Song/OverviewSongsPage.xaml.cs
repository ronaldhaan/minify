﻿using Minify.Core.Controllers;
using Minify.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Minify.WPF.Managers;
using Minify.Core.Managers;

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for OverviewSongsPage.xaml
    /// </summary>
    public partial class OverviewSongsPage : Page
    {
        private readonly List<Song> _songs;
        private WpfMediaManager _mediaManager;
        private readonly SongController _songController;
        private readonly HitlistController _hitlistController;
        public OverviewSongsPage(WpfMediaManager manager)
        {
            _mediaManager = manager;
            InitializeComponent();
            _songController = ControllerManager.Get<SongController>();
            _hitlistController = ControllerManager.Get<HitlistController>();
            _songs = _songController.GetAll();
            Songs.ItemsSource = _songs;
        }

        public void Refresh(Song song)
        {
            Songs.ItemsSource = _songs;

            foreach (var item in Songs.Items)
            {
                if (((Song)item).Equals(song))
                    Songs.SelectedItem = item;
            }
        }

        private void Songs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                // Get song
                Song selectedSong = (Song)e.AddedItems[0];

                // Initialize songs
                _mediaManager.Songs = _songs;

                // Open song
                _mediaManager.Open(selectedSong);
            }
        }

        public void Refresh()
        {
            List<Song> items = _songController.GetAll();
            Songs.ItemsSource = items.ToArray();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                Guid songId = (Guid)btn.CommandParameter;
                ChooseHitlistDialog choose = new ChooseHitlistDialog(songId, this);
                choose.IdRetreived += IdRetreived;
                choose.Show();
                btn.Visibility = Visibility.Hidden;
            }
            catch (Exception)
            {
                //Handle Exception
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IdRetreived(object sender, EventArgs e)
        {
            try
            {
                var re = (IdRetreivedEventArgs)e;

                Hitlist hitlist = _hitlistController.Get(re.HitlistId, true);
                Song song = _songController.Get(re.SongId);

                if (!hitlist.Songs.Any(x => x.SongId == song.Id))
                {
                    _hitlistController.AddSongsToHitlist(hitlist, song);
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Debug.Write(ex);
            }
        }

        public OverviewSongsPage(List<Song> songs)
        {
            InitializeComponent();
            Songs.ItemsSource = songs;
        }
    }
}