using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using SharedSquawk.Client.Model;
using SharedSquawk.Client.Model.Managers;
using SharedSquawk.Client.Seedwork;
using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using Xamarin.Forms;
using System;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;

namespace SharedSquawk.Client.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ApplicationManager _appManager;
        private string _name;
        private string _password;

		public LoginViewModel(ApplicationManager appManager)
        {
            _appManager = appManager;
			Name = appManager.AccountManager.AccountUsername;
			Password = appManager.AccountManager.AccountPassword;
			_appManager.ConnectionManager.ConnectionDropped += OnConnectionLost;
        }

		//The login page defines what will occur on the connection dropping
		public async void OnConnectionLost()
		{
			if (CurrentViewModel == this)
			{
				_appManager.AccountManager.Logout ();
				Notify ("Connection Error", "An error has occurred when trying to login.");
			}
			else
			{
				await Logout ();
			}
		}

		public async Task Logout()
		{
			await Notify ("Connection Error", "An error has occurred.  You will need to login again.");
			_appManager.AccountManager.Logout ();
			await new LoginViewModel (_appManager).ShowModalAsync ();
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
                await Notify("Invalid data", "Please, fill all the fields.");
            }
            else
            {
                IsBusy = true;
				var	registrationResult = await _appManager.AccountManager.Login(Name, Password);
                IsBusy = false;

                if (registrationResult == true)
                {
					await new HomeViewModel(_appManager).ShowModalAsyncWithNavPage();
                }
                else if (registrationResult == false)
                {
                    await Notify("Error", "Failed to login.");
                }
            }
        }

		public ICommand RegisterCommand
		{
			get { return new Command(OnRegister); }
		}

		private async void OnRegister()
		{
			await Notify ("Registration", "Please visit www.SharedTalk.com to create an account to use " +
			"with this app, or to retrieve a lost password.  This app cannot be used to register new " +
			"accounts for the website at this time.");
		}
    }
}
