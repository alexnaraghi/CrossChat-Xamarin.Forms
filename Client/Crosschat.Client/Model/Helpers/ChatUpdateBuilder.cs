using System;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using System.Collections.Generic;
using System.Linq;

namespace SharedSquawk.Client
{
	public class ChatUpdateBuilder
	{
		private ChatUpdateRequest _request;
		private List<string> _activeRooms;
		private List<string> _enteredRooms;
		private bool _isInstantiated;
		public ChatUpdateBuilder ()
		{
			_request = new ChatUpdateRequest () {
				Messages = new List<MessageDto>(),
				ActiveRooms = string.Empty,
				EnteredRooms = new List<RoomEntryRequestDto>()
			};
			_activeRooms = new List<string> ();
			_enteredRooms = new List<string> ();
		}

		public void Instantiate(int userId, string guid)
		{
			_request.UserId = userId;
			_request.GUID = guid;
			_request.Messages.Clear ();
			_request.ActiveRooms = string.Empty;
			_request.EnteredRooms.Clear ();
			_activeRooms.Clear ();
			_enteredRooms.Clear ();

			_isInstantiated = true;

		}

		public void NewRequest(double delay, int lastServerUpdateId, int clientUpdateId)
		{
			_request.Delay = delay;
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
			_request.ActiveRooms = string.Join (",", _activeRooms);

			return _request;
		}
	}
}

