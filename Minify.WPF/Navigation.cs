﻿using Minify.DAL.Entities;
using Minify.WPF.Managers;
using Minify.WPF.View;

using System;
using System.Collections.Generic;
using System.Text;

namespace Minify.WPF
{
    public class Navigation
    {
        private OverviewSongsPage _overviewSongsPage;
        private DetailHitlistPage _hitlistPage;
        private DetailStreamroomPage _overviewStreamroomPage;
        private AddHitlistPage _addHitlistPage;
        private readonly MainWindow _mainWindow;


        public DetailStreamroomPage DetailStreamroomPage
        {
            get { return _overviewStreamroomPage; }
            set
            {
                if (value != null)
                    value.MessagesRefreshed += _mainWindow.OverviewStreamroom_MessagesRefreshed;
                _overviewStreamroomPage = value;
            }
        }

        public AddHitlistPage AddHitlistPage
        {
            get { return _addHitlistPage; }
            set
            {
                value.HitlistAdded += _mainWindow.AddHitlistPage_UpdateHitlistMenu;
                _addHitlistPage = value;
            }
        }

        public DetailHitlistPage DetailHitlistPage
        {
            get { return _hitlistPage; }
            set
            {
                value.HitlistSongsSelectionChanged += _mainWindow.PlaySong;
                value.StreamroomCreated += _mainWindow.DetailHitlistPage_StreamroomCreated;
                value.RefreshHitlistOverview += _mainWindow.RefreshHitListMenu;
                _hitlistPage = value;
            }
        }

        public OverviewSongsPage OverviewSongsPage
        {
            get
            {
                return _overviewSongsPage;
            }
            set
            {
                _overviewSongsPage = value;
                _overviewSongsPage.SongSelected += _mainWindow.PlaySong;
            }
        }
    
        public Navigation(MainWindow mainWindow)
        {
            _mainWindow = mainWindow;
        } 

        public OverviewSongsPage CreateOverviewSongsPage(List<Song> songs = null)
        {
            OverviewSongsPage = new OverviewSongsPage(songs);
            return OverviewSongsPage;
        }

        public DetailHitlistPage CreateDetailHitlistPage(Guid id)
        {
            DetailHitlistPage = new DetailHitlistPage(id);
            return DetailHitlistPage;
        }

        public AddHitlistPage CreateAddHilistPage()
        {
            AddHitlistPage = new AddHitlistPage();
            return AddHitlistPage;
        }

        public DetailStreamroomPage CreateDetailStreamroomPage(Guid id, WpfMediaManager manager)
        {
            DetailStreamroomPage = new DetailStreamroomPage(id, manager);
            return DetailStreamroomPage;
        }

        internal bool Refresh(Song song)
        {
            DetailHitlistPage?.Refresh(song);

            OverviewSongsPage?.Refresh(song);

            if(DetailStreamroomPage != null)
                return DetailStreamroomPage.Refresh(song);

            return true;
        }
    }
}
