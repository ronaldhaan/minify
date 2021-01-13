﻿using Castle.Core.Internal;

using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;
using Minify.WPF.Managers;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Minify.WPF.View
{
    public delegate void StreamroomCreatedEventHandler(object sender, CreatedStreamRoomEventArgs e);

    public delegate void RefreshHitlistOverview(object sender, EventArgs e);

    /// <summary>
    /// Interaction logic for OverviewHitlistPage.xaml
    /// </summary>
    public partial class DetailHitlistPage : Page
    {
        private readonly HitlistController _hitlistController;
        private readonly StreamroomController _streamroomController;
        private readonly Hitlist _hitlist;
        private readonly AppData _appData;
        private List<Song> _songs = new List<Song>();

        public WpfMediaManager MediaManager;

        public event RefreshHitlistOverview RefreshHitlistOverview;

        public StreamroomCreatedEventHandler StreamroomCreated;

        public DetailHitlistPage(Guid id, WpfMediaManager manager) : this(id)
        {
            MediaManager = manager;
        }

        public DetailHitlistPage(Guid id)
        {
            InitializeComponent();
            btnDeleteHitlist.Visibility = Visibility.Hidden;
            btnCreateStreamroom.Visibility = Visibility.Hidden;
            // create instances of controllers and get the hitlist by id
            _hitlistController = AppManager.Get<HitlistController>();
            _streamroomController = AppManager.Get<StreamroomController>();
            _appData = AppManager.Get<AppData>();
            _hitlist = _hitlistController.Get(id, true);

            // check if hitlist is not null
            if (_hitlist != null)
            {
                // set the title, description and the info in the overview
                HitlistTitle.Content = _hitlist.Title;
                if (!_hitlist.Description.IsNullOrEmpty())
                {
                    HitlistDescription.Content = _hitlist.Description;
                    HitlistDescription.Visibility = Visibility.Visible;
                }
                HitlistInfo.Content = _hitlistController.GetHitlistInfo(_hitlist);

                // if there are songs, display the listview
                if (_hitlist.Songs != null && _hitlist.Songs.Count > 0)
                {
                    _songs = _hitlistController.GetSongs(_hitlist.Songs);
                    HitlistSongs.ItemsSource = _songs;
                    HitlistSongs.Visibility = Visibility.Visible;

                    // display create streamroom button when room doesn't exist yet
                    if (!_streamroomController.DoesRoomAlreadyExist(_hitlist.Id))
                    {
                        btnCreateStreamroom.Visibility = Visibility.Visible;
                    }
                }

                if (_appData.BelongsEntityToUser(_hitlist.UserId) && !_streamroomController.DoesRoomAlreadyExist(_hitlist.Id))
                {
                    btnDeleteHitlist.Visibility = Visibility.Visible;
                }
            }
        }

        public void Refresh(Song song)
        {
            _songs = _hitlistController.GetSongs(_hitlist.Songs);
            HitlistSongs.ItemsSource = _songs;

            foreach (var item in HitlistSongs.Items)
            {
                if (((Song)item).Equals(song))
                    HitlistSongs.SelectedItem = item;
            }

            HitlistSongs.Visibility = Visibility.Visible;
        }

        private void HitlistSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!Core.Utility.ListIsNullOrEmpty(e.AddedItems) && e.AddedItems[0] is Song selected)
            {
                // Initialize songs
                MediaManager.Songs = _songs;

                // Open song
                MediaManager.Open(selected);

                // Play song
                MediaManager.Play();
            }
        }

        private void Btn_delete_click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure?",
                                             "Confirmation",
                                             MessageBoxButton.YesNo,
                                             MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                _hitlistController.Delete(_hitlist);
                MessageBox.Show("Hitlist Deleted", "Success");
                RefreshHitlistOverview?.Invoke(this, new EventArgs());
            }
        }

        /// <summary>
        /// Creates new streamroom when button is clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateStreamroom_Click(object sender, RoutedEventArgs e)
        {
            Streamroom streamroom = new Streamroom(_hitlist.Id, _songs.First().Id);
            _streamroomController.Add(streamroom);
            StreamroomCreated?.Invoke(this, new CreatedStreamRoomEventArgs { Streamroom = streamroom });
        }
    }
}