using System;
using System.Collections.Generic;
using SharedSquawk.Client.Model.Entities.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using SharedSquawk.Client.Seedwork.Extensions;

namespace SharedSquawk.Client
{
	public class RoomData
	{
		public ObservableCollection<Event> TextMessages{get; private set;}
		public ObservableCollection<Event> TypingEvents{ get; private set; }

		public RoomData()
		{
			TextMessages = new ObservableCollection<Event> ();
			TypingEvents = new ObservableCollection<Event> ();
		}
	}

	public class RoomCollection : ObservableDictionary<string, RoomData>
	{
		//
		public void AddTextMessage(string key, Event value)
		{
			if (!ContainsKey (key))
			{
				Add (key, new RoomData());
			}

			this [key].TextMessages.Add (value);
		}

		public void AddTextMessageRange(string key, IEnumerable<Event> values)
		{
			if (!ContainsKey (key))
			{
				Add (key, new RoomData());
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
				Add (key, new RoomData());
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
	}
}

