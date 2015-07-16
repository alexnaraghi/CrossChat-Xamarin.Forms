using SharedSquawk.Client.Seedwork;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using SharedSquawk.Client.Model.Entities;
using SharedSquawk.Client.Model;

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
			Name = profile.FirstName + " " + profile.LastName;
			Description = string.Format("{0}, {1} years old, {2}", 
				profile.Gender.ToString(), profile.Age, CountriesRepository.Get(profile.LocaleID));
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
