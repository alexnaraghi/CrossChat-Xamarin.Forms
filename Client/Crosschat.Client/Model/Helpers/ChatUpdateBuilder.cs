using System;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using System.Collections.Generic;
using System.Linq;

namespace Crosschat.Client
{
	public class ChatUpdateBuilder
	{
		private ChatUpdateRequest _request;
		private List<string> _activeRooms;
		private List<string> _enteredRooms;
		public ChatUpdateBuilder (string d, int userId, string guid, int lastServerUpdateId, int clientUpdateId)
		{
			_request = new ChatUpdateRequest () {
				D = d,
				UserId = userId,
				GUID = guid,
				LastServerUpdateId = lastServerUpdateId,
				ClientUpdateId = clientUpdateId,
				Messages = new List<MessageDto>(),
				ActiveRooms = "", //TODO: Filter by rooms
				EnteredRooms = new List<RoomEntryRequestDto>()
			};
			_activeRooms = new List<string> ();
			_enteredRooms = new List<string> ();
		}

		public void NewRequest(int lastServerUpdateId, int clientUpdateId)
		{
			_request.LastServerUpdateId = lastServerUpdateId;
			_request.ClientUpdateId = clientUpdateId;
			_request.Messages = new List<MessageDto> ();
			_request.EnteredRooms.Clear ();
			_request.ActiveRooms = null;

			//When a new request is started, all rooms entered become part of the active room list
			_activeRooms.AddRange (_enteredRooms);
			_enteredRooms.Clear ();
		}

		public void AddMessage(MessageDto message)
		{
			_request.Messages.Add (message);
		}

		public void AddRoom(string room)
		{
			_enteredRooms.Add (room);
		}

		public void RemoveRoom(string room)
		{
			if (_enteredRooms.Contains (room))
			{
				_enteredRooms.Remove (room);
			}
			if (_activeRooms.Contains (room))
			{
				_activeRooms.Remove (room);
			}
		}

		public ChatUpdateRequest ToRequest()
		{
			//Entered Rooms
			_request.EnteredRooms.Clear ();
			foreach (var room in _enteredRooms)
			{
				_request.EnteredRooms.Add (new RoomEntryRequestDto{ Room = room });
			}

			//Active Rooms
			_request.ActiveRooms = string.Join ("|", _activeRooms);

			return _request;
		}
	}
}

