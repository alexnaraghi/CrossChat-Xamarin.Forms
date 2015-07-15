using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using SharedSquawk.Client.Model.Entities.Messages;
using SharedSquawk.Client.Model.Proxies;
using SharedSquawk.Client.Seedwork.Extensions;
using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using System.Collections.Generic;
using System.Diagnostics;
using SharedSquawk.Client.Model.Entities;
using SharedSquawk.Client.Model.Helpers;
using SharedSquawk.Server.Application.DataTransferObjects;

namespace SharedSquawk.Client.Model.Managers
{
    public class ChatManager : ManagerBase
    {
        private readonly AccountManager _accountManager;

		private readonly IChatServiceProxy _chatServiceProxy = null;
        private string _subject;
		private string _sessionGuid;
		private ChatUpdateBuilder _updateBuilder;
		private DateTime _lastUpdateTime;

		public int CurrentUpdateRequestCount {
			get;
			private set;
		}

		public int LastServerUpdateId {
			get;
			private set;
		}

		public int UpdateObjectsReceivedCount {
			get;
			private set;
		}


        public ChatManager(
            ConnectionManager connectionManager,
			IChatServiceProxy chatServiceProxy,
            AccountManager accountManager)
            : base(connectionManager)
        {
            _chatServiceProxy = chatServiceProxy;
            _accountManager = accountManager;
			_accountManager.LoggedIn += OnLoggedIn;

            //Messages = new ObservableCollection<Event>();
            OnlineUsers = new ObservableCollection<Profile>();
			UserDirectory = new Dictionary<int, Profile> ();
			Rooms = new RoomCollection ();
			_updateBuilder = new ChatUpdateBuilder ();
        }


		//Keep requesting chat updates as long as we are logged in and have a valid state
		//TODO: unify the login state somehow so we don't have to do so many disparate checks
		//for the same general idea of a user being able to chat.
		public async void SpinOnChatUpdate()
		{
			const int chatUpdateIntervalMs = 8000;
			while (_accountManager.IsLoggedIn && _sessionGuid != null)
			{
				//Make sure that automatic updates only go as fast as the interval,
				//delay it longer if a manual update occurred between now and the last auto update
				var timeSinceLastUpdate = DateTime.Now - _lastUpdateTime;
				if (timeSinceLastUpdate.TotalMilliseconds >= chatUpdateIntervalMs)
				{
					await GetChatUpdate ();
					await Task.Delay (chatUpdateIntervalMs);
				}
				else
				{
					await Task.Delay (chatUpdateIntervalMs - (int)timeSinceLastUpdate.TotalMilliseconds);
				}
			}
		}

        /// <summary>
        /// This is the first step of a logged in user for chatting.
        /// </summary>
        public async Task ReloadUsers()
        {
			ConnectedMembersResponse chatStatus;

			chatStatus = await _chatServiceProxy.GetConnectedMembers(new ConnectedMembersRequest()
				{
					SSID = _accountManager.SSID,
					PracticingLanguages = _accountManager.CurrentUser.PracticingLanguages,
					KnownLanguages = _accountManager.CurrentUser.KnownLanguages,
					LocaleID = _accountManager.CurrentUser.LocaleID,
					Age = _accountManager.CurrentUser.Age,
					Gender = _accountManager.CurrentUser.Gender,
					LastName = _accountManager.CurrentUser.LastName,
					FirstName = _accountManager.CurrentUser.FirstName,
					UserID = _accountManager.CurrentUser.UserId
				});

            OnlineUsers.Clear();

			if (chatStatus == null)
			{
				return;
			}
            
			_sessionGuid = chatStatus.GUID;

			_updateBuilder.Instantiate(_accountManager.CurrentUser.UserId,_sessionGuid);
			_updateBuilder.NewRequest (0, UpdateObjectsReceivedCount, CurrentUpdateRequestCount);

			foreach (var user in chatStatus.Users)
			{
				Profile userProfile = new Profile ();
				AutoMapper.CopyPropertyValues (user, userProfile);
				OnlineUsers.Add(userProfile);
				UserDirectory.Add (userProfile.UserId, userProfile);
			}

			//Get our first update immediately, then start spinning
			await GetChatUpdate ();
			SpinOnChatUpdate ();
        }

