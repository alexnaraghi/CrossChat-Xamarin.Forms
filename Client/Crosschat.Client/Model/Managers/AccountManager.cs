using System.Threading.Tasks;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Proxies;
using SharedSquawk.Server.Application.DataTransferObjects.Enums;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using System;

namespace SharedSquawk.Client.Model.Managers
{
    public class AccountManager : ManagerBase
    {
        private readonly IDeviceInfo _deviceInfo;
        private readonly ProfileServiceProxy _profileServiceProxy;
        private readonly AuthenticationServiceProxy _authenticationServiceProxy;
        private readonly RegistrationServiceProxy _registrationServiceProxy;
		private readonly ILoginServiceProxy _loginServiceProxy;
        private readonly IStorage _storage;
		private LoginResponse _currentUser = null;
        private bool _deviceInfoInitialized = false;

		public event EventHandler LoggedIn = delegate { };
		public event EventHandler LoggedOut = delegate { };

        public AccountManager(IStorage storage,
            IDeviceInfo deviceInfo,
            ConnectionManager connectionManager,
            ProfileServiceProxy profileServiceProxy,
			RegistrationServiceProxy registrationServiceProxy,
            AuthenticationServiceProxy authenticationServiceProxy,
			ILoginServiceProxy loginServiceProxy)
            : base(connectionManager)
        {
            _deviceInfo = deviceInfo;
            _profileServiceProxy = profileServiceProxy;
            _authenticationServiceProxy = authenticationServiceProxy;
            _registrationServiceProxy = registrationServiceProxy;
			_loginServiceProxy = loginServiceProxy;
            _storage = storage;
        }

		public bool IsLoggedIn
		{
			get{return CurrentUser != null;}
		}

		//ALEXTEST
        //yeah, I didn't create an entity representing User and let managers expose DTO instead. I'm lazy :(
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

		/*
		//ALEXTEST
        public bool IsRegistered
        {
			get { return true; return _storage.Get<bool>(); }
            private set { _storage.Set(value); }
        }
		*/

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
		private long _ssid;
		public long SSID
		{
			get { return _ssid; }
			private set { _ssid = value; }
		}

        public async Task<AuthenticationResponseType> ValidateAccount(string name, string password)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(password))
                return AuthenticationResponseType.InvalidNameOrPassword;

            await InitDeviceInfo();
            await ConnectionManager.ConnectAsync();
            var authResult = await _authenticationServiceProxy.Authenticate(
                new AuthenticationRequest { Huid = _deviceInfo.Huid, Name = name, Password = password });

            if (authResult.Result == AuthenticationResponseType.Success)
            {
                AccountUsername = name;
                AccountPassword = password;
                //CurrentUser = authResult.User;
                //IsRegistered = true;
            }

            return authResult.Result;
        }

        public async Task<AuthenticationResponseType> ValidateAccount()
        {
            //if (!IsRegistered)
                return AuthenticationResponseType.InvalidNameOrPassword;
            return await ValidateAccount(AccountUsername, AccountPassword);
        }

		public async Task<bool> Login(string username, string password)
		{
			//ALEXTEST
			//await InitDeviceInfo();
			//await ConnectionManager.ConnectAsync();
			var result = await _loginServiceProxy.Login(
				new LoginRequest
				{
					Name = username,
					Password = password
				});

			if (result.RequestResult == true)
			{
				//ALEXTEST
				//CurrentUser = result.Name;
				CurrentUser = result;
				AccountUsername = username;
				AccountPassword = password;
				SSID = result.SSID;


				//Do we need to look out for this member status result?
				var memberStatusResult = await _loginServiceProxy.GetMemberStatus(
					new MemberStatusRequest
					{
						SessionId = result.SSID,
						UserId = CurrentUser.UserId
					});
				//Do something with the member status result.  Not sure what the US codes mean yet.

				LoggedIn(this, EventArgs.Empty);

			}

			return result.RequestResult;
		}

		public void Logout()
		{
			//We don't really have to do anything to tell the server we logged out, just clear our data
			CurrentUser = null;
			LoggedOut (this, EventArgs.Empty);
		}

        public async Task<RegistrationResponseType> Register(string name, string password, int age, bool sex, string country, string platform)
        {
            await InitDeviceInfo();
			//ALEXTEST
            //await ConnectionManager.ConnectAsync();
            var result = await _registrationServiceProxy.RegisterNewUser(
                new RegistrationRequest
                {
                    Age = age,
                    Name = name,
                    Password = password,
                    Platform = platform,
                    Sex = sex,
                    Country = country,
                    Huid = _deviceInfo.Huid,
                    PushUri = _deviceInfo.PushUri,
                });

            if (result.Result == RegistrationResponseType.Success)
            {
                //IsRegistered = true;
                //CurrentUser = result.User;
                AccountUsername = name;
                AccountPassword = password;
            }

            return result.Result;
        }

        public async Task<bool> ChangePhoto(byte[] photoData)
        {
            var response = await _profileServiceProxy.ChangePhoto(new ChangePhotoRequest { PhotoData = photoData });
            if (response.Success)
            {
                //CurrentUser.PhotoId = response.PhotoId;
            }
            return response.Success;
        }

        public async Task Deactivate()
        {
            var response = await _registrationServiceProxy.Deactivate(new DeactivationRequest());
            //IsRegistered = false;
            AccountUsername = "";
            AccountPassword = "";
            CurrentUser = null;
        }

        private async Task InitDeviceInfo()
        {
            if (_deviceInfoInitialized)
                return;
            await _deviceInfo.InitAsync();
            _deviceInfoInitialized = true;
        }
    }
}
