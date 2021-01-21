using Minify.Core.Models;
using Minify.WPF.Managers;

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
    /// Interaction logic for MediaPage.xaml
    /// </summary>
    public partial class MediaControl : UserControl
    {
        private readonly WpfMediaManager _mediaManager;

        public WpfMediaManager MediaManager { get => _mediaManager; }

        public event EventHandler OnPlay;
        public event EventHandler OnPause;

        public MediaControl()
        {
            _mediaManager = new WpfMediaManager(null);
            _mediaManager.UpdateMediaplayer += MediaManager_UpdateMediaplayer;
            _mediaManager.OnPlay += MediaManager_OnPlay;
            _mediaManager.OnPause += MediaManager_OnPause;

            InitializeComponent();

            if (UtilityWpf.IsInDesignMode) return;
            _mediaManager.Volume = (double)volumeSlider.Value;

        }

        #region Events
        private void VolumeSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _mediaManager.Volume = (double)volumeSlider.Value;
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e) => Play();

        private void BtnPause_Click(object sender, RoutedEventArgs e) => Pause();

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            DisplayPause(_mediaManager.Next());
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            _mediaManager.Replay();
        }

        private void BtnBack_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DisplayPause(_mediaManager.Previous());
        }

        private void TimelineSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            // Overloaded constructor takes the arguments days, hours, minutes, seconds, milliseconds.
            // Create a TimeSpan with miliseconds equal to the slider value.
            double minBetween = 2000;
            if (e.OldValue - e.NewValue > minBetween || e.NewValue - e.OldValue > minBetween)
            {
                int SliderValue = (int)timelineSlider.Value;
                _mediaManager.Position = new TimeSpan(0, 0, 0, 0, SliderValue);
            }

            var duration = _mediaManager.GetCurrentSong().Duration.ToString(@"mm\:ss");
            if (lblDuration.Content != null && (string)lblDuration.Content != duration)
            {
                lblDuration.Content = duration;
            }

            var position = _mediaManager.Position.ToString(@"mm\:ss");
            if (lblCurrentTime.Content != null && (string)lblCurrentTime.Content != position)
            {
                lblCurrentTime.Content = position;
            }
        }

        public void PlaySong(object sender, PlaySongEventArgs e)
        {
            _mediaManager.Songs = e.Songs;
            _mediaManager.Open(e.Song);
            _mediaManager.Play();
        }

        private void MediaManager_UpdateMediaplayer(object sender, UpdateMediaplayerEventArgs e)
        {
            SetSongData(e);
        }

        private void MediaManager_OnPause(object sender, EventArgs e)
        {
            DisplayPlay();

            OnPause?.Invoke(this, new EventArgs());
        }

        private void MediaManager_OnPlay(object sender, UpdateMediaplayerEventArgs e)
        {
            DisplayPause();
            SetSongData(e);

            OnPlay?.Invoke(this, new EventArgs());
        }
        #endregion Events

        private void Play()
        {
            _mediaManager.Play();
        }

        private void Pause()
        {
            _mediaManager.Pause();
        }

        private void DisplayPause(bool condition)
        {
            if (condition)
            {
                DisplayPause();
            }
            else
            {
                DisplayPlay();
            }
        }

        private void DisplayPlay()
        {
            Dispatcher.Invoke(() =>
            {
                btnPause.Visibility = Visibility.Collapsed;
                btnPlay.Visibility = Visibility.Visible;
            });
        }

        private void DisplayPause()
        {
            Dispatcher.Invoke(() =>
            {
                btnPlay.Visibility = Visibility.Collapsed;
                btnPause.Visibility = Visibility.Visible;
            });
        }

        public void EnableSlider(bool condition)
        {
            timelineSlider.IsEnabled = condition;
        }

       
        private void SetSongData(UpdateMediaplayerEventArgs e)
        {
            if ((string)lblSongName.Content != e.SongName)
            {
                lblSongName.Content = e.SongName;
            }
            if ((string)lblArtist.Content != e.Artist)
            {
                lblArtist.Content = e.Artist;
            }

            double duration = e.Duration.TotalMilliseconds;
            if (timelineSlider.Maximum != duration)
            {
                timelineSlider.Maximum = e.Duration.TotalMilliseconds;
            }

            timelineSlider.Value = e.Position.TotalMilliseconds;
        }
        public void Stop() => _mediaManager.Stop();

        public void Close() => _mediaManager.Close();

        internal void ToggleMediaPlayer()
        {
            if (_mediaManager.Paused)
            {
                Play();
            }
            else
            {
                Pause();
            }
        }
    }
}
