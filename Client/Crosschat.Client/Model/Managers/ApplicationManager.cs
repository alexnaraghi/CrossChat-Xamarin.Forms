#define ALEXMOCK

using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Proxies;
using SharedSquawk.Server.Application.DataTransferObjects.Utils;
using SharedSquawk.Server.Infrastructure.Protocol;
using System;

namespace SharedSquawk.Client.Model.Managers
{
    /// <summary>
    /// Managers locator
    /// </summary>
    public class ApplicationManager
    {
		private ITransportResource _transport;
		private IStorage _storage;

        public ApplicationManager(
            ITransportResource transportResource,
            IStorage storage)
        {
			_transport = transportResource;
			_storage = storage;

			Initialize ();
        }

		private void Initialize()
		{
			//we don't have autofac so let's build the tree
			var connectionManager = new ConnectionManager(_transport, new RequestsHandler());
			var loginServiceProxy = new SquawkLoginServiceProxy(connectionManager);
			AccountManager = new AccountManager(_storage, connectionManager,
				new RegistrationServiceProxy(connectionManager), 
				new AuthenticationServiceProxy(connectionManager),
				loginServiceProxy);
			AccountManager.LoggedOut += LoggedOut;
			
			var chatServiceProxy = new SquawkChatServiceProxy(connectionManager);
			ChatManager = new ChatManager(connectionManager, chatServiceProxy, AccountManager);
			ConnectionManager = connectionManager;
		}

        public ConnectionManager ConnectionManager { get; private set; }

        public AccountManager AccountManager { get; private set; }

        public ChatManager ChatManager { get; private set; }

		public void LoggedOut(object sender, EventArgs args)
		{
			//Easiest way, just clear out all the stateful things and reinstantiate them.
			//We can get more granular if we need later.
			Initialize();
		}
    }
}
