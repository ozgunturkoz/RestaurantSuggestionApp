using Microsoft.Extensions.Configuration;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading;
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
            var smtpServer = _configuration["EmailSettings:SmtpServer"];
            var portStr = _configuration["EmailSettings:Port"];
            var senderEmail = _configuration["EmailSettings:SenderEmail"];
            var password = _configuration["EmailSettings:Password"];
            var senderName = _configuration["EmailSettings:SenderName"] ?? "Akıllı Restoran Sistemi";

            if (string.IsNullOrWhiteSpace(smtpServer) || string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine($"[EmailService] SMTP ayarları tanımlanmadığı için e-posta gönderimi simüle edildi ({toEmail}): {subject}");
                return;
            }

            if (!int.TryParse(portStr, out int port))
            {
                port = 587;
            }

            try
            {
                using var client = new SmtpClient(smtpServer, port)
                {
                    Credentials = new NetworkCredential(senderEmail, password),
                    EnableSsl = true,
                    Timeout = 3000
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(senderEmail, senderName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(3));
                await client.SendMailAsync(mailMessage, cts.Token);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] E-posta gönderim uyarısı ({toEmail}): {ex.Message}");
            }
        }
    }
}
