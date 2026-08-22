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
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var portStr = _configuration["EmailSettings:Port"] ?? "587";
            var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? "oz2003gun5@gmail.com";
            var password = _configuration["EmailSettings:Password"] ?? "rctr wwhx qynm iksd";
            var senderName = _configuration["EmailSettings:SenderName"] ?? "Akıllı Restoran Sistemi";

            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(password))
            {
                Console.WriteLine("[EmailService] E-posta ayarları eksik.");
                return;
            }

            var cleanPassword = password.Replace(" ", "").Trim();
            int port = int.TryParse(portStr, out var p) ? p : 587;

            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls13;

            try
            {
                using var client = new SmtpClient(smtpServer, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail, cleanPassword),
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Timeout = 8000
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
                Console.WriteLine($"[EmailService] E-posta başarıyla gönderildi: {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] Port {port} başarısız oldu ({ex.Message}). Port 465 deneniyor...");
                try
                {
                    using var client465 = new SmtpClient(smtpServer, 465)
                    {
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential(senderEmail, cleanPassword),
                        EnableSsl = true,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Timeout = 8000
                    };

                    var mailMessage465 = new MailMessage
                    {
                        From = new MailAddress(senderEmail, senderName),
                        Subject = subject,
                        Body = body,
                        IsBodyHtml = true
                    };

                    mailMessage465.To.Add(toEmail);
                    await client465.SendMailAsync(mailMessage465);
                    Console.WriteLine($"[EmailService] E-posta Port 465 üzerinden başarıyla gönderildi: {toEmail}");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"[EmailService] Gmail SMTP 587 ve 465 portlarının ikisi de başarısız: {ex2.Message}");
                }
            }
        }
    }
}
