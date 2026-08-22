using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
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

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(senderName, senderEmail));
                message.To.Add(new MailboxAddress("", toEmail));
                message.Subject = subject;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                client.Timeout = 15000;

                var options = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                await client.ConnectAsync(smtpServer, port, options);
                await client.AuthenticateAsync(senderEmail, cleanPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                Console.WriteLine($"[EmailService] MailKit via Gmail SUCCESS: Email sent to {toEmail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[EmailService] MailKit Port {port} error: {ex.Message}");
                if (port == 587)
                {
                    try
                    {
                        var message = new MimeMessage();
                        message.From.Add(new MailboxAddress(senderName, senderEmail));
                        message.To.Add(new MailboxAddress("", toEmail));
                        message.Subject = subject;
                        var bodyBuilder = new BodyBuilder { HtmlBody = body };
                        message.Body = bodyBuilder.ToMessageBody();

                        using var client465 = new SmtpClient();
                        client465.Timeout = 15000;
                        await client465.ConnectAsync(smtpServer, 465, SecureSocketOptions.SslOnConnect);
                        await client465.AuthenticateAsync(senderEmail, cleanPassword);
                        await client465.SendAsync(message);
                        await client465.DisconnectAsync(true);

                        Console.WriteLine($"[EmailService] MailKit Port 465 SUCCESS: Email sent to {toEmail}");
                    }
                    catch (Exception ex2)
                    {
                        Console.WriteLine($"[EmailService] MailKit Port 465 fallback error: {ex2.Message}");
                    }
                }
            }
        }
    }
}
