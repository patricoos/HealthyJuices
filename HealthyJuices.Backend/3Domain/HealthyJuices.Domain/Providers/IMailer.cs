﻿using System.Threading.Tasks;

namespace HealthyJuices.Domain.Providers
{
    public interface IMailer
    {
        Task SendAsync(string recipient, string subject, string body, bool isBodyHtml = false, params string[] ccs);
    }
}