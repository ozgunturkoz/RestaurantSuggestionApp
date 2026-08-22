using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
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
            var resendApiKey = _configuration["EmailSettings:ResendApiKey"];

            // 1. Preferred Cloud Method: HTTPS REST API (Port 443 - Never Blocked by Render Firewall)
            if (!string.IsNullOrWhiteSpace(resendApiKey))
            {
                try
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resendApiKey.Trim());

                    var senderName = _configuration["EmailSettings:SenderName"] ?? "Akıllı Restoran Sistemi";
                    var payload = new
                    {
                        from = $"{senderName} <onboarding@resend.dev>",
                        to = new[] { toEmail },
                        subject = subject,
                        html = body
                    };

                    var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    var httpResponse = await httpClient.PostAsync("https://api.resend.com/emails", jsonContent);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[EmailService] Resend HTTPS API SUCCESS: Email delivered to {toEmail}");
                        return;
                    }
                    else
                    {
                        var errorBody = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"[EmailService] Resend HTTPS API error ({httpResponse.StatusCode}): {errorBody}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EmailService] Resend HTTPS API Exception: {ex.Message}");
                }
            }

            // 2. Direct SMTP Fallback (Ports 587 & 465)
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var portStr = _configuration["EmailSettings:Port"] ?? "587";
            var senderEmail = _configuration["EmailSettings:SenderEmail"] ?? "oz2003gun5@gmail.com";
            var password = _configuration["EmailSettings:Password"] ?? "rctr wwhx qynm iksd";
            var defaultSenderName = _configuration["EmailSettings:SenderName"] ?? "Akıllı Restoran Sistemi";

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
                message.From.Add(new MailboxAddress(defaultSenderName, senderEmail));
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
                        message.From.Add(new MailboxAddress(defaultSenderName, senderEmail));
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
