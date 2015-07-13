using Android.Content;
using Android.Telephony;
using SharedSquawk.Client.Droid.Infrastructure;
using SharedSquawk.Client.Model.Contracts;
using SharedSquawk.Client.Model.Entities;
using Xamarin.Forms;

[assembly: Dependency(typeof(InvitationSender))]

namespace SharedSquawk.Client.Droid.Infrastructure
{
    public class InvitationSender : IContactInvitationSender
    {
        public void Send(Contact contact)
        {
            //if (!string.IsNullOrEmpty(contact.Email))
            //{
            //    var email = new Intent(Intent.ActionSend);
            //    email.PutExtra(Intent.ExtraEmail, new[] { contact.Email });
            //    email.PutExtra(Intent.ExtraSubject, "Hey, join me in SharedSquawk!");
            //    email.PutExtra(Intent.ExtraText, "Check this out: https://github.com/EgorBo/SharedSquawk-Xamarin.Forms");
            //    email.SetType("message/rfc822");
            //    Forms.Context.StartActivity(email);
            //}
            SmsManager.Default.SendTextMessage(contact.Number, null, "Check this out: https://github.com/EgorBo/SharedSquawk-Xamarin.Forms", null, null);
        }
    }
}
