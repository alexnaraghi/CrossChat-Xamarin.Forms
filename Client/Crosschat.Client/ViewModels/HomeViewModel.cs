using System.Collections.ObjectModel;
using System.Windows.Input;
using Crosschat.Client.Model.Contracts;
using Crosschat.Client.Model.Managers;
using Crosschat.Client.Seedwork;
using Crosschat.Client.Seedwork.Extensions;
using Xamarin.Forms;
using System.Collections.Generic;
using Crosschat.Client.Model;
using System;

namespace Crosschat.Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ApplicationManager _appManager;
        private readonly EventViewModelFactory _eventViewModelFactory;
        private readonly IPhotoPicker _photoPicker;
        private ObservableCollection<UserViewModel> _users;
		private ObservableCollection<Room> _publicRooms;
        //private ObservableCollection<EventViewModel> _events;
        private string _inputText;
        private string _subject;

        public HomeViewModel(ApplicationManager appManager)
        {
            _appManager = appManager;
            Users = new ObservableCollection<UserViewModel>();
            //Events = new ObservableCollection<EventViewModel>();
            _eventViewModelFactory = new EventViewModelFactory();
            _photoPicker = DependencyService.Get<IPhotoPicker>();
            LoadData();
        }

        private async void LoadData()
        {
            IsBusy = true;
            await _appManager.ChatManager.ReloadChat();
            await _appManager.ChatManager.ReloadUsers();
            Subject = _appManager.ChatManager.Subject;

            _appManager.ChatManager.OnlineUsers.SynchronizeWith(Users, u => new UserViewModel(u));
			//_appManager.ChatManager.Messages.SynchronizeWith(Events, i => _eventViewModelFactory.Get(i, _appManager.AccountManager.CurrentUser.UserId));
            IsBusy = false;

			PublicRooms = new ObservableCollection<Room> ();
			PublicRooms.AddRange (PublicRoomsRepository.GetAll ());
        }

        public ObservableCollection<UserViewModel> Users
        {
            get { return _users; }
            set { SetProperty(ref _users, value); }
        }

		public ObservableCollection<Room> PublicRooms
		{
			get { return _publicRooms; }
			set { SetProperty(ref _publicRooms, value); }
		}


        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }


		public ICommand SelectRoomCommand
		{
			get { return new Command(OnSelectRoom); }
		}

		private async void OnSelectRoom(object roomArg)
		{
			var room = roomArg as Room;
			if (room == null)
			{
				throw new Exception ("Selected item was not a room");
			}
			await _appManager.ChatManager.JoinRoom (room.RoomId);
			var model = new ChatViewModel (_appManager, room);
			await model.ShowAsync ();
		}

		#if DEBUG
		public string User
		{
			get{ return "User : " + _appManager.AccountManager.CurrentUser.FirstName + " " + _appManager.AccountManager.CurrentUser.LastName; }
		}

		public string SSID
		{
			get{ return "Session ID : " + _appManager.AccountManager.SSID.ToString(); }
		}
		#endif


		/*


        private async void OnSendImage()
        {
            var imageData = await _photoPicker.PickPhoto();
            IsBusy = true;
            //await _appManager.ChatManager.SendImage(imageData);
            IsBusy = false;
        }
		public ICommand InviteCommand
		{
			get { return new Command(() => new InviteToAppViewModel().ShowAsync());}
		}

		public ICommand SendImageCommand
		{
			get { return new Command(OnSendImage); }
		}*/
		/*
        private async void OnSendMessage()
        {
            if (string.IsNullOrEmpty(InputText))
                return;
            string text = InputText;
            InputText = string.Empty;

			IsBusy = true;
            await _appManager.ChatManager.SendMessage(text, "CREn");
			IsBusy = false;
        }
        */

		/*
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
		*/

		/*
		public ObservableCollection<EventViewModel> Events
		{
			get { return _events; }
			set { SetProperty(ref _events, value); }
		}
		*/

		/*
		public string InputText
		{
			get { return _inputText; }
			set { SetProperty(ref _inputText, value); }
		}
		*/


		/*
		public ICommand SendMessageCommand
		{
			get { return new Command(OnSendMessage); }
		}
		*/
    }
}
