using System;

using Xamarin.Forms;
using System.Collections.Generic;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using SharedSquawk.Client.Model.Contracts;

namespace SharedSquawk.Client
{
	public class EndpointFinder
	{
		private static Dictionary<Type, TransportEndpoint> dict = new Dictionary<Type, TransportEndpoint>()
		{
			{typeof(LoginRequest), new TransportEndpoint(){TransportMethod = TransportMethod.POST, Address = "Members/Login.ashx"}},
			{typeof(MemberStatusRequest), new TransportEndpoint(){TransportMethod = TransportMethod.POST, Address = "Members/GetMemberStatus2.ashx"}},
			{typeof(ConnectedMembersRequest), new TransportEndpoint(){TransportMethod = TransportMethod.POST, Address = "Chat3/JoinTC.ashx"}},
			{typeof(ChatUpdateRequest), new TransportEndpoint(){TransportMethod = TransportMethod.POST, Address = "Chat3/GetTCUpdate.ashx"}},
			{typeof(ProfileRequest), new TransportEndpoint(){TransportMethod = TransportMethod.POST, Address = "Members/GetFullMember.aspx"}}
		};

		public static TransportEndpoint Get(Type type)
		{
			if (!dict.ContainsKey (type))
			{
				throw new NotSupportedException ("Transport endpoint could not be found");
			}
			return dict [type];
		}
	}
}


