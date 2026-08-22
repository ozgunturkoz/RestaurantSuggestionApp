using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace RestaurantSuggestionApp.Business.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? throw new InvalidOperationException("EmailSettings:SmtpServer konfigürasyonu eksik.");
            var port = int.Parse(_configuration["EmailSettings:Port"] ?? throw new InvalidOperationException("EmailSettings:Port konfigürasyonu eksik."));
            var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? throw new InvalidOperationException("EmailSettings:SenderEmail konfigürasyonu eksik.");
            var password = _configuration["EmailSettings:Password"] ?? throw new InvalidOperationException("EmailSettings:Password konfigürasyonu eksik.");
            var senderName = _configuration["EmailSettings:SenderName"] ?? "Akıllı Restoran Sistemi";

            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(password))
            {
                throw new System.InvalidOperationException("E-posta ayarları (SenderEmail ve Password) yapılandırılmadığı için e-posta gönderilemedi.");
            }

            var cleanPassword = password.Replace(" ", "").Trim();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            using var client = new SmtpClient(smtpServer, port)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail, cleanPassword),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Timeout = 10000
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(toEmail);
            await client.SendMailAsync(mailMessage);
        }
    }
}
