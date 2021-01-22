using Minify.DAL.Entities;
using Minify.WPF.Managers;
using Minify.WPF.View;

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

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

        public OverviewSongsPage ToOverviewSongsPage(List<Song> songs = null)
        {
            OverviewSongsPage = new OverviewSongsPage(songs);
            return OverviewSongsPage;
        }

        public DetailHitlistPage ToDetailHitlistPage(Guid id)
        {
            DetailHitlistPage = new DetailHitlistPage(id);
            return DetailHitlistPage;
        }

        public AddHitlistPage ToAddHilistPage()
        {
            AddHitlistPage = new AddHitlistPage();
            return AddHitlistPage;
        }

        public DetailStreamroomPage ToDetailStreamroomPage(Guid id)
        {
            DetailStreamroomPage = new DetailStreamroomPage(id);
            return DetailStreamroomPage;
        }

        public Page To404Page()
        {
            return new Page { Content = "Page not found" };
        }

        public object To<T>(object param = null) where T : Page
        {
            Page page = To404Page();

            Type t = typeof(T);

            if (t == typeof(AddHitlistPage))
            {
                page = ToAddHilistPage();
            }
            else if(t == typeof(DetailUserPage))
            {
                page = new DetailUserPage();
            }
            else if(t == typeof(SettingsPage))
            {
                page = new SettingsPage();
            }
            else if (t == typeof(OverviewSongsPage))
            {
                if (param is List<Song> list)
                    page = ToOverviewSongsPage(list);
                else if (param == null)
                    page = ToOverviewSongsPage();
            }
            else if (t == typeof(DetailHitlistPage))
            {
                if (param is Guid id)
                    page = ToDetailHitlistPage(id);
            }
            else if (t == typeof(DetailStreamroomPage))
            {
                if (param is Guid id)
                    page = ToDetailStreamroomPage(id);
            }

            return page;
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