		public async Task GetChatUpdate()
		{
			if (_sessionGuid != null && _accountManager.IsLoggedIn)
			{
				_lastUpdateTime = DateTime.Now;

				//This might be slightly smarter if the timer was closer to the request, but this approximation on the 
				//response time should be close enough for the server
				System.Diagnostics.Stopwatch timer = new Stopwatch();
				timer.Start();

				//Send a chat update request
				ChatUpdateResponse chatStatus = await _chatServiceProxy.GetChatUpdate (_updateBuilder.ToRequest ());

				if (chatStatus == null)
				{
					return;
				}

				timer.Stop();
				//HACK: Time is way off, making it more realistic till i figure out how to improve accuracy
				TimeSpan responseTime = timer.Elapsed - TimeSpan.FromMilliseconds(130);

				//Client update count increments by one every time we send a request
				CurrentUpdateRequestCount++;

				//Do some quick checks to make sure we are still logged in and ready for a message
				if (_sessionGuid != null && _accountManager.IsLoggedIn && _updateBuilder != null && chatStatus != null)
				{
					//Every update, we refresh all our typing events
					Rooms.ClearAllTypingEvents ();

					//Clear out our update buffer and increment our packet id's

					//Update our received count by the count in the response
					UpdateObjectsReceivedCount += chatStatus.Updates.Count;

					//Clear out our buffer so we can start a new update request
					_updateBuilder.NewRequest (Math.Abs(responseTime.TotalMilliseconds), UpdateObjectsReceivedCount, CurrentUpdateRequestCount);

					//Consume the response
					LastServerUpdateId = chatStatus.ServerUpdateId;
					foreach (var update in chatStatus.Updates)
					{
						//Parse the update type
						if (update.User != null)
						{
							Profile userProfile = new Profile ();
							AutoMapper.CopyPropertyValues (update.User, userProfile);
							if (!OnlineUsers.Any (p => p.UserId == userProfile.UserId))
							{
								OnlineUsers.Add (userProfile);
							}
							//Else update?
							if (!UserDirectory.ContainsKey (userProfile.UserId))
							{
								UserDirectory.Add (userProfile.UserId, userProfile);
							}
							//Else update?
						}
						else if (update.DisconnectedUser != null)
						{
							//The server sends two things with this update, either the user disconnected from a 
							//room, or disconnected entirely from the server.
							if (update.DisconnectedUser.RoomId != null)
							{
								Rooms.SetStatus (update.DisconnectedUser.RoomId, RoomStatus.OtherUserLeft);
							}
							else
							{
								//Should we remove these users from the directory too?
								OnlineUsers.RemoveAll (t => t.UserId == update.DisconnectedUser.UserId);
							}
						}
						else if (update.FP != null)
						{
							var fp = update.FP;
							Rooms.AddTypingEvent (fp.Room, new TypingEvent () {
								UserName = GetUserFirstName (fp.UserId),
								UserId = fp.UserId
							});
						}
						else if (update.Message != null)
						{
							var message = update.Message;
							var room = message.Room;

							//group the same user's last chats together
							//var lastMessage = Rooms.LastTextMessage (room) as TextMessage;
							//TODO: Fix the bug with grouping chats duplication
							//if (lastMessage != null && lastMessage.UserId == message.UserId)
//							{
//								Rooms.RemoveTextMessage (room, lastMessage);
//								lastMessage.Body += "\n\n" + message.Text;
//								lastMessage.Timestamp = DateTime.Now;
//								Rooms.AddTextMessage (room, lastMessage);
//							}
//							else
							{
								//BUG: Sometimes we are getting some users that whose names we aren't sure about...
								//Need to investigate this.
								var messageUsername = GetUserFirstName (message.UserId);
								Rooms.AddTextMessage (room, new TextMessage () {
									UserName = messageUsername,
									Body = message.Text,
									UserId = message.UserId,
									Timestamp = DateTime.Now
								});
							}
						}
						else if (update.EnteredRoom != null)
						{
							var enteredRoom = update.EnteredRoom;
							var room = update.EnteredRoom.Room;

							Rooms.SetStatus (room, RoomStatus.Connected);
							Rooms.AddTextMessageRange (room, enteredRoom.RoomMessages.Select (r =>
								new TextMessage () {
								UserName = r.User.Name,
								Body = r.Message.Body,
								UserId = null, //How can we get this?  only the first name is sent, so we can't resolve duplicate names
								Timestamp = DateTime.Now
							}));
											
						}
						else if (update.UserChatRequest != null)
						{
							var request = update.UserChatRequest;

							var roomId = RoomFactory.GetRoomId (request.UserId, _accountManager.CurrentUser.UserId);
							//do something
							if (!Rooms.ContainsKey (roomId))
							{
								Rooms.CreateRoom (new Room(roomId, GetUserFullName(request.UserId)){ UserId = request.UserId });
							}
							Rooms.SetStatus (roomId, RoomStatus.AwaitingOurApproval);
						}
						else if (update.UserChatResult != null)
						{
							var chatResult = update.UserChatResult;
							if (!UserDirectory.ContainsKey (chatResult.UserId))
							{
								//If for some reason we got a chat result from a user that isn't connected,
								//I think we should just ignore it
								break;
							}

							var roomId = RoomFactory.GetRoomId(_accountManager.CurrentUser.UserId, chatResult.UserId);

							switch (chatResult.Reply)
							{
							case ChatReply.Ok:
								{
									_updateBuilder.AddAcceptedUserRoom (roomId);


									//So, we can be getting responses from a different session.  In that case,
									//we will need to spawn up a room and act as if we always intended to get
									//this chat accpetance
									if (!Rooms.ContainsKey (roomId))
									{
										Rooms.CreateRoom (new Room(roomId, GetUserFullName(chatResult.UserId)){ UserId = chatResult.UserId });
									}

									Rooms.SetStatus (roomId, RoomStatus.Connected);
								}
								break;
							case ChatReply.No:
								Rooms.SetStatus (roomId, RoomStatus.OtherUserDeclined);
								break;
							default:
								throw new NotSupportedException ("Chat reply is not known");
								break;
							}
						}
						else
						{
							throw new InvalidOperationException ("An update was sent with no parameter");
						}
					}

				}
			}
			
		}

