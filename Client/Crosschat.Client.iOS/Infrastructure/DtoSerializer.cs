using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System;
using SharedSquawk.Server.Infrastructure.Protocol;
using Xamarin.Forms;
using SharedSquawk.Client.iOS.Infrastructure;

[assembly: Dependency(typeof(DtoSerializer))]

namespace SharedSquawk.Client.iOS.Infrastructure
{
    public class DtoSerializer : IDtoSerializer
    {
		public void Serialize<T>(T data, Stream stream)
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


		public T Deserialize<T>(Stream stream)
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
				try
				{
					deserializedObject = (T)serializer.Deserialize (reader);
				}
				catch(Exception exc)
				{
					throw new DeserializeException ("Deserialize failed, see inner exception");
				}
			}
			return deserializedObject;
		}

	}

	public class DeserializeException : Exception
	{
		public DeserializeException(string message):base(message)
		{

		}
	}

}
