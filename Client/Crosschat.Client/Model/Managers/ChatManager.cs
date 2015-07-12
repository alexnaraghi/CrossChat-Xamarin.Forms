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

namespace Crosschat.Client.Model.Managers
{
    public class ChatManager : ManagerBase
    {
        private readonly AccountManager _accountManager;

		private readonly IChatServiceProxy _chatServiceProxy = null;
        private string _subject;
		private string _sessionGuid;
		private ChatUpdateBuilder _updateBuilder;

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

            Messages = new ObservableCollection<Event>();
            OnlineUsers = new ObservableCollection<UserDto>();
        }


		//Keep requesting chat updates as long as we are logged in and have a valid state
		//TODO: unify the login state somehow so we don't have to do so many disparate checks
		//for the same general idea of a user being able to chat.
		public async void SpinOnChatUpdate()
		{
			const int chatUpdateIntervalMs = 8000;
			while (_accountManager.IsLoggedIn && _sessionGuid != null)
			{
				GetChatUpdate ();
				await Task.Delay(chatUpdateIntervalMs);
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
			OnlineUsers.AddRange(chatStatus.Users);
        }

		public async Task GetChatUpdate()
		{
			if (_sessionGuid != null && _accountManager.IsLoggedIn)
			{
			
				//Send a chat update request
				var chatStatus = await _chatServiceProxy.GetChatUpdate (_updateBuilder.ToRequest ());

				//Client update count increments by one every time we send a request
				CurrentUpdateRequestCount++; 

				//Do some quick checks to make sure we are still logged in and ready for a message
				if (_sessionGuid != null && !_accountManager.IsLoggedIn && _updateBuilder != null && chatStatus != null)
				{
					//Clear out our update buffer and increment our packet id's

					//Update our received count by the count in the response
					UpdateObjectsReceivedCount += chatStatus.Updates.Count;

					//Clear out our buffer so we can start a new update request
					_updateBuilder.Clear (UpdateObjectsReceivedCount, CurrentUpdateRequestCount);

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
							//Just throw everything in the same lobby for now, no filtering
							Messages.Add(new TextMessage()
								{
									Body = message.Text,
									UserId = message.UserId,
									Timestamp = DateTime.Now
								});
						}
						else if (update.EnteredRoom != null)
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

		public void OnLoggedIn(object sender, EventArgs e)
		{
			_updateBuilder = new ChatUpdateBuilder (
				//What is this?  ping Last packet delay?
				"182.4189453125",
				_accountManager.CurrentUser.UserId,
				_sessionGuid,
				UpdateObjectsReceivedCount,
				CurrentUpdateRequestCount);
			UpdateObjectsReceivedCount = 0;
		}

		public void OnLoggedOut(object sender, EventArgs e)
		{
			_updateBuilder = null;
			_sessionGuid = null;
		}

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

        public ObservableCollection<Event> Messages { get; private set; }

        public ObservableCollection<UserDto> OnlineUsers { get; private set; }

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
