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
            // 0. Google Apps Script Webhook (Direct Official Gmail Delivery - 100% Free, 0% Spam, No Domain Needed)
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
                    using var httpClient = new HttpClient();
                    var payload = new
                    {
                        to = toEmail.Trim(),
                        subject = subject,
                        html = body
                    };

                    var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    var httpResponse = await httpClient.PostAsync(webhookUrl.Trim(), jsonContent);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[EmailService] Gmail Webhook SUCCESS: E-posta başarıyla iletildi: {toEmail}");
                        return;
                    }
                    else
                    {
                        var errorBody = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"[EmailService] Gmail Webhook Hatası ({httpResponse.StatusCode}): {errorBody}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EmailService] Gmail Webhook Exception: {ex.Message}");
                }
            }

            // 1. Brevo HTTPS API (Port 443)
            var brevoApiKey = _configuration["EmailSettings:BrevoApiKey"] 
                ?? _configuration["BREVO_API_KEY"] 
                ?? _configuration["EmailSettings__BrevoApiKey"]
                ?? Environment.GetEnvironmentVariable("EmailSettings__BrevoApiKey")
                ?? Environment.GetEnvironmentVariable("BREVO_API_KEY")
                ?? Environment.GetEnvironmentVariable("EmailSettings:BrevoApiKey");

            var resendApiKey = _configuration["EmailSettings:ResendApiKey"]
                ?? _configuration["RESEND_API_KEY"]
                ?? _configuration["EmailSettings__ResendApiKey"]
                ?? Environment.GetEnvironmentVariable("EmailSettings__ResendApiKey")
                ?? Environment.GetEnvironmentVariable("RESEND_API_KEY");

            var senderEmail = _configuration["EmailSettings:SenderEmail"] 
                ?? _configuration["SENDER_EMAIL"] 
                ?? _configuration["EmailSettings__SenderEmail"]
                ?? Environment.GetEnvironmentVariable("EmailSettings__SenderEmail")
                ?? Environment.GetEnvironmentVariable("SENDER_EMAIL")
                ?? "oz2003gun5@gmail.com";

            var senderName = _configuration["EmailSettings:SenderName"] ?? "Akıllı Restoran Sistemi";

            Console.WriteLine($"[EmailService] Gönderim başlıyor -> Alıcı: {toEmail}, WebhookPresent: {!string.IsNullOrWhiteSpace(webhookUrl)}, BrevoKey: {!string.IsNullOrWhiteSpace(brevoApiKey)}, ResendKey: {!string.IsNullOrWhiteSpace(resendApiKey)}, Gönderici: {senderEmail}");

            if (!string.IsNullOrWhiteSpace(brevoApiKey))
            {
                try
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Add("api-key", brevoApiKey.Trim());

                    var payload = new
                    {
                        sender = new { name = senderName, email = senderEmail.Trim() },
                        to = new[] { new { email = toEmail.Trim() } },
                        subject = subject,
                        htmlContent = body
                    };

                    var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    var httpResponse = await httpClient.PostAsync("https://api.brevo.com/v3/smtp/email", jsonContent);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[EmailService] Brevo HTTPS API SUCCESS: E-posta başarıyla iletildi: {toEmail}");
                        return;
                    }
                    else
                    {
                        var errorBody = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"[EmailService] Brevo HTTPS API Hatası ({httpResponse.StatusCode}): {errorBody}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EmailService] Brevo HTTPS API Exception: {ex.Message}");
                }
            }

            // 2. Resend HTTPS API (Port 443)
            if (!string.IsNullOrWhiteSpace(resendApiKey))
            {
                try
                {
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", resendApiKey.Trim());

                    var payload = new
                    {
                        from = $"{senderName} <onboarding@resend.dev>",
                        to = new[] { toEmail.Trim() },
                        subject = subject,
                        html = body
                    };

                    var jsonContent = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
                    var httpResponse = await httpClient.PostAsync("https://api.resend.com/emails", jsonContent);

                    if (httpResponse.IsSuccessStatusCode)
                    {
                        Console.WriteLine($"[EmailService] Resend HTTPS API SUCCESS: E-posta başarıyla iletildi: {toEmail}");
                        return;
                    }
                    else
                    {
                        var errorBody = await httpResponse.Content.ReadAsStringAsync();
                        Console.WriteLine($"[EmailService] Resend HTTPS API Hatası ({httpResponse.StatusCode}): {errorBody}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[EmailService] Resend HTTPS API Exception: {ex.Message}");
                }
            }

            // 3. Direct SMTP Fallback (Ports 587 & 465)
            var smtpServer = _configuration["EmailSettings:SmtpServer"] ?? "smtp.gmail.com";
            var portStr = _configuration["EmailSettings:Port"] ?? "587";
            var password = _configuration["EmailSettings:Password"] ?? "rctr wwhx qynm iksd";

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
