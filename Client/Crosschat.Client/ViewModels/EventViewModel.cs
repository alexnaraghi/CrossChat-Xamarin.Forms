using SharedSquawk.Client.Model.Entities.Messages;
using SharedSquawk.Client.Seedwork;

namespace SharedSquawk.Client.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        private readonly Event _eventPoco;

        public EventViewModel(Event eventPoco)
        {
            _eventPoco = eventPoco;
        }
    }
}
