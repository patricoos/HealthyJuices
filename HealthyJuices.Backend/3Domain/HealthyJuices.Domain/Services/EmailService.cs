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

        public async Task<bool> SendRegisterCodeEmail(string email, string url, string token)
        {
            return await _mailer.Send(email, "Register Veryfication", $"Confirm: '{url}?token={token}&email={email}'");
        }

        public async Task<bool> SendForgotPasswordEmail(string email, string token)
        {
            return await _mailer.Send(email, "Veryfication Code", $"Code: '{token}'");
        }
    }
}