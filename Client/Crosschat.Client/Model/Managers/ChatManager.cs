using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Crosschat.Client.Model.Entities.Messages;
using Crosschat.Client.Model.Proxies;
using Crosschat.Client.Seedwork.Extensions;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using System.Collections.Generic;

namespace Crosschat.Client.Model.Managers
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
            OnlineUsers = new ObservableCollection<UserDto>();
			UserDirectory = new Dictionary<int, UserDto> ();
			Rooms = new RoomCollection ();
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
        /// Reloads only messages and subject
        /// </summary>
        public async Task ReloadChat()
        {
            //var chatStatus = await _chatServiceProxy.GetLastMessages(new LastMessageRequest());
            //Subject = chatStatus.Subject;
            //Messages.Clear();
            //if (chatStatus.Messages != null)
            //{
                //Messages.AddRange(chatStatus.Messages.Select(ToEntity<TextMessage>));
            //}
        }

        /// <summary>
        /// Reloads only online players
        /// </summary>
        public async Task ReloadUsers()
        {
			var chatStatus = await _chatServiceProxy.GetConnectedMembers(new ConnectedMembersRequest()
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
            
			_sessionGuid = chatStatus.GUID;

			_updateBuilder = new ChatUpdateBuilder (
				//What is this?  ping Last packet delay?
				"0",
				_accountManager.CurrentUser.UserId,
				_sessionGuid,
				UpdateObjectsReceivedCount,
				CurrentUpdateRequestCount);
			
			OnlineUsers.AddRange(chatStatus.Users);
			foreach (var user in chatStatus.Users)
			{
				UserDirectory.Add (user.UserID, user);
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

				//Send a chat update request
				var chatStatus = await _chatServiceProxy.GetChatUpdate (_updateBuilder.ToRequest ());

				//Client update count increments by one every time we send a request
				CurrentUpdateRequestCount++;

				//Do some quick checks to make sure we are still logged in and ready for a message
				if (_sessionGuid != null && _accountManager.IsLoggedIn && _updateBuilder != null && chatStatus != null)
				{
					//Clear out our update buffer and increment our packet id's

					//Update our received count by the count in the response
					UpdateObjectsReceivedCount += chatStatus.Updates.Count;

					//Clear out our buffer so we can start a new update request
					_updateBuilder.NewRequest (UpdateObjectsReceivedCount, CurrentUpdateRequestCount);

					//Consume the response
					LastServerUpdateId = chatStatus.ServerUpdateId;
					foreach (var update in chatStatus.Updates)
					{
						//Parse the update type
						if (update.User != null)
						{
							OnlineUsers.Add (update.User);
						}
						else if (update.DisconnectedUser != null)
						{
							OnlineUsers.RemoveAll (t => t.UserID == update.DisconnectedUser.UserId);	
						}
						else if (update.FP != null)
						{
							//Is this for typing?
						}
						else if (update.Message != null)
						{
							var message = update.Message;
							var room = message.Room;

							//group the same user's last chats together
							var lastMessage = Rooms.Last(room) as TextMessage;
							if (lastMessage != null && lastMessage.UserId == message.UserId)
							{
								Rooms.RemoveMessage(room, lastMessage);
								lastMessage.Body += "\n\n" + message.Text;
								lastMessage.Timestamp = DateTime.Now;
								Rooms.AddMessage (room, lastMessage);
							}
							else
							{
								//BUG: Sometimes we are getting some users that whose names we aren't sure about...
								//Need to investigate this.
								var messageUsername = UserDirectory.ContainsKey (message.UserId) 
									? UserDirectory [message.UserId].FirstName 
									: string.Format("Unknown({0})", message.UserId);
								Rooms.AddMessage (room, new TextMessage () {
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

							//Make the room exist, no matter how many messages there are
							if (!Rooms.ContainsKey (room))
							{
								Rooms.Add(room, new ObservableCollection<Event>());
							}

							Rooms.AddMessageRange(room, enteredRoom.RoomMessages.Select( r =>
								new TextMessage(){
									UserName = r.User.Name,
									Body = r.Message.Body,
									UserId = null, //How can we get this?  only the first name is sent, so we can't resolve duplicate names
									Timestamp = DateTime.Now
								}));
							//do something				
						}
						else if (update.UserChatRequest != null)
						{
							//do something
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
			Rooms.AddMessage(room, new TextMessage
				{
					UserId = _accountManager.CurrentUser.UserId,
					Body = text,
					Timestamp = DateTime.Now,
					UserName = _accountManager.CurrentUser.FirstName + " " + _accountManager.CurrentUser.LastName
				});
		}

		public async Task JoinRoom(string roomId)
		{
			_updateBuilder.AddRoom (roomId);
			//Manually force update the chat, to send our message asap
			await GetChatUpdate ();
		}

		public async Task LeaveRoom(string roomId)
		{
			_updateBuilder.RemoveRoom (roomId);
			//Manually force update the chat, to send our message asap
			await GetChatUpdate ();
		}

		public void OnLoggedIn(object sender, EventArgs e)
		{
			CurrentUpdateRequestCount = 1;
			UpdateObjectsReceivedCount = 0;
		}

		public void OnLoggedOut(object sender, EventArgs e)
		{
			_updateBuilder = null;
			_sessionGuid = null;
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

		//public ObservableCollection<Event> Messages { get; private set; }
		public RoomCollection Rooms { get; private set; }

		public ObservableCollection<UserDto> OnlineUsers { get; private set; }

		public Dictionary<int,UserDto> UserDirectory{ get; private set; }

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
