using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using RestaurantSuggestionApp.DataAccess;

namespace RestaurantSuggestionApp.WebApi.Localizer;

public class SqlStringLocalizer : IStringLocalizer
{
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly IMemoryCache _cache;

    private readonly string? _culture;

    private static readonly Dictionary<string, (string Tr, string En)> StaticFallbacks = new(StringComparer.OrdinalIgnoreCase)
    {
        ["ForgotPasswordEmailSubject"] = ("Şifre Sıfırlama Kodunuz - Sofra Restoran", "Your Password Reset Code - Sofra Restaurant"),
        ["ForgotPasswordEmailBody"] = ("<h3>Merhaba {0},</h3><p>Şifrenizi sıfırlamak için kullanabileceğiniz doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>Bu kod 10 dakika süreyle geçerlidir.</p>", "<h3>Hello {0},</h3><p>Your verification code to reset your password is: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>This code is valid for 10 minutes.</p>"),
        ["ForgotPasswordSuccess"] = ("Şifre sıfırlama kodu e-posta adresinize gönderildi.", "Password reset code has been sent to your email address."),
        ["PasswordResetSuccess"] = ("Şifreniz başarıyla güncellendi. Yeni şifrenizle giriş yapabilirsiniz.", "Your password has been reset successfully. You can now log in."),
        ["LoginVerificationEmailSubject"] = ("Giriş Doğrulama Kodunuz - Sofra Restoran", "Your Login Verification Code - Sofra Restaurant"),
        ["LoginVerificationEmailBody"] = ("<h3>Merhaba {0},</h3><p>Giriş yapabilmek için doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>Bu kod 10 dakika süreyle geçerlidir.</p>", "<h3>Hello {0},</h3><p>Your verification code to sign in is: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>This code is valid for 10 minutes.</p>"),
        ["WelcomeEmailSubject"] = ("Sofra Restoran'a Hoş Geldiniz!", "Welcome to Sofra Restaurant!"),
        ["WelcomeEmailBody"] = ("<h3>Merhaba {0},</h3><p>E-posta adresiniz başarıyla doğrulandı. Hesabınız aktif edildi!</p><p>Sofra Restoran Akıllı Öneri Sistemini keyifle kullanabilirsiniz.</p>", "<h3>Hello {0},</h3><p>Your email address has been verified successfully. Your account is activated!</p><p>Enjoy using the Sofra Restaurant Smart Recommendation System.</p>"),
        ["RegisterVerificationEmailSubject"] = ("Sofra Restoran - Hesabınızı Doğrulayın", "Sofra Restaurant - Verify Your Account"),
        ["RegisterVerificationEmailBody"] = ("<h3>Aramıza Hoş Geldiniz {0}!</h3><p>Kayıt işleminizi tamamlamak için doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>Bu kod 10 dakika süreyle geçerlidir.</p>", "<h3>Welcome Aboard {0}!</h3><p>Your verification code to complete registration is: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>This code is valid for 10 minutes.</p>"),
        ["PasswordChangeEmailSubject"] = ("Şifre Değiştirme Kodunuz - Sofra Restoran", "Your Password Change Code - Sofra Restaurant"),
        ["PasswordChangeEmailBody"] = ("<h3>Merhaba {0},</h3><p>Şifrenizi değiştirmek için kullanabileceğiniz doğrulama kodunuz: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>Bu kod 5 dakika süreyle geçerlidir.</p>", "<h3>Hello {0},</h3><p>Your verification code to change your password is: <b style='font-size: 24px; color: #E0A458;'>{1}</b></p><p>This code is valid for 5 minutes.</p>"),
        ["EmailChangeEmailSubject"] = ("Sofra - E-Posta Değişikliği Doğrulama Kodu", "Sofra - Email Change Verification Code"),
        ["EmailChangeEmailBody"] = ("<h3>Merhaba {0},</h3><p>Hesabınızın e-posta adresini <strong>{1}</strong> olarak değiştirmek üzeresiniz.</p><p>Doğrulama Kodunuz: <b style='font-size: 24px; color: #E0A458;'>{2}</b></p><p>Bu kod 5 dakika boyunca geçerlidir.</p>", "<h3>Hello {0},</h3><p>You are about to change your account email to <strong>{1}</strong>.</p><p>Verification Code: <b style='font-size: 24px; color: #E0A458;'>{2}</b></p><p>This code is valid for 5 minutes.</p>"),
        ["InvalidEmailFormat"] = ("Lütfen geçerli bir e-posta adresi giriniz.", "Please enter a valid email address."),
        ["PasswordsDoNotMatch"] = ("Girdiğiniz şifreler birbiriyle eşleşmiyor.", "Passwords do not match."),
        ["UserNotFound"] = ("Kullanıcı bulunamadı.", "User not found."),
        ["PasswordResetCodeSendError"] = ("Şifre sıfırlama kodu gönderilirken hata oluştu.", "An error occurred while sending the password reset code."),
        ["InvalidVerificationCode"] = ("Geçersiz veya süresi dolmuş doğrulama kodu!", "Invalid or expired verification code!"),
        ["InvalidPassword"] = ("E-posta veya şifre hatalı!", "Invalid email or password!"),
        ["InvalidInput"] = ("E-posta ve şifre boş bırakılamaz.", "Email and password cannot be empty.")
    };

