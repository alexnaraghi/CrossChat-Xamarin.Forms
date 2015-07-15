using System;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using System.Collections.Generic;
using System.Linq;
using SharedSquawk.Server.Application.DataTransferObjects;

namespace SharedSquawk.Client
{
	public class ChatUpdateBuilder
	{
		private ChatUpdateRequest _request;
		private List<string> _activeRooms;
		private List<string> _enteredRooms;
		private List<int> _enteredUserChatRequests;

		private bool _isInstantiated;
		public ChatUpdateBuilder ()
		{
			_request = new ChatUpdateRequest () {
				Messages = new List<MessageDto>(),
				ActiveRooms = string.Empty,
				EnteredRooms = new List<RoomEntryRequestDto>(),
				UserChatRequests = new List<UserChatDto>(),
				UserChatResponses = new List<ChatResultDto>()
			};
			_activeRooms = new List<string> ();
			_enteredRooms = new List<string> ();
			_enteredUserChatRequests = new List<int> ();
		}

		public void Instantiate(int userId, string guid)
		{
			_request.UserId = userId;
			_request.GUID = guid;
			_request.Messages.Clear ();
			_request.ActiveRooms = string.Empty;
			_request.EnteredRooms.Clear ();
			_request.UserChatRequests.Clear ();
			_request.UserChatResponses.Clear ();
			_activeRooms.Clear ();
			_enteredRooms.Clear ();
			_enteredUserChatRequests.Clear ();

			_isInstantiated = true;

		}

		public void NewRequest(double delayMs, int lastServerUpdateId, int clientUpdateId)
		{
			_request.Delay = delayMs.ToString("0.###");
			_request.LastServerUpdateId = lastServerUpdateId;
			_request.ClientUpdateId = clientUpdateId;
			_request.Messages = new List<MessageDto> ();
			_request.EnteredRooms.Clear ();
			_request.UserChatRequests.Clear ();
			_request.UserChatResponses.Clear ();
			_request.ActiveRooms = null;

			//When a new request is started, all rooms entered become part of the active room list
			_activeRooms.AddRange (_enteredRooms);
			_enteredRooms.Clear ();
			_enteredUserChatRequests.Clear ();
		}

		public void AddMessage(MessageDto message)
		{
			_request.Messages.Add (message);
		}

		public void AddPublicRoom(string room)
		{
			_enteredRooms.Add (room);
		}

		public void AddUserRoomRequest(int userId)
		{
			_enteredUserChatRequests.Add (userId);
		}

		public void AddAcceptedUserRoom(string room)
		{
			_enteredRooms.Add (room);
		}

		public void AcceptChatRequest(int userId, string roomId)
		{
			_request.UserChatResponses.Add(new ChatResultDto
			{
				UserId = userId,
				Reply = ChatReply.Ok
			});
			_enteredRooms.Add (roomId);
		}

		public void DeclineChatRequest(int userId)
		{
			_request.UserChatResponses.Add(new ChatResultDto
				{
					UserId = userId,
					Reply = ChatReply.No
				});
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

			_request.UserChatRequests.Clear ();
			foreach (var userId in _enteredUserChatRequests)
			{
				_request.UserChatRequests.Add (new UserChatDto{ UserId = userId });
			}

			//Active Rooms
			_request.ActiveRooms = string.Join (",", _activeRooms);

			return _request;
		}
	}
}

