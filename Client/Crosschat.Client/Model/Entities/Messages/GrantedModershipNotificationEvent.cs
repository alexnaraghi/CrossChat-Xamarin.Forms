using System;

namespace SharedSquawk.Client.Model.Entities.Messages
{
    public class GrantedModershipNotificationEvent : Event
    {
        public string ActorName { get; set; }

        public string TargetName { get; set; }

        public Guid TargetId { get; set; }
    }
}