using System;

namespace SharedSquawk.Client.Model.Entities.Messages
{
    public class TextMessage : Event
    {
		public string UserName { get; set; }

        public int? UserId { get; set; }

		public string Body { get; set; }

		public DateTime Timestamp { get; set; }

        //public bool IsAdmin { get; set; }

        //public bool IsModerator { get; set; }

        

        //public int? ImageId { get; set; }
    }
}
