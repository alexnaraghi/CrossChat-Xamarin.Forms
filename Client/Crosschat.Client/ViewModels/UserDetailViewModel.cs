using SharedSquawk.Client.Model.Entities;
using SharedSquawk.Client.Seedwork;
using SharedSquawk.Client.Model.Managers;
using SharedSquawk.Server.Application.DataTransferObjects;
using SharedSquawk.Client.Model.Helpers;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using System;

namespace SharedSquawk.Client.ViewModels
{
    public class UserDetailViewModel : ViewModelBase
    {
		private ApplicationManager _appManager;

		private string number;
		private int localeID;
		private int age;
		private string lastName;
		private string firstName;
		private string us;
		private string l;
		private string description;
		private int xcm;
		private int xcc;
		private string registrationDate;
		private int userId;
		private string knownLanguages;
		private string practicingLanguages;
		private Gender gender;

		public UserDetailViewModel(ApplicationManager appManager, Profile profile)
        {
			_appManager = appManager;

			//Auto map it like it's hot
			//Copy, flatten, you can make it happen
			AutoMapper.CopyPropertyValues (profile, this);
			AutoMapper.CopyPropertyValues (profile.Details, this);
        }

		public string Header
		{
			get { return string.Format("{0} {1} ({2} years old)", FirstName, LastName, Age); }
		}

		public string LocaleDetails
		{
			get { return string.Format ("{0}", L);}
		}

		public string PracticingLanguagesDisplay {
			get { return "Learns · " + PracticingLanguages; }
		}
		public string KnownLanguagesDisplay {
			get { return "Knows · " + KnownLanguages; }
		}

		public string UserPermissionsDisplay {
			get 
			{ 
				StringBuilder sb = new StringBuilder ();
				sb.Append("Exchange email messages? ").Append(xcm);
				sb.Append("   Chat enabled? ").Append(xcc);
				return sb.ToString ();
			}
		}

        public string Number
        {
            get { return number; }
            set { SetProperty(ref number, value); }
        }
		public string PracticingLanguages {
			get { return practicingLanguages; }
			set { SetProperty(ref practicingLanguages, value); }
		}
		public string KnownLanguages {
			get { return knownLanguages; }
			set { SetProperty(ref knownLanguages, value); }
		}
		public int LocaleID {
			get { return localeID; }
			set { SetProperty(ref localeID, value); }
		}
		public int Age {
			get { return age; }
			set { SetProperty(ref age, value); Raise ("Header");}
		}
		public Gender Gender
		{
			get{return gender;}
			set{ SetProperty (ref gender, value); }
		}
		public string LastName {
			get { return lastName; }
			set { SetProperty(ref lastName, value); Raise ("Header");}
		}
		public string FirstName {
			get { return firstName; }
			set { SetProperty(ref firstName, value); Raise ("Header"); }
		}
		public int UserId {
			get { return userId; }
			set { SetProperty(ref userId, value); }
		}
		public string US {
			get { return us; }
			set { SetProperty(ref us, value); }
		}
		public string L {
			get { return l; }
			set { SetProperty(ref l, value); Raise ("LocaleDetails");}
		}
		public string Description {
			get { return description; }
			set { SetProperty(ref description, value); }
		}
		public int XCM {
			get { return xcm; }
			set { SetProperty(ref xcm, value); }
		}
		public int XCC {
			get { return xcc; }
			set { SetProperty(ref xcc, value); }
		}
		public string RegistrationDate {
			get { return registrationDate; }
			set { SetProperty(ref registrationDate, value); }
		}

		public ICommand SelectChatCommand
		{
			get { return new Command(OnSelectChat); }
		}

		private async void OnSelectChat()
		{
			IsBusy = true;
			var roomData = await _appManager.ChatManager.JoinUserRoom (UserId);
			IsBusy = false;

			//Go to the root and open the chat window
			await this.PopToRootAsync ();
			var model = new ChatViewModel (_appManager, roomData);
			await model.ShowAsync ();
		}
    }
}
