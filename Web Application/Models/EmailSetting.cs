
using MimeKit;
using Web_Application.ModelViews;

namespace Web_Application.Models
{
    public class EmailSetting
    {
        public bool SendEmail(EmailSetupVM vm, string AttachmentPath, string ToEmail, string subject, string body)
        {
            var client = new MailKit.Net.Smtp.SmtpClient();
            client.Connect(vm.SMTP, Convert.ToInt32(vm.PORT), true);
            // Note: since we don't have an OAuth2 token, disable the XOAUTH2 authentication mechanism.
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            // Note: only needed if the SMTP server requires authentication
            client.Authenticate(vm.EmailId, vm.EmailPws);
            var msg = new MimeMessage();
            msg.From.Add(new MailboxAddress("Crm", vm.EmailId));
            msg.To.Add(new MailboxAddress("yourclientdname",ToEmail));
            msg.Subject = subject;
            msg.Body = new TextPart("plain")
            {
                Text = body
            };
            client.Send(msg);
            client.Disconnect(true);
            return true;
        }
    }
}
