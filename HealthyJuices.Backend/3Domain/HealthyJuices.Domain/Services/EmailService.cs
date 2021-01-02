using System.Text;
using System.Threading.Tasks;

namespace HealthyJuices.Domain.Services
{
    public class EmailService
    {
        private readonly IMailer _mailer;

        public EmailService(IMailer mailer)
        {
            _mailer = mailer;
        }

        public async Task SendRegisterCodeEmail(string email, string url, string token)
        {
             await _mailer.SendAsync(email, "Register Veryfication", $"Confirm: '{url}?token={token}&email={email}'");
        }

        public async Task SendForgotPasswordEmail(string email, string token)
        {
            await _mailer.SendAsync(email, "Veryfication Code", $"Code: '{token}'");
        }
    }
}