		public async Task SendMessage(string text, string room)
		{
			_updateBuilder.AddMessage(new MessageDto
				{
					Text = text,
					Room = room,
					UserId = _accountManager.CurrentUser.UserId
				});

			//Manually force update the chat, to send our message asap
			await GetChatUpdate ();

			//Add our message locally after the update for an accurate ordering
			Rooms.AddTextMessage(room, new TextMessage
				{
					UserId = _accountManager.CurrentUser.UserId,
					Body = text,
					Timestamp = DateTime.Now,
					UserName = _accountManager.CurrentUser.FirstName + " " + _accountManager.CurrentUser.LastName
				});
		}

		public async Task SendTypingEvent(string room)
		{
			//TODO
		}

		public async Task<RoomData> JoinPublicRoom(Room room)
		{
			if (!Rooms.Any (r => r.Value.Room.RoomId == room.RoomId))
			{
				Rooms.CreateRoom (room);
				_updateBuilder.AddPublicRoom (room.RoomId);

				//Manually force update the chat, to send our message asap
				await GetChatUpdate ();
			}
			//Otherwise we already have the room in our active chats, nothing to do here

			return Rooms [room.RoomId];
		}

		public async Task<RoomData> RequestUserChat(int userId)
		{
			var otherUserName = GetUserFullName (userId);
			var room = RoomFactory.Get (_accountManager.CurrentUser.UserId, userId, otherUserName);

			if (!Rooms.Any (r => r.Value.Room.RoomId == room.RoomId))
			{
				Rooms.CreateRoom (room);
				_updateBuilder.AddUserRoomRequest (userId);
			}

			//Manually force update the chat, to send our message asap
			await GetChatUpdate ();

			return Rooms [room.RoomId];
		}
			
