using System;

using Xamarin.Forms;
using System.Collections.Generic;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using Crosschat.Client.Model.Contracts;
using Crosschat.Server.Application.DataTransferObjects.Requests;

namespace Crosschat.Client
{
	public class EndpointFinder
	{
		private static Dictionary<Type, TransportEndpoint> dict = new Dictionary<Type, TransportEndpoint>()
		{
			{typeof(LoginRequest), new TransportEndpoint(){TransportMethod = TransportMethod.POST, Address = "Members/Login.ashx"}},
			{typeof(ConnectedMembersRequest), new TransportEndpoint(){TransportMethod = TransportMethod.POST, Address = "Chat3/JoinTC.ashx"}}
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


