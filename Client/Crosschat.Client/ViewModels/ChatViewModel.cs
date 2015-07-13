using System;
using Crosschat.Client.Seedwork;
using Crosschat.Client.Model.Managers;
using System.Windows.Input;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using Crosschat.Client.Model.Entities.Messages;
using Crosschat.Client.Seedwork.Extensions;
using System.Collections.Specialized;
using System.Linq;
using Crosschat.Client.Views;

namespace Crosschat.Client.ViewModels
{
	public class ChatViewModel : ViewModelBase
	{
		private readonly ApplicationManager _appManager;
		private ObservableCollection<EventViewModel> _events;
		private Room _room;
		private string _inputText;
		private readonly EventViewModelFactory _eventViewModelFactory;

		public ChatViewModel (ApplicationManager appManager, Room room)
		{
			_room = room;
			_appManager = appManager;
			_eventViewModelFactory = new EventViewModelFactory();
			_events = new ObservableCollection<EventViewModel>();

			var chatRoomModel = _appManager.ChatManager.Rooms [room.RoomId];
			//Yikes, every time we switch rooms, we are creating EventViewModels for potentially hundreds of messages.
			//This could result in some major garbage.  Keep an eye on performance, maybe we will need to drop the view model
			//approach for messages.
			chatRoomModel.SynchronizeWith(Events, i => _eventViewModelFactory.Get(i, _appManager.AccountManager.CurrentUser.UserId));
		}

		protected override void OnShown ()
		{
			//Scroll to the bottom of the list whenever we get a new message
			//Future idea: scroll to bottom only when the user is already at the bottom, so they can browse messages easier.
			Events.CollectionChanged += (s, e) =>
			{
				if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems != null)
				{
					ScrollToBottom();
				}
			};

			ScrollToBottom ();
			base.OnShown ();
		}

		void ScrollToBottom ()
		{
			var chatPage = _currentPage as ChatPage;
			if( chatPage != null && Events.Count > 0)
			{
				chatPage.OnItemAdded(Events.Last());
			}
		}

		public string RoomName
		{
			get { return _room.Name; }
		}

		public string InputText
		{
			get { return _inputText; }
			set { SetProperty(ref _inputText, value); }
		}

		public ObservableCollection<EventViewModel> Events {
			get { return _events; }
			set { SetProperty(ref _events, value); }
		}

		public ICommand SendMessageCommand
		{
			get { return new Command(OnSendMessage); }
		}

		private async void OnSendMessage()
		{
			if (string.IsNullOrEmpty(InputText))
				return;
			string text = InputText;
			InputText = string.Empty;

			IsBusy = true;
			await _appManager.ChatManager.SendMessage(text, _room.RoomId);
			IsBusy = false;
		}

		public ICommand UpdateCommand
		{
			get{ return new Command (OnForceUpdate); }
		}

		private async void OnForceUpdate()
		{
			IsBusy = true;
			await _appManager.ChatManager.GetChatUpdate ();
			IsBusy = false;
		}


	}
}

