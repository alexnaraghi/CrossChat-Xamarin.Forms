using System;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Client.Model.Proxies;

namespace Crosschat.Client
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

	}
}

