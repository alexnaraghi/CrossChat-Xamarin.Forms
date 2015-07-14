﻿using SharedSquawk.Client.Seedwork;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client.ViewModels
{
    public class UserViewModel : ViewModelBase
    {
        private readonly Profile _profile;
        private string _description;
        private string _name;

        public UserViewModel(Profile profile)
        {
			_profile = profile;
			Name = profile.LastName + " " + profile.FirstName;
			Description = string.Format("{0}, {1} years old, Country: {2}", profile.Gender.ToString(), profile.Age, profile.LocaleID);
        }

        public string Description
        {
            get { return _description; }
            set { SetProperty(ref _description, value); }
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

		public int UserId
		{
			get { return _profile.UserId; }
		}
    }
}
