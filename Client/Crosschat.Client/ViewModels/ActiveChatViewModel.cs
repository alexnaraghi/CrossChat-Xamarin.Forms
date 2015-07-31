using System;
using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Model.Entities;
using SharedSquawk.Client.Model.Entities.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using SharedSquawk.Client.Model.Managers;
using System.Text;
using System.Collections.Specialized;
using Xamarin.Forms;

namespace SharedSquawk.Client.ViewModels
{
	public class ActiveChatViewModel : ViewModelBase
	{
		private ApplicationManager _manager;
		private Room _room;
		private RoomData _roomData;
		private RoomStatus _roomStatus;
		private ObservableCollection<TextMessage> _messages;

		public ActiveChatViewModel (ApplicationManager manager, RoomData roomData)
		{
			_room = roomData.Room;
			_roomData = roomData;
			_manager = manager;
			_messages = roomData.TextMessages;
			_roomStatus = roomData.Status;
			roomData.PropertyChanged += RoomDataChanged;
			_messages.CollectionChanged += TextMessagesChanged;
			Raise ("DescriptionText");
		}

		void RoomDataChanged (object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			const string statusString = "Status"; //Upgrade C#, use nameof
			if (e.PropertyName == statusString)
			{
				_roomStatus = _roomData.Status;
				Raise ("DescriptionText");
			}
		}

		void TextMessagesChanged (object sender, NotifyCollectionChangedEventArgs e)
		{
			//We could be more precise here if we want, but I think it should be ok
			//for this view model just to refresh all the text on any collection change.
			Raise ("DescriptionText");
		}

		public bool HasUnreadMessages
		{
			get 
			{
				throw new NotImplementedException("HasUnreadMessages not yet implemented");
			}
		}

		public string Name
		{
			get{ return _room.Name; }
		}

		//For comparisons
		public string RoomId
		{
			get{ return _room.RoomId; }
		}


		public RoomData RoomData
		{
			get{ return _roomData; }
		}

		public string DescriptionText
		{
			get
			{
				return GetUpdatedStatusMessage();	
			}
		}

		private string GetUpdatedStatusMessage()
		{
			string message;
			switch (_roomStatus)
			{
			case RoomStatus.Waiting:
				if (_room.UserId == null)
				{
					//Public chat
					message = "Joining...";
				}
				else
				{
					//Private chat
					message = "Waiting for the other user to join...";
				}
				break;
			case RoomStatus.Connected:
				message = GetLastMessageString();
				break;
			case RoomStatus.OtherUserLeft:
				message = "User has left.";
				break;
			case RoomStatus.OtherUserDeclined:
				message = "The user declined your chat";
				break;
			case RoomStatus.AwaitingOurApproval:
				message = "Invites you to a chat.";
				break;
			case RoomStatus.Error:
				message = "There was an error in this chat.";
				break;
			default:
				throw new NotSupportedException ("Unknown room status in the ActiveChatViewModel");
			}
			return "    " + message;
		}

		private string GetLastMessageString()
		{
			var lastMesage = GetLastMessageFromOtherUser ();
			if (lastMesage == null)
			{
				if (_room.UserId == null)
				{
					return "Start chatting!";
				}
				else
				{
					//User Chat
					return "The other user has joined, start chatting!";
				}

			}

			StringBuilder sb = new StringBuilder ();
			if (_room.UserId == null)
			{
				//Public chat
				sb.Append(lastMesage.UserName).Append(" · ");
			}

			int bodyLimit = Device.Idiom == TargetIdiom.Tablet ? 150 : 75;
			bool isLongerThanLimit = lastMesage.Body.Length > bodyLimit;

			sb.Append(lastMesage.Body.CutIfLonger(bodyLimit))
				.Append(isLongerThanLimit ? "..." : "");
			return sb.ToString();
		}

		private TextMessage GetLastMessageFromOtherUser()
		{
			return _messages.LastOrDefault (
				m => m.UserId != _manager.AccountManager.CurrentUser.UserId);
		}
	}
}

