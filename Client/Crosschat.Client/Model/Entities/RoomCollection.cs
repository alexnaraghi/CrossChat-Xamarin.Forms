using System;
using System.Collections.Generic;
using SharedSquawk.Client.Model.Entities.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using SharedSquawk.Client.Seedwork.Extensions;
using System.ComponentModel;
using SharedSquawk.Client.Properties;
using System.Runtime.CompilerServices;

namespace SharedSquawk.Client.Model.Entities
{
	public class RoomCollection : ObservableDictionary<string, RoomData>
	{
		public void AddTextMessage(string key, Event value)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant add event");
			}

			this [key].TextMessages.Add (value);
		}

		public void AddTextMessageRange(string key, IEnumerable<Event> values)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant add event");
			}
			this [key].TextMessages.AddRange (values);
		}

		public void RemoveTextMessage(string key, Event value)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant remove event");
			}

			this [key].TextMessages.Remove (value);
		}

		public Event LastTextMessage(string key)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant remove event");
			}

			return this [key].TextMessages.Last();
		}

		public void AddTypingEvent(string key, Event value)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant add event");
			}

			this [key].TypingEvents.Add (value);
		}

		public void ClearAllTypingEvents()
		{
			foreach (var roomCollection in this.Values)
			{
				roomCollection.TypingEvents.Clear ();
			}
		}

		public void SetStatus(string key, RoomStatus status)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant set status");
			}
			this [key].Status = status;
		}

		public void CreateRoom(Room room)
		{
			//A redundant key here, but that actually is useful for synchronizing observable collections
			//(the synchronize command doesn't have access to the key in this app's implementation).
			this.Add (room.RoomId, new RoomData(room));
		}

		public void RemoveRoom(string key)
		{
			this.Remove (key);
		}
	}
}

