using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Crosschat.Client.Model;
using Crosschat.Client.Model.Managers;
using Crosschat.Client.Seedwork;
using Crosschat.Server.Application.DataTransferObjects.Enums;
using Xamarin.Forms;

namespace Crosschat.Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ApplicationManager _appManager;
        private string _name;
        private string _password;

		public LoginViewModel(ApplicationManager appManager)
        {
            _appManager = appManager;
        }

        public string Name
        {
            get { return _name; }
            set { SetProperty(ref _name, value); }
        }

        public string Password
        {
            get { return _password; }
            set { SetProperty(ref _password, value); }
        }

		public ICommand LoginCommand
        {
            get { return new Command(OnLogin); }
        }

		private async void OnLogin()
        {
            if (string.IsNullOrEmpty(Name) || 
                string.IsNullOrEmpty(Password))
            {
                await Notify("Invalid data", "Please, fill all the fields");
            }
            else
            {
                string platform = Device.OnPlatform("iOS", "Android", "WP8") + (Device.Idiom == TargetIdiom.Tablet ? " Tablet" : "");

                IsBusy = true;
				var registrationResult = await _appManager.AccountManager.Login(Name, Password);
                IsBusy = false;

                if (registrationResult == true)
                {
                    await new HomeViewModel(_appManager).ShowAsync();
                }
                else if (registrationResult == false)
                {
                    await Notify("Error", "Failed to login.");
                }
            }
        }
    }
}
