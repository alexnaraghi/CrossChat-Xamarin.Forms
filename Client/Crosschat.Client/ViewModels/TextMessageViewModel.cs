using System;
using SharedSquawk.Client.Model.Entities.Messages;
using SharedSquawk.Server.Infrastructure;

namespace SharedSquawk.Client.ViewModels
{
    public class TextMessageViewModel : EventViewModel
    {
		public TextMessageViewModel(TextMessage textMessage, int currentUserId) : base(textMessage)
        {
			AuthorName = textMessage.UserName;
            Text = textMessage.Body;
            Timestamp = textMessage.Timestamp;
			IsMine = textMessage.UserId.HasValue ? textMessage.UserId.Value == currentUserId : false;
        }

        public bool IsMine { get; set; }

        public string AuthorName { get; private set; }

        public string Text { get; private set; }

        public DateTime Timestamp { get; private set; }
    }
}