		public async Task ApproveUserChat(int userId)
		{
			var creatorUserId = RoomFactory.GetRoomId (userId, _accountManager.CurrentUser.UserId);

			_updateBuilder.AcceptChatRequest (userId, creatorUserId);

			//Wait for the room to be entered by the server now
			Rooms.SetStatus (creatorUserId, RoomStatus.Waiting);

			//Manually force update the chat, to send our message asap
			await GetChatUpdate ();
		}

		public async Task DeclineUserChat(int userId)
		{
			_updateBuilder.DeclineChatRequest (userId);

			//Cleanup in case the room is already active
			LeaveRoom(userId);

			//Manually force update the chat, to send our message asap
			await GetChatUpdate ();
		}

		public void LeaveRoom(string roomId)
		{
			_updateBuilder.RemoveRoom (roomId);
			if (Rooms.ContainsKey (roomId))
			{
				Rooms.RemoveRoom (roomId);
			}
		}

		public void LeaveRoom(int userId)
		{
			//We aren't sure who made the room based on this context, just leave either combo
			//and don't worry about it
			var roomId = RoomFactory.GetRoomId (_accountManager.CurrentUser.UserId, userId);
			LeaveRoom (roomId);
			var roomId2 = RoomFactory.GetRoomId (userId, _accountManager.CurrentUser.UserId);
			LeaveRoom (roomId2);
		}

		public async Task<Profile> GetMemberDetails(int userId)
		{
			Profile profile;

			bool didFindMemberDetails = false;
			if (UserDirectory.ContainsKey (userId))
			{
				profile = UserDirectory [userId];

				//If we have all the data already in our directory, pass it along
				if (profile.Details != null)
				{
					didFindMemberDetails = true;
				}
			}
			else
			{
				//hmmm this seems like a weird desync issue.  How can we know the user id of a
				//user not in the directory?
				profile = null;
			}

			if (profile != null && !didFindMemberDetails)
			{
				//We don't have the profile details, grab it
				ProfileResponse response;

				try
				{
					response = await _chatServiceProxy.GetUserProfile(new ProfileRequest()
					{
						UserId = userId
					});
				}
				catch(AggregateException ex)
				{
					throw ex.Flatten ();
				}

				ProfileDetails details = new ProfileDetails ();
				AutoMapper.CopyPropertyValues (response, details);

				//Populating the profile details will change the entry in both the directory and the user list.
				profile.Details = details;
			}

			return profile;
		}

		public void OnLoggedIn(object sender, EventArgs e)
		{
			CurrentUpdateRequestCount = 1;
			UpdateObjectsReceivedCount = 0;
		}

		/// <summary>
		/// Fires on subject change
		/// </summary>
		public event EventHandler SubjectChanged = delegate { };

		/// <summary>
		/// Chat topic (subject)
		/// </summary>
		public string Subject
		{
			get { return _subject; }
			private set
			{
				_subject = value;
				SubjectChanged(this, EventArgs.Empty);
			}
		}
		
		//Helper
		private string GetUserFirstName(int userId)
		{
			return UserDirectory.ContainsKey (userId) 
			? UserDirectory [userId].FirstName 
				: string.Format("Unknown({0})", userId);
		}

		private string GetUserFullName(int userId)
		{
			if (UserDirectory.ContainsKey (userId))
			{
				var user = UserDirectory [userId];
				return user.FirstName + " " + user.LastName;
			}
			else
			{
				return string.Format("Unknown({0})", userId);
			}
		}

		//public ObservableCollection<Event> Messages { get; private set; }
		public RoomCollection Rooms { get; private set; }

		public ObservableCollection<Profile> OnlineUsers { get; private set; }

		public Dictionary<int,Profile> UserDirectory{ get; private set; }



		/// <summary>
		/// Reloads only messages and subject
		/// </summary>
		//        public async Task ReloadChat()
		//        {
		//var chatStatus = await _chatServiceProxy.GetLastMessages(new LastMessageRequest());
		//Subject = chatStatus.Subject;
		//Messages.Clear();
		//if (chatStatus.Messages != null)
		//{
		//Messages.AddRange(chatStatus.Messages.Select(ToEntity<TextMessage>));
		//}
		//        }

