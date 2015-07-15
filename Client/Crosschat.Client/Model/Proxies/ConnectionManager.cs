using System;
using System.Threading;
using System.Threading.Tasks;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Server.Application.DataTransferObjects.Messages;
using SharedSquawk.Server.Application.DataTransferObjects.Requests;
using SharedSquawk.Server.Application.DataTransferObjects.Utils;
using SharedSquawk.Server.Infrastructure.Protocol;
using SharedSquawk.Utils.Tasking;

namespace SharedSquawk.Client.Model.Proxies
{
	public class ConnectionManager
    {
        private readonly SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        private readonly ITransportResource _transport;
        private readonly RequestsHandler _requestsHandler;
        private long _lastToken = 1;

        public ConnectionManager(
            ITransportResource transport,
            RequestsHandler requestsHandler)
        {
            _transport = transport;
            _requestsHandler = requestsHandler;

			_transport.DataReceived += DataReceived;
            _transport.ConnectionError += Transport_ConnectionError;

		}

		private void DataReceived(ResponseBase response)
		{
			_requestsHandler.AppendResponse (response);
		}

        private void Transport_ConnectionError()
        {
            ConnectionDropped();
        }

        public event Action ConnectionDropped = delegate { }; 

		internal Task<TResponse> SendRequestAndWaitResponse<TRequest, TResponse>(TRequest request)
			where TRequest : RequestBase
			where TResponse : ResponseBase
        {
            Interlocked.Increment(ref _lastToken);
            request.Token = _lastToken;
			var endpoint = EndpointFinder.Get (request.GetType ());

			var response = _requestsHandler.WaitForResponse<TResponse>(request, () => _transport.SendData<TRequest, TResponse>(endpoint, request, request.Token));

			if (!response.Result.RequestResult)
			{
				ConnectionDropped ();
			}
			return response;
        }

        public async Task DisconnectAsync()
        {
            await _semaphoreSlim.WaitAsync();
            await _transport.DisconnectAsync().WrapWithErrorIgnoring();
            _semaphoreSlim.Release();
        }
    }
}
