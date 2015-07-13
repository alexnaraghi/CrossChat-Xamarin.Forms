using SharedSquawk.Client.Model.Entities;

namespace SharedSquawk.Client.Model.Contracts
{
    public interface IContactInvitationSender
    {
        void Send(Contact contact);
    }
}
