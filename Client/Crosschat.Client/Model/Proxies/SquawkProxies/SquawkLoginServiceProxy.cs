using System;
using System.Threading.Tasks;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using SharedSquawk.Client.Model.Proxies;

namespace SharedSquawk.Client
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

		public Task<MemberStatusResponse> GetMemberStatus (MemberStatusRequest request)
		{
			return ConnectionManager.SendRequestAndWaitResponse<MemberStatusRequest,MemberStatusResponse>(request);
		}

		#endregion

	}
}