		/*
        /// <summary>
        /// Kick & ban (only for admins)
        /// </summary>
        public async Task<bool> Ban(int playerId, string reason)
        {
            var result = await _chatServiceProxy.Ban(new BanRequest { Reason = reason, TargetUserId = playerId, Ban = true });
            return result.Result == BanResponseType.Success;
        }

        /// <summary>
        /// Kick & ban (only for admins)
        /// </summary>
        public async Task<bool> UnBan(int playerId)
        {
            var result = await _chatServiceProxy.Ban(new BanRequest { TargetUserId = playerId });
            return result.Result == BanResponseType.Success;
        }

        /// <summary>
        /// Shut somebody up (only for moders)
        /// </summary>
        public async Task<bool> Devoice(int playerId, string reason)
        {
            var result = await _chatServiceProxy.Devoice(new DevoiceRequest { Reason = reason, TargetUserId = playerId, Devoice = true });
            return result.Result == DevoiceResponseType.Success;
        }

        /// <summary>
        /// Shut somebody up (only for moders)
        /// </summary>
        public async Task<bool> BringVoiceBack(int playerId)
        {
            var result = await _chatServiceProxy.Devoice(new DevoiceRequest { TargetUserId = playerId });
            return result.Result == DevoiceResponseType.Success;
        }

        /// <summary>
        /// Reset photo (only for moders)
        /// </summary>
        public async Task<bool> ResetPhoto(int playerId)
        {
            var result = await _chatServiceProxy.ResetPhoto(new ResetPhotoRequest { TargetId = playerId });
            return result.Success;
        }

        /// <summary>
        /// Send a public message
        /// </summary>
        public void SendMessage(string message)
        {
            _chatServiceProxy.PublicMessage(new PublicMessageRequest { Body = message });
        }

        public Task SendImage(byte[] image)
        {
            return _chatServiceProxy.SendImage(new SendImageRequest { Image = image });
        }
		*/

        
		/*
        protected override void OnUnknownDtoReceived(BaseDto dto)
        {
            //messages:
            ToEntityAndAddToList<BanNotificationEvent, Event, BanNotification>(dto, Messages, null, false);
            ToEntityAndAddToList<TextMessage, Event, PublicMessageDto>(dto, Messages, null, false);
            ToEntityAndAddToList<DevoiceNotificationEvent, Event, DevoiceNotification>(dto, Messages, null, false);
            ToEntityAndAddToList<GrantedModershipNotificationEvent, Event, ModershipGrantedInfo>(dto, Messages, null, false);
            ToEntityAndAddToList<RemovedModershipNotificationEvent, Event, ModershipRemovedInfo>(dto, Messages, null, false);

            var userDto = dto as UserDto;
            if (userDto != null)
                OnlineUsers.Add(userDto);

            var joinedUserDto = dto as JoinedUserInfo;
            if (joinedUserDto != null)
                OnlineUsers.Add(joinedUserDto.User);

            var leftUserInfo = dto as LeftUserInfo;
            if (leftUserInfo != null)
                RemoveEntityFromList(OnlineUsers, i => i.Id == leftUserInfo.UserId);

            var playerProfileChanges = dto as UserPropertiesChangedInfo;
            if (playerProfileChanges != null)
            {
                UpdatePropertiesForList(OnlineUsers, p => p.Id == playerProfileChanges.UserId, playerProfileChanges.Properties);
            }

			
            //update property IsDevoiced for players
            var devoiceNotification = dto as DevoiceNotification;
            if (devoiceNotification != null)
            {
                var player = OnlineUsers.FirstOrDefault(i => i.Id == devoiceNotification.TargetId);
                if (player != null)
                    player.IsDevoiced = devoiceNotification.Devoice;
            }
			

            var youAreDevoicedNotification = dto as YouAreDevoicedNotification;
            if (youAreDevoicedNotification != null)
            {
                //TODO: Notify current user
            }
        }
		*/
    }
}
