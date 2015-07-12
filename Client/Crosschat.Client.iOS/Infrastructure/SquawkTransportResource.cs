using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Crosschat.Client.iOS.Infrastructure;
using Crosschat.Client.Model.Contracts;
using Crosschat.Server.Infrastructure;
using Xamarin.Forms;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using Crosschat.Server.Application.DataTransferObjects.Requests;
using System.Xml;

[assembly: Dependency(typeof(SquawkTransportResource))]

namespace Crosschat.Client.iOS.Infrastructure
{
    public class 
		SquawkTransportResource: ITransportResource
    {
        private bool _isConnected;
        private const int BufferSize = 8 * 1024;
        private bool _triedConnect = false;

        public event Action ConnectionError = delegate { };
		public event Action<ResponseBase> DataReceived = delegate { };
        public event Action ConnectionStateChanged = delegate { };
        
        public async Task ConnectAsync()
        {
            IsConnected = true;
        }

		public bool IsConnected
		{
			//ALEXTEST
			get { return true;
				//return _isConnected; 
			}
			private set
			{
				if (_isConnected != value)
				{
					ConnectionStateChanged();
				}
				_isConnected = value;
			}
		}

        public async void DropConnection()
        {
            ConnectionError();
        }

        private void StartListening()
        {
        }

        public Task DisconnectAsync()
        {
            return Task.FromResult(false);
        }

		public void SendData<TRequest, TResponse>(TransportEndpoint endpoint, TRequest data, long token) 
			where TRequest : RequestBase 
			where TResponse : ResponseBase
		{
			var request = (HttpWebRequest)WebRequest.Create(GlobalConfig.ServiceBase+endpoint.Address);

			request.Method = endpoint.TransportMethod.ToString();

			request.ContentType = "application/x-www-form-urlencoded";
			request.UserAgent = GlobalConfig.UserAgent;
			request.Referer = GlobalConfig.Referer;
			request.Headers.Add ("X-Requested-With", GlobalConfig.RequestedWith);
			request.Headers.Add (HttpRequestHeader.AcceptEncoding, GlobalConfig.AcceptEncoding);
			//ALEXTEST
			//TODO: Set the language
			request.Headers.Add (HttpRequestHeader.AcceptLanguage, "en-US,en;q=0.8");
			request.Timeout = GlobalConfig.TimeoutMs;

			Stream stream = request.GetRequestStream();
			Serialize<TRequest> (data, stream);
			stream.Close();

			try
			{
				request.BeginGetResponse(new AsyncCallback(OnDataReceived<TResponse>), new TokenRequest {Request = request, Token = token});
			}
			catch (Exception exc)
			{
				DropConnection();
			}
		}
		private void OnDataReceived<T>(IAsyncResult ar) where T : ResponseBase
		{
			HttpWebRequest request;
			HttpWebResponse response;
			long token;
			ResponseBase responseObject;
			try
			{
				var tokenRequest = ar.AsyncState as TokenRequest;
				request = tokenRequest.Request;
				token = tokenRequest.Token;
				response = request.EndGetResponse (ar) as HttpWebResponse;
				responseObject = Deserialize<T> (response.GetResponseStream());
				response.Close ();
			}
			catch(Exception ex)
			{
				DropConnection();
				return;
			}

			//Here we are downcasting T to an object, but the token will allow us to figure out the type
			responseObject.Token = token;
			DataReceived(responseObject);
		}

		private void Serialize<T>(T data, Stream stream)
		{
			XmlSerializer serializer = new XmlSerializer (typeof(T));

			//We have to strip out all the namespaces and xml declaration stuff because that is how the server accepts it.
			//Serialize into a raw element.
			XmlSerializerNamespaces ns = new XmlSerializerNamespaces ();
			ns.Add ("", "");
			using (XmlWriter writer = XmlWriter.Create (stream, new XmlWriterSettings { OmitXmlDeclaration = true }))
			{
				serializer.Serialize (writer, data, ns);
			}

			#if DEBUG
			//Debug code to capture what the xml looked like on serialization
			using (StringWriter textWriter = new StringWriter ())
			{
				using (XmlWriter writer = XmlWriter.Create (textWriter, new XmlWriterSettings { OmitXmlDeclaration = true }))
				{
					serializer.Serialize (writer, data, ns);
					var dataAsString = textWriter.ToString ();
					Console.WriteLine (string.Format ("serializing {0}: {1}", typeof(T).Name, dataAsString));
				}

			}
			#endif

		}


		private T Deserialize<T>(Stream stream)
		{
			//Reading the stream into a string is easier for debugging problems, think i'll just leave it in here.
			string dataAsString;
			using (StreamReader reader = new StreamReader (stream))
			{
				dataAsString = reader.ReadToEnd ();
			}
			#if DEBUG
			//Debug code to capture what the xml looked like on deserialization
			Console.WriteLine (string.Format ("deserializing {0}: {1}", typeof(T).Name, dataAsString));
			#endif

			T deserializedObject;
			XmlSerializer serializer = new XmlSerializer (typeof(T));

			//We have to read the xml as an element since we don't get a full xml file
			using(XmlTextReader reader = new XmlTextReader(dataAsString, XmlNodeType.Element, null))
			{
				deserializedObject = (T)serializer.Deserialize (reader);
			}
			return deserializedObject;
		}
    }




	public class TokenRequest
	{
		public HttpWebRequest Request{ get; set;}
		public long Token{ get; set;}
	}
}