    public SqlStringLocalizer(IServiceScopeFactory scopeFactory, IMemoryCache cache, string? culture = null)
    {
        _scopeFactory = scopeFactory;
        _cache = cache;
        _culture = culture;
    }

    public LocalizedString this[string name]
    {
        get
        {
            var value = GetString(name);
            return new LocalizedString(name, value ?? name, resourceNotFound: value == null);
        }
    }

    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var format = GetString(name);
            if (format == null)
            {
                return new LocalizedString(name, name, resourceNotFound: true);
            }
            try
            {
                var value = string.Format(format, arguments);
                return new LocalizedString(name, value, resourceNotFound: false);
            }
            catch
            {
                return new LocalizedString(name, format, resourceNotFound: false);
            }
        }
    }

    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        var dict = GetDictionary();
        return dict.Select(kv => new LocalizedString(kv.Key, kv.Value, resourceNotFound: false));
    }

    public Dictionary<string, string> GetDictionary(string? cultureName = null)
    {
        var culture = cultureName ?? _culture ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName.ToLowerInvariant();
        culture = culture.ToLowerInvariant();

        var cacheKey = $"localization_dict_{culture}";

        if (_cache.TryGetValue(cacheKey, out Dictionary<string, string>? cachedDict) && cachedDict != null)
        {
            return cachedDict;
        }

        var dictionary = LoadFromDatabase(culture);

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromHours(24));

        _cache.Set(cacheKey, dictionary, cacheOptions);

        return dictionary;
    }

    public void ClearCache(string? cultureName = null)
    {
        if (!string.IsNullOrWhiteSpace(cultureName))
        {
            _cache.Remove($"localization_dict_{cultureName.ToLowerInvariant()}");
        }
        else
        {
            _cache.Remove("localization_dict_tr");
            _cache.Remove("localization_dict_en");
        }
    }

    private string? GetString(string key)
    {
        var activeCulture = (_culture ?? CultureInfo.CurrentUICulture.TwoLetterISOLanguageName).ToLowerInvariant();
        var shortCulture = activeCulture.Split('-')[0];

        var dict = GetDictionary(shortCulture);
        if (dict.TryGetValue(key, out var val) && !string.IsNullOrWhiteSpace(val))
        {
            return val;
        }

        var trDict = GetDictionary("tr");
        if (trDict.TryGetValue(key, out var trVal) && !string.IsNullOrWhiteSpace(trVal))
        {
            return trVal;
        }

        if (StaticFallbacks.TryGetValue(key, out var fallback))
        {
            return shortCulture == "en" ? fallback.En : fallback.Tr;
        }

        return null;
    }

    private Dictionary<string, string> LoadFromDatabase(string culture)
    {
        using var scope = _scopeFactory.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var shortCulture = culture.Split('-')[0];

        var records = context.LocalizationResources
            .AsNoTracking()
            .Where(r => r.Culture == culture || r.Culture == shortCulture)
            .ToList();

        var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        foreach (var item in records)
        {
            if (!dict.ContainsKey(item.ResourceKey))
            {
                dict[item.ResourceKey] = item.Value;
            }
        }

        if (dict.Count == 0 && shortCulture != "tr")
        {
            var fallbackRecords = context.LocalizationResources
                .AsNoTracking()
                .Where(r => r.Culture == "tr")
                .ToList();

            foreach (var item in fallbackRecords)
            {
                if (!dict.ContainsKey(item.ResourceKey))
                {
                    dict[item.ResourceKey] = item.Value;
                }
            }
        }

        return dict;
    }
}
