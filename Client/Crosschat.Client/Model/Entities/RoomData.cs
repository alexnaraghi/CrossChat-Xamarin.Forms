using System;
using System.Collections.Generic;
using SharedSquawk.Client.Model.Entities.Messages;
using System.Collections.ObjectModel;
using System.Linq;
using SharedSquawk.Client.Seedwork.Extensions;
using System.ComponentModel;
using SharedSquawk.Client.Properties;
using System.Runtime.CompilerServices;
using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client.Model.Entities
{
	public class RoomData : INotifyPropertyChanged
	{
		private RoomStatus _status;
		private Room _room;
		private ObservableCollection<Event> _textMessages;
		private ObservableCollection<Event> _typingEvents;

		public RoomStatus Status { 
			get { return _status; }
			set
			{
				SetProperty (ref _status, value); 
			}
		}
		public ObservableCollection<Event> TextMessages{ 
			get { return _textMessages; }
			set{ SetProperty (ref _textMessages, value); }
		}
		public ObservableCollection<Event> TypingEvents{ 
			get { return _typingEvents; }
			set{ SetProperty (ref _typingEvents, value); }
		}

		public Room Room
		{ 
			get { return _room; }
			set{ SetProperty (ref _room, value); }
		}

		//I believe a room data has a dependency where it cannot be created without a backing
		//room object
		public RoomData(Room room)
		{
			TextMessages = new ObservableCollection<Event> ();
			TypingEvents = new ObservableCollection<Event> ();
			Room = room;
			Status = RoomStatus.Waiting;
		}

		#region INotifyPropertyChanged Interface
		[NotifyPropertyChangedInvocator]
		protected virtual RoomData SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			field = value;
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
			return this;
		}

		[NotifyPropertyChangedInvocator]
		protected virtual RoomData Raise(string propertyName)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
			return this;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		#endregion
	}
		
	public enum RoomStatus
	{
		Waiting,
		Connected,
		OtherUserLeft,
		OtherUserDeclined,
		AwaitingOurApproval,
		Error
	}
}

