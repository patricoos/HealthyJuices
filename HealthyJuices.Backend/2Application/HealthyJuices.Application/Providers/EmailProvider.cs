using System.Threading.Tasks;
using HealthyJuices.Domain.Providers;

namespace HealthyJuices.Application.Providers
{
    public class EmailProvider
    {
        private readonly IMailer _mailer;

        public EmailProvider(IMailer mailer)
        {
            _mailer = mailer;
        }

        public async Task SendRegisterCodeEmail(string email, string url, string token)
        {
             await _mailer.SendAsync(email, "Register Veryfication", $"Confirm: '{url}?email={email}&token={token}'");
        }

        public async Task SendForgotPasswordEmail(string email, string url, string token)
        {
            await _mailer.SendAsync(email, "Reset Password", $"Confirm: '{url}?email={email}&token={token}'");
        }
    }
}