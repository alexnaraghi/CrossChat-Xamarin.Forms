using System;
using Crosschat.Client.Model.Proxies;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Messages;
using Crosschat.Server.Application.DataTransferObjects.Enums;

namespace Crosschat.Client
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

		public Task<GetOnlineUsersResponse> GetOnlineUsers(GetOnlineUsersRequest request)
		{
			var rand = new Random ();
			var response = request.CreateResponse<GetOnlineUsersResponse> ();
			response.Users = new UserDto[] {
				new UserDto {
					Id = 1,
					Name = "alex",
					Sex = true,
					Age = 12,
					Country = "USA",
					Role = UserRoleEnum.User
				},
				new UserDto {
					Id = 2,
					Name = "bennett",
					Sex = true,
					Age = 12,
					Country = "USA",
					Role = UserRoleEnum.User
				},
				new UserDto {
					Id = 3,
					Name = "justin",
					Sex = true,
					Age = 12,
					Country = "USA",
					Role = UserRoleEnum.User
				}
			};

			return GetFakeTask(response);
		}

	}

	public class FakeLoginServiceProxy : FakeServiceProxyBase, ILoginServiceProxy
	{
		public FakeLoginServiceProxy(ConnectionManager connectionManager) : base(connectionManager)
		{
		}

		public Task<LoginResponse> Login (LoginRequest request)
		{
			var response = request.CreateResponse<LoginResponse> ();
			response.U = new U {
				SSID = 1234,
				FN = "Alex",
				LN = "Fish"
			};

			return GetFakeTask(response);
		}
	}
}

