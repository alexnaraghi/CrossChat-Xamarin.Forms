using SharedSquawk.Client.iOS.Infrastructure;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Entities;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(InvitationSender))]

namespace SharedSquawk.Client.iOS.Infrastructure
{
    public class InvitationSender : IContactInvitationSender
    {
        public void Send(Contact contact)
        {
            var smsTo = NSUrl.FromString("sms:" + contact.Number);
            UIApplication.SharedApplication.OpenUrl(smsTo);
        }
    }
}
