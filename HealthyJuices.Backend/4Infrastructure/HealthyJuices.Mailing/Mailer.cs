using System.Threading.Tasks;
using HealthyJuices.Domain.Providers;
using MailKit.Net.Smtp;
using MimeKit;

namespace HealthyJuices.Mailing
{
    public class Mailer : IMailer
    {
        protected readonly string SmtpServer;
        protected readonly string SmtpUser;
        protected readonly string SmtpPassword;
        protected readonly string SmtpMailFrom;
        protected readonly int Port;

        public Mailer(string smtpServer, string smtpUser, string smtpPassword, string smtpMailFrom, int port)
        {
            SmtpServer = smtpServer;
            SmtpUser = smtpUser;
            SmtpPassword = smtpPassword;
            SmtpMailFrom = smtpMailFrom;
            Port = port;
        }

        public async Task SendAsync(string recipient, string subject, string body, bool isBodyHtml = false, params string[] ccs)
        {
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect(SmtpServer, Port, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate(SmtpUser, SmtpPassword);

                var message = new MimeMessage();

                foreach (var cc in ccs)
                {
                    message.Cc.Add(new MailboxAddress("", cc));
                }

                message.From.Add(new MailboxAddress("Fenix", SmtpMailFrom));
                message.To.Add(new MailboxAddress("", recipient));
                message.Subject = subject;

                if (isBodyHtml)
                {
                    var builder = new BodyBuilder { HtmlBody = body };

                    message.Body = builder.ToMessageBody();
                }
                else
                {
                    message.Body = new TextPart("plain") { Text = body };
                }

                await client.SendAsync(message);
            }
        }
    }
}
