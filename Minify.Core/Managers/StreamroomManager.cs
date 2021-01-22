using Minify.Core.Controllers;
using Minify.Core.Models;
using Minify.DAL.Entities;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace Minify.Core.Managers
{
    public delegate void StreamroomRefreshedEventHandler(object sender, LocalStreamroomUpdatedEventArgs e);

    public delegate void StreamroomIsPausedToggledEventHandler(object sender, IsPausedEventArgs e);

    public class StreamroomManager
    {
        private const int INTERVAL = 1000;
        private readonly Timer _timer;
        private readonly Guid _streamroomId;
        private Streamroom _streamroom;
        private List<Message> _messages;

        private readonly DateTime _timeJoined;

        private readonly StreamroomController _streamroomController;
        private readonly MessageController _messageController;
        private readonly AppData _appData;
        public StreamroomRefreshedEventHandler StreamroomRefreshed;
        public StreamroomIsPausedToggledEventHandler IsPausedToggled;

        public StreamroomManager(Guid streamroomId)
        {
            _streamroomController = AppManager.Get<StreamroomController>();
            _messageController = AppManager.Get<MessageController>();
            _appData = AppManager.Get<AppData>();

            _timer = new Timer(INTERVAL);
            _messages = new List<Message>();
            _streamroomId = streamroomId;
            _timeJoined = DateTime.Now;
            LoadData();

            _timer.Enabled = true;
            _timer.Elapsed += OnTimedEvent;
        }

        public void OnTimedEvent(object obj, ElapsedEventArgs e)
        {
            if (_appData.BelongsEntityToUser(_streamroom.Hitlist.UserId))
            {
                UpdateData();
            }
            LoadData();

            StreamroomRefreshed?.Invoke(this, new LocalStreamroomUpdatedEventArgs(_streamroom, _messages));
        }

        public void Start()
        {
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
        }

        public void Pause()
        {
            if (Utility.GuidIsNullOrEmpty(_streamroom.Id))
                throw new ArgumentNullException("id");

            _streamroom.IsPaused = true;
            Update();
        }

        public void Play()
        {
            if (Utility.GuidIsNullOrEmpty(_streamroom.Id))
                throw new ArgumentNullException("id");

            _streamroom.IsPaused = false;
            Update();
        }

        public bool IsPaused()
        {
            return _streamroom.IsPaused;
        }

        private void Update()
        {
            _streamroomController.Update(_streamroom);
        }

        private void UpdateData()
        {
            if(_appData.CurrentSong != null)
            {
                if (_appData.BelongsEntityToUser(_streamroom.Hitlist.UserId))
                {
                    _streamroom.CurrentSongPosition = _appData.CurrentSongPosition;
                    _streamroom.CurrentSongId = _appData.CurrentSong.Id;
                    Update();
                }
            }            
        }

        private void LoadData()
        {
            _streamroom = _streamroomController.Get(_streamroomId, true);
            _messages = _messageController.GetMessages(_streamroom);

            _messages = _messages
                .Where(m => m.CreatedAt > _timeJoined)
                .Distinct()
                .ToList();

            Debug.WriteLine($"Position song: {_streamroom.CurrentSongPosition}");
            Debug.WriteLine($"amount of messages: {_messages.Count}");
        }

        public void Close()
        {
            IsPausedToggled = null;
            StreamroomRefreshed = null;
        }
    }
}