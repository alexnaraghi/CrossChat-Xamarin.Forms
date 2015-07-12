using System;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using System.Collections.Generic;

namespace Crosschat.Client
{
	public class ChatUpdateBuilder
	{
		private ChatUpdateRequest _request;
		public ChatUpdateBuilder (string d, int userId, string guid, int lastServerUpdateId, int clientUpdateId)
		{
			_request = new ChatUpdateRequest () {
				D = d,
				UserId = userId,
				GUID = guid,
				LastServerUpdateId = lastServerUpdateId,
				ClientUpdateId = clientUpdateId,
				Messages = new List<MessageDto>(),
				Rooms = "CREn" //TODO: Filter by rooms really
			};
		}

		public void Clear(int lastServerUpdateId, int clientUpdateId)
		{
			_request.LastServerUpdateId = lastServerUpdateId;
			_request.ClientUpdateId = clientUpdateId;
			_request.Messages = new List<MessageDto> ();
			_request.Rooms = string.Empty;
		}

		public void AddMessage(MessageDto message)
		{
			_request.Messages.Add (message);
		}

		public void SetJoinedRooms(IEnumerable<string> rooms)
		{
			_request.Rooms = string.Join (",", rooms);
		}

		public ChatUpdateRequest ToRequest()
		{
			return _request;
		}
	}
}

