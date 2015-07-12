using System;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Client.Model.Proxies;

namespace Crosschat.Client
{
	public class SquawkLoginServiceProxy : ServiceProxyBase, ILoginServiceProxy
	{
		public SquawkLoginServiceProxy (ConnectionManager connectionManager) : base(connectionManager)
		{
		}

		#region ILoginServiceProxy implementation

		public Task<LoginResponse> Login (LoginRequest request)
		{
			return ConnectionManager.SendRequestAndWaitResponse<LoginRequest,LoginResponse>(request);
		}

		#endregion

	}
}

