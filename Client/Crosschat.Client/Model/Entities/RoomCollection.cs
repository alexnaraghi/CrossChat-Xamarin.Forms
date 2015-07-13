using System;
using System.Collections.Generic;
using SharedSquawk.Client.Model.Entities.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using SharedSquawk.Client.Seedwork.Extensions;

namespace SharedSquawk.Client
{
	public class RoomCollection : ObservableDictionary<string, ObservableCollection<Event>>
	{
		//
		public void AddMessage(string key, Event value)
		{
			if (!ContainsKey (key))
			{
				Add (key, new ObservableCollection<Event> ());
			}

			this [key].Add (value);
		}

		public void AddMessageRange(string key, IEnumerable<Event> values)
		{
			if (!ContainsKey (key))
			{
				Add (key, new ObservableCollection<Event> ());
			}

			this [key].AddRange (values);
		}

		public void RemoveMessage(string key, Event value)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant remove event");
			}

			this [key].Remove (value);
		}

		public Event Last(string key)
		{
			if (!ContainsKey (key))
			{
				throw new IndexOutOfRangeException ("list doesnt exit, cant remove event");
			}

			return this [key].Last();
		}
	}
}

