using System.Collections.ObjectModel;
using System.Windows.Input;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Managers;
using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Seedwork.Extensions;
using Xamarin.Forms;
using System.Collections.Generic;
using SharedSquawk.Client.Model;
using System;
using SharedSquawk.Client.Model.Entities;
using SharedSquawk.Client.Model.Helpers;

namespace SharedSquawk.Client.ViewModels
{
    public class HomeViewModel : ViewModelBase
    {
        private readonly ApplicationManager _appManager;
        private readonly EventViewModelFactory _eventViewModelFactory;
        private readonly IPhotoPicker _photoPicker;
        private ObservableCollection<UserViewModel> _users;
		private ObservableCollection<Room> _publicRooms;
		private ObservableCollection<Room> _activeChats;
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
			_publicRooms = new ObservableCollection<Room> ();
			_activeChats = new ObservableCollection<Room> ();
            LoadData();
        }

        private async void LoadData()
        {
            IsBusy = true;
            await _appManager.ChatManager.ReloadUsers();
            Subject = _appManager.ChatManager.Subject;

            _appManager.ChatManager.OnlineUsers.SynchronizeWith(Users, u => new UserViewModel(u));
			_appManager.ChatManager.Rooms.SynchronizeWith(ActiveChats, r => r.Room, s => s.Room.RoomId, d => d.RoomId);
			//_appManager.ChatManager.Messages.SynchronizeWith(Events, i => _eventViewModelFactory.Get(i, _appManager.AccountManager.CurrentUser.UserId));
            IsBusy = false;

			PublicRooms.AddRange (PublicRoomsRepository.GetAll ());

			//Link our bool
			ActiveChats.CollectionChanged += (sender, e) => {
				Raise ("HasConversations");
			};
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

		public ObservableCollection<Room> ActiveChats
		{
			get { return _activeChats; }
			set { SetProperty(ref _activeChats, value); }
		}


        public string Subject
        {
            get { return _subject; }
            set { SetProperty(ref _subject, value); }
        }

		public bool HasConversations
		{
			get { return ActiveChats.Count > 0; }
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
			var roomData = await _appManager.ChatManager.JoinPublicRoom (room);
			var model = new ChatViewModel (_appManager, roomData);
			await model.ShowAsync ();
		}

		public ICommand SelectUserCommand
		{
			get { return new Command(OnSelectUser); }
		}

		private async void OnSelectUser(object userArg)
		{
			var user = userArg as UserViewModel;
			if (user == null)
			{
				throw new Exception ("Selected item was not a user detail view model");
			}

			//If the user is not us, go to their detail page
			if (user.UserId != _appManager.AccountManager.CurrentUser.UserId)
			{
				var fullMember = await _appManager.ChatManager.GetMemberDetails (user.UserId);
				var model = new UserDetailViewModel (_appManager, fullMember);
				await model.ShowAsync ();
			}
		}

		public ICommand ViewProfileCommand
		{
			get { return new Command(OnViewProfile); }
		}
		private async void OnViewProfile()
		{
			var profile = new Profile ();
			profile.Details = new ProfileDetails ();
			AutoMapper.CopyPropertyValues (_appManager.AccountManager.CurrentUser, profile);
			AutoMapper.CopyPropertyValues (_appManager.AccountManager.CurrentUser, profile.Details);

			var model = new UserDetailViewModel (_appManager, profile) {
				HasChatOption = false
			};
			await model.ShowAsync ();
		}

		public ICommand LogoutCommand
		{
			get { return new Command(OnLogout); }
		}

		public async void OnLogout()
		{
			_appManager.AccountManager.Logout ();
			await new LoginViewModel (_appManager).ShowModalAsync ();
		}


		#if DEBUG
		public string User
		{
			get{ return _appManager.AccountManager.CurrentUser.FirstName + " " + _appManager.AccountManager.CurrentUser.LastName; }
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
