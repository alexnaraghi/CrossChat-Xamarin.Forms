using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Entities;
using SharedSquawk.Client.WinPhone.Infrastructure;
using Microsoft.Phone.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(InvitationSender))]

namespace SharedSquawk.Client.WinPhone.Infrastructure
{
    public class InvitationSender : IContactInvitationSender
    {
        public void Send(Contact contact)
        {
            if (!string.IsNullOrEmpty(contact.Email))
            {
                var emailComposeTask = new EmailComposeTask();

                emailComposeTask.Subject = "Hey, join me in SharedSquawk!";
                emailComposeTask.Body = "Check this out: https://github.com/EgorBo/SharedSquawk-Xamarin.Forms";
                emailComposeTask.To = contact.Email;

                emailComposeTask.Show();
            }
            else
            {
                var emailComposeTask = new SmsComposeTask();

                emailComposeTask.Body = "Check this out: https://github.com/EgorBo/SharedSquawk-Xamarin.Forms";
                emailComposeTask.To = contact.Number;

                emailComposeTask.Show();
            }
        }
    }
}
