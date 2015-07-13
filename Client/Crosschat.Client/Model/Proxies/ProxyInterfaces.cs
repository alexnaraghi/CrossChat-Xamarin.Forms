using System;
using Xamarin.Forms;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using System.Threading.Tasks;

namespace Crosschat.Client
{
	public interface IChatServiceProxy
	{
		//void PublicMessage (PublicMessageRequest request);

		//Task<SendImageResponse> SendImage (SendImageRequest request);

		//Task<GrantModershipResponse> GrantModership (GrantModershipRequest request);

		//Task<RemoveModershipResponse> RemoveModership (RemoveModershipRequest request);

		//Task<DevoiceResponse> Devoice (DevoiceRequest request);

		//Task<BanResponse> Ban (BanRequest request);

		//Task<ResetPhotoResponse> ResetPhoto (ResetPhotoRequest request);

		//Task<LastMessageResponse> GetLastMessages (LastMessageRequest request);

		Task<ConnectedMembersResponse> GetConnectedMembers (ConnectedMembersRequest request);

		Task<ChatUpdateResponse> GetChatUpdate (ChatUpdateRequest request);
	}

	public interface ILoginServiceProxy
	{
		Task<LoginResponse> Login (LoginRequest request);
		Task<MemberStatusResponse> GetMemberStatus (MemberStatusRequest request);
	}
}


