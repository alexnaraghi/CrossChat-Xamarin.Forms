﻿using SharedSquawk.Client.Model.Entities.Messages;

namespace SharedSquawk.Client.ViewModels
{
    public class EventViewModelFactory
    {
        public EventViewModel Get(Event @event, int currentUserId)
        {
            if (@event is TextMessage)
				return new TextMessageViewModel(@event as TextMessage, currentUserId);

            //TODO: create VM for other event types 

            return new EventViewModel(@event);
        }
    }
}
