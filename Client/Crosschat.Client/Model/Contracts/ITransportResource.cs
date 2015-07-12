using System;
using System.Threading.Tasks;
using Crosschat.Server.Application.DataTransferObjects.Requests;

namespace Crosschat.Client.Model.Contracts
{
    public interface ITransportResource
    {
        Task ConnectAsync();

        Task DisconnectAsync();

        event Action ConnectionError;

		event Action<ResponseBase> DataReceived;

		void SendData<TRequest, TResponse>(TransportEndpoint endpoint, TRequest data, long token) 
			where TRequest : RequestBase
			where TResponse : ResponseBase;
    }

	public enum TransportMethod
	{
		POST,
		GET
	}

	public struct TransportEndpoint
	{
		public TransportMethod TransportMethod;
		public string Address;
	}
}