﻿using Minify.Core.Controllers;
using Minify.DAL.Entities;
using Minify.Core.Models;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Timers;
using System.Linq;

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

        private readonly DateTime timeJoined;
        private readonly MediaManager _manager;

        private readonly StreamroomController _streamroomController;
        private readonly MessageController _messageController;

        public StreamroomRefreshedEventHandler StreamroomRefreshed;
        public StreamroomIsPausedToggledEventHandler IsPausedToggled;

        public StreamroomManager(Guid streamroomId, MediaManager manager)
        {
            _streamroomController = ControllerManager.Get<StreamroomController>();
            _messageController = ControllerManager.Get<MessageController>();
            _manager = manager;

            _timer = new Timer(INTERVAL);
            _messages = new List<Message>();
            _streamroomId = streamroomId;
            timeJoined = DateTime.Now;
            LoadData();

            _timer.Enabled = true;
            _timer.Elapsed += OnTimedEvent;
        }

        public void OnTimedEvent(object obj, ElapsedEventArgs e)
        {
            if (_streamroom.Hitlist.UserId == AppData.UserId)
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
            if (_streamroom.Id == null)
                throw new ArgumentNullException("id");

            _streamroom.IsPaused = true;
            Update();
        }

        public void Play()
        {
            if (_streamroom.Id == null)
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
            if (_streamroom.Hitlist.UserId == AppData.UserId)
            {
                _streamroom.CurrentSongPosition = _manager.CurrentSongPosition;
                _streamroom.CurrentSongId = _manager.GetCurrentSong().Id;
                Update();
            }
        }

        private void LoadData()
        {
            _streamroom = _streamroomController.Get(_streamroomId, true);
            _messages = _messageController.GetMessages(_streamroom);

            _messages = _messages
                .Where(m => m.CreatedAt > timeJoined)
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