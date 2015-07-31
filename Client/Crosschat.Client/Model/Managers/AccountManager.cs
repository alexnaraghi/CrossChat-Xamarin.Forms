using System.Threading.Tasks;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Proxies;
using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using System;
using Xamarin;

namespace SharedSquawk.Client.Model.Managers
{
    public class AccountManager : ManagerBase
    {
        private readonly ILoginServiceProxy _loginServiceProxy;
        private readonly IStorage _storage;
		private LoginResponse _currentUser = null;

		public event EventHandler LoggedIn = delegate { };
		public event EventHandler LoggedOut = delegate { };

        public AccountManager(IStorage storage,
            ConnectionManager connectionManager,
			RegistrationServiceProxy registrationServiceProxy,
            AuthenticationServiceProxy authenticationServiceProxy,
			ILoginServiceProxy loginServiceProxy)
            : base(connectionManager)
        {
			_loginServiceProxy = loginServiceProxy;
            _storage = storage;
        }

		public bool IsLoggedIn
		{
			get{return CurrentUser != null;}
		}

		public LoginResponse CurrentUser
        {
            get {
				return _currentUser;
			}
            private set
            {
                _currentUser = value;
            }
        }

		//ALEXTEST
        public string AccountUsername
        {
			get { return _storage.Get<string>(); }
            private set { _storage.Set(value); }
        }

		//ALEXTEST
        public string AccountPassword
        {
			get { return _storage.Get<string>(); }
            private set { _storage.Set(value); }
        }

		//This is the current session id of the login
		private string _ssid;
		public string SSID
		{
			get { return _ssid; }
			private set { _ssid = value; }
		}

		public async Task<bool> Login(string username, string password)
		{
			var result = await _loginServiceProxy.Login(
				new LoginRequest
				{
					Name = username,
					Password = password
				});

			if (result == null)
			{
				return false;
			}

			CurrentUser = result;
			AccountUsername = result.Username;
			AccountPassword = result.Password;
			SSID = result.SSID;

			//Log a user login
			Insights.Identify(result.Username, Insights.Traits.Name, result.FirstName + " " + result.LastName);

			//Do we need to look out for this member status result?
			MemberStatusResponse memberStatusResult;
			try
			{
				memberStatusResult = await _loginServiceProxy.GetMemberStatus(
					new MemberStatusRequest
					{
						SessionId = result.SSID,
						UserId = CurrentUser.UserId
					});
			}
			catch(AggregateException ex)
			{
				throw ex.Flatten ();
			}
			//Do something with the member status result.  Not sure what the US codes mean yet.

			LoggedIn(this, EventArgs.Empty);

			return result.RequestResult;
		}

		public void Logout()
		{
			this.CurrentUser = null;
			//We don't really have to do anything to tell the server we logged out, just throw the message up
			//to the application manager
			LoggedOut (this, EventArgs.Empty);
		}
    }
}
