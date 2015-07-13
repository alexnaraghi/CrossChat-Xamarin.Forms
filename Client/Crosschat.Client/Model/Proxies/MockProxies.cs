using System;
using SharedSquawk.Client.Model.Proxies;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using System.Threading.Tasks;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Enums;

namespace SharedSquawk.Client
{
	public class FakeServiceProxyBase : ServiceProxyBase
	{
		public FakeServiceProxyBase(ConnectionManager connectionManager) : base(connectionManager)
		{
		}

		protected Task<T> GetFakeTask<T>(T response) where T: ResponseBase
		{
			var task = new Task<T> (() => {
				return response;
			});
			task.Start ();
			return task;
		}
	}

	public class FakeChatServiceProxy : FakeServiceProxyBase, IChatServiceProxy
	{
		public Task<ChatUpdateResponse> GetChatUpdate (ChatUpdateRequest request)
		{
			throw new NotImplementedException ();
		}

		public Task<ConnectedMembersResponse> GetConnectedMembers (ConnectedMembersRequest request)
		{
			throw new NotImplementedException ();
		}

		public FakeChatServiceProxy(ConnectionManager connectionManager) : base(connectionManager)
		{
		}


		public void PublicMessage(PublicMessageRequest request)
		{
			//Do nothing
		}

		public Task<SendImageResponse> SendImage(SendImageRequest request)
		{
			//Do nothing
			return null;
		}

		public Task<GrantModershipResponse> GrantModership(GrantModershipRequest request)
		{
			//Do nothing
			return null;
		}

		public Task<RemoveModershipResponse> RemoveModership(RemoveModershipRequest request)
		{
			//Do nothing
			return null;
		}

		public Task<DevoiceResponse> Devoice(DevoiceRequest request)
		{
			//Do nothing
			return null;
		}

		public Task<BanResponse> Ban(BanRequest request)
		{
			//Do nothing
			return null;
		}

		public Task<ResetPhotoResponse> ResetPhoto(ResetPhotoRequest request)
		{
			//Do nothing
			return null;
		}

		public Task<LastMessageResponse> GetLastMessages(LastMessageRequest request)
		{
			var rand = new Random ();
			var response = request.CreateResponse<LastMessageResponse> ();
			response.Subject = "YEA BOY";
			response.Messages = new PublicMessageDto[] {
				new PublicMessageDto {
					Timestamp = DateTime.Now - TimeSpan.FromMinutes (rand.Next() % 60),
					Role = UserRoleEnum.User,
					AuthorName = "alex",
					Body = "WHAT ??",
					ImageId = null
				},
				new PublicMessageDto {
					Timestamp = DateTime.Now - TimeSpan.FromMinutes (rand.Next() % 60),
					Role = UserRoleEnum.User,
					AuthorName = "justin",
					Body = "YEA ??",
					ImageId = null
				},
				new PublicMessageDto {
					Timestamp = DateTime.Now - TimeSpan.FromMinutes (rand.Next() % 60),
					Role = UserRoleEnum.User,
					AuthorName = "bennett",
					Body = "WHO ??",
					ImageId = null
				},
			};
			return GetFakeTask(response);
		}


	}
	/*
	public class FakeLoginServiceProxy : FakeServiceProxyBase, ILoginServiceProxy
	{
		public FakeLoginServiceProxy(ConnectionManager connectionManager) : base(connectionManager)
		{
			
		}

		public Task<MemberStatusResponse> GetMemberStatus (MemberStatusRequest request)
		{
		}
		public Task<LoginResponse> Login (LoginRequest request)
		{
			var response = request.CreateResponse<LoginResponse> ();
			{
				response.SSID = 1234;
				response.FirstName = "Alex";
				response.LastName = "Fish";
			}

			return GetFakeTask(response);
		}
	}*/
}

