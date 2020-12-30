using System.Collections.Generic;
using System.Threading.Tasks;

namespace HealthyJuices.Domain.Services
{
    public interface IMailer
    {
        Task<bool> Send(string recipient, string subject, string body, bool isBodyHtml = false, params string[] ccs);
    }
}