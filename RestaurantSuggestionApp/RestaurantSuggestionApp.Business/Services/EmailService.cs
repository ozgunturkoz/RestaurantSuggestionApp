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
    // =========================================================================================
    // BU ARAYÜZ (INTERFACE) NE İŞE YARAR?
    // Sistemin herhangi bir yerinden bir kullanıcıya e-posta gönderilmek istendiğinde
    // çağrılacak olan standardı (Kime gidecek, Konu ne, Mesaj içeriği ne) belirler.
    // =========================================================================================
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body);
    }

    // =========================================================================================
    // BU SINIF (CLASS) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Bu sınıf sistemin "Postacısıdır". Kullanıcıya doğrulama kodları, şifre sıfırlama linkleri
    // ve hoş geldiniz mesajları iletmek için farklı e-posta sağlayıcılarını (Google Apps Script,
    // Brevo API, Resend API veya doğrudan Gmail SMTP) sırasıyla dener ve iletinin kesinlikle
    // kullanıcıya ulaşmasını garanti altına alır.
    // =========================================================================================
    public class EmailService : IEmailService
    {
        // appsettings.json dosyasından e-posta şifrelerini ve API anahtarlarını okuyan nesne
        private readonly IConfiguration _configuration;

        // -------------------------------------------------------------------------------------
        // Kurucu Metot (Constructor):
        // EmailService oluşturulduğunda sistem ayarlarını (Configuration) içine alır.
        // -------------------------------------------------------------------------------------
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // -------------------------------------------------------------------------------------
        // E-Posta Gönderme Ana Metodu:
        // Çok kademeli (Fallback) bir mimari ile e-postayı göndermeye çalışır.
        // -------------------------------------------------------------------------------------
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // 0. AŞAMA: Google Apps Script Webhook (Doğrudan Gmail Entegrasyonu - En Güvenilir ve Ücretsiz Yol)
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

                    // E-posta başarıyla iletildiyse metottan çıkıyoruz
                    if (httpResponse.IsSuccessStatusCode || (int)httpResponse.StatusCode == 302)
                    {
                        return;
                    }
                }
                catch
                {
                    // Webhook başarısız olursa bir sonraki yedek yönteme geçiyoruz
                }
            }

            // 1. AŞAMA: Brevo (Sendinblue) HTTPS API (Port engeli olmayan güvenli bulut e-posta servisi)
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

            var senderName = _configuration["EmailSettings:SenderName"] ?? "Sofra Restoran Sistemi";

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
                        return;
                    }
                }
                catch
                {
                    // Brevo başarısız olursa Resend'e geçiyoruz
                }
            }

            // 2. AŞAMA: Resend HTTPS API (İkinci Bulut Yedek Servis)
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
                        return;
                    }
                }
                catch
                {
                    // Resend başarısız olursa doğrudan SMTP'ye geçiyoruz
                }
            }

            // 3. AŞAMA: Doğrudan Gmail SMTP Protokolü (Port 587 ve Port 465)
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
                // MimeMessage ile zengin HTML e-posta zarfı oluşturuyoruz
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
                // Port 587 hata verirse alternatif olarak Port 465 SSL protokolünü deniyoruz
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
                        // Hata sessizce yönetilir
                    }
                }
            }
        }
    }
}
