using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
            var webhookUrl = _configuration["EmailSettings:WebhookUrl"]
                ?? _configuration["GMAIL_WEBHOOK_URL"]
                ?? _configuration["EmailSettings__WebhookUrl"]
                ?? Environment.GetEnvironmentVariable("EmailSettings__WebhookUrl")
                ?? Environment.GetEnvironmentVariable("GMAIL_WEBHOOK_URL")
                ?? Environment.GetEnvironmentVariable("EmailSettings:WebhookUrl");

            if (!string.IsNullOrWhiteSpace(webhookUrl))
            {
                try
                {
                    var handler = new HttpClientHandler { AllowAutoRedirect = true };
                    using var httpClient = new HttpClient(handler);
                    httpClient.Timeout = TimeSpan.FromSeconds(15);

                    var payload = new
                    {
                        to = toEmail.Trim(),
                        subject = subject,
                        html = body
                    };

                    var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    var httpResponse = await httpClient.PostAsync(webhookUrl.Trim(), jsonContent);

                    if (httpResponse.IsSuccessStatusCode || (int)httpResponse.StatusCode == 302)
                    {
                        return;
                    }
                }
                catch
                {
                }
            }

            var senderEmail = _configuration["EmailSettings:SenderEmail"]
                ?? _configuration["SENDER_EMAIL"]
                ?? _configuration["EmailSettings__SenderEmail"]
                ?? Environment.GetEnvironmentVariable("EmailSettings__SenderEmail")
                ?? Environment.GetEnvironmentVariable("SENDER_EMAIL")
                ?? "oz2003gun5@gmail.com";

            var senderName = _configuration["EmailSettings:SenderName"] ?? "Sofra Restoran Sistemi";

            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var portStr = _configuration["EmailSettings:Port"] ?? "587";
            var password = _configuration["EmailSettings:Password"] ?? "rctr wwhx qynm iksd";

            if (string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(password))
            {
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

                var bodyBuilder = new BodyBuilder { HtmlBody = body };
                message.Body = bodyBuilder.ToMessageBody();

                using var client = new SmtpClient();
                client.Timeout = 10000;

                var options = port == 465 ? SecureSocketOptions.SslOnConnect : SecureSocketOptions.StartTls;
                await client.ConnectAsync(smtpServer, port, options);
                await client.AuthenticateAsync(senderEmail, cleanPassword);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch
            {
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
                        client465.Timeout = 10000;
                        await client465.ConnectAsync(smtpServer, 465, SecureSocketOptions.SslOnConnect);
                        await client465.AuthenticateAsync(senderEmail, cleanPassword);
                        await client465.SendAsync(message);
                        await client465.DisconnectAsync(true);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }
}
