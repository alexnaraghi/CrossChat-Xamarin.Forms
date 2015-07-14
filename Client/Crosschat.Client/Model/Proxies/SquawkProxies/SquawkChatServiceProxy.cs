using System;
using System.Threading.Tasks;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using SharedSquawk.Client.Model.Proxies;

namespace SharedSquawk.Client
{
	public class SquawkChatServiceProxy : ServiceProxyBase, IChatServiceProxy
	{
		public SquawkChatServiceProxy (ConnectionManager connectionManager) : base(connectionManager)
		{
		}

		public Task<ConnectedMembersResponse> GetConnectedMembers(ConnectedMembersRequest request)
		{
			return ConnectionManager.SendRequestAndWaitResponse<ConnectedMembersRequest,ConnectedMembersResponse>(request);
		}

		public Task<ChatUpdateResponse> GetChatUpdate(ChatUpdateRequest request)
		{
			return ConnectionManager.SendRequestAndWaitResponse<ChatUpdateRequest,ChatUpdateResponse>(request);
		}

		public Task<ProfileResponse> GetUserProfile (ProfileRequest request)
		{
			return ConnectionManager.SendRequestAndWaitResponse<ProfileRequest,ProfileResponse>(request);
		}

	}
}

