using System;
using SharedSquawk.Client.Model.Entities.Messages;

namespace SharedSquawk.Client
{
	public class TypingEvent : Event
	{
		public string UserName { get; set; }
		public int? UserId { get; set; }
	}
}

