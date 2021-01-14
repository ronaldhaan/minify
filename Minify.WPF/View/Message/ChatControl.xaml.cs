using Minify.Core.Controllers;
using Minify.Core.Managers;
using Minify.Core.Models;
using Minify.DAL.Entities;

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

namespace Minify.WPF.View
{
    /// <summary>
    /// Interaction logic for ChatControl.xaml
    /// </summary>
    public partial class ChatControl : UserControl
    {
        private readonly MessageController _messageController;
        private readonly AppData appData;
        public Guid StreamroomId { get; set; }

        private bool _autoScroll = true;

        public event EventHandler AutoScrollValueChanged;
        public bool AutoScroll
        {
            get => _autoScroll; 
            private set
            {
                _autoScroll = value;
                AutoScrollValueChanged?.Invoke(this, new EventArgs());
            }
        }

        public ChatControl()
        {
            _messageController = AppManager.Get<MessageController>();
            appData = AppManager.Get<AppData>();
            InitializeComponent();
        }

        private void TbxMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                CreateMessage();
            }
        }

        private void CreateMessage()
        {
            if(!string.IsNullOrEmpty(tbxMessage.Text))
            {
                _messageController.CreateMessage(tbxMessage.Text, appData.UserId, StreamroomId);
                tbxMessage.Text = string.Empty;
            }
        }

        private void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange == 0)
            {
                _autoScroll = scrollViewer.VerticalOffset == scrollViewer.ScrollableHeight;
            }

            if (_autoScroll && e.ExtentHeightChange != 0)
            {
                scrollViewer.ScrollToVerticalOffset(scrollViewer.ExtentHeight);
            }
        }

        public void LoadMessages(List<Message> messages) => messages.ForEach(m => LoadMessage(m));

        /// <summary>
        /// Sends chat message into the chatbox
        /// </summary>
        /// <param name="message"></param>
        public void LoadMessage(Message message)
        {
            StackPanel stackPanel = new StackPanel();
            TextBlock user = new TextBlock();
            TextBlock lblmessage = new TextBlock();

            stackPanel.HorizontalAlignment = HorizontalAlignment.Stretch;
            user.Text = $"{message.User.Person.FirstName} {message.User.Person.LastName} - {message.CreatedAt:t}";
            user.FontWeight = FontWeights.Bold;
            lblmessage.Text = message.Text;
            stackPanel.Children.Add(user);
            stackPanel.Children.Add(lblmessage);

            messagePanel.Children.Add(stackPanel);
        }

        public void Clear() => messagePanel.Children.Clear();
    }
}
