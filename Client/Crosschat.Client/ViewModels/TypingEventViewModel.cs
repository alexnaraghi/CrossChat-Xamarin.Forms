using System;
using SharedSquawk.Client.Model.Entities.Messages;
using SharedSquawk.Server.Infrastructure;

namespace SharedSquawk.Client.ViewModels
{
    public class TypingEventViewModel : EventViewModel
    {
		public TypingEventViewModel(TypingEvent typingEvent) : base(typingEvent)
        {
			UserId = typingEvent.UserId;
			UserName = typingEvent.UserName;
        }

        public int? UserId { get; set; }
        public string UserName { get; private set; }
    }
}