using System;

namespace RestaurantSuggestionApp.Core.DTOs.Requests
{
    // DTO (Data Transfer Object): Kullanıcıdan gelen veya kullanıcıya giden verileri düzenli bir paket halinde taşımak için oluşturduğumuz sınıflardır.
    
    // -------------------------------------------------------------
    // GİRİŞ (LOGIN) İÇİN KULLANILAN VERİ PAKETİ
    // Kullanıcı sisteme giriş yaparken e-posta ve şifresini yazar, bu sınıf o iki bilgiyi arka plana taşır.
    // -------------------------------------------------------------
    public class LoginDto
    {
        // Kullanıcının girdiği e-posta adresi
        public string Email { get; set; } = string.Empty;

        // Kullanıcının girdiği şifre
        public string Password { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // KAYIT (REGISTER) İÇİN KULLANILAN VERİ PAKETİ
    // Yeni bir üye olurken ad, e-posta, şifre ve şifre tekrarını taşır.
    // -------------------------------------------------------------
    public class RegisterDto
    {
        // Kullanıcının adı ve soyadı
        public string Name { get; set; } = string.Empty;

        // Kullanıcının e-posta adresi
        public string Email { get; set; } = string.Empty;

        // Kullanıcının belirlediği ilk şifre
        public string Password { get; set; } = string.Empty;

        // Şifrenin doğru yazıldığını teyit etmek için ikinci kez girilen şifre
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // GENEL KULLANICI BİLGİSİ TAŞIMA SINIFI
    // Yönetim veya kullanıcı güncelleme işlemlerinde kullanılır.
    // -------------------------------------------------------------
    public class UserDto
    {
        // Kullanıcının sistemdeki benzersiz kimlik numarası (ID)
        public int Id { get; set; }

        // Kullanıcı adı
        public string Name { get; set; } = string.Empty;

        // Kullanıcı e-postası
        public string Email { get; set; } = string.Empty;

        // İsteğe bağlı şifre alanı
        public string? Password { get; set; }

        // Eğer kullanıcı bir restoran ile ilişkiliyse o restoranın ID'si
        public int? RestaurantId { get; set; }
    }

    // -------------------------------------------------------------
    // GİRİŞ DOĞRULAMA KODU TAŞIMA SINIFI (2FA)
    // İki adımlı doğrulamada e-postaya gelen 6 haneli kodu ve e-postayı taşır.
    // -------------------------------------------------------------
    public class VerifyLoginDto
    {
        // Doğrulama yapılacak e-posta adresi
        public string Email { get; set; } = string.Empty;

        // E-postaya gönderilen 6 haneli güvenlik kodu
        public string Code { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // KAYIT DOĞRULAMA KODU TAŞIMA SINIFI
    // Kayıt olduktan sonra hesabı aktifleştirmek için girilen kodu taşır.
    // -------------------------------------------------------------
    public class VerifyRegisterDto
    {
        // Kayıt olan e-posta adresi
        public string Email { get; set; } = string.Empty;

        // E-postaya gönderilen 6 haneli aktivasyon kodu
        public string Code { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // GİRİŞ CEVAP (RESPONSE) SINIFI
    // Giriş yapıldığında ön yüze kod mu beklendiğini yoksa doğrudan oturum anahtarı (Token) mı verildiğini döner.
    // -------------------------------------------------------------
    public class LoginResponseDto
    {
        // Kod ile onaylama adımı gerekiyor mu? (True/False)
        public bool RequiresVerification { get; set; }

        // Doğrulamanın gideceği e-posta adresi
        public string Email { get; set; } = string.Empty;

        // Giriş tamamlandıysa üretilen güvenli oturum anahtarı (JWT Token)
        public string? Token { get; set; }
    }

    // -------------------------------------------------------------
    // ŞİFREMİ UNUTTUM KOD İSTEME PAKETİ
    // Şifresini unutan kullanıcının sıfırlama kodu almak için girdiği e-posta bilgisini taşır.
    // -------------------------------------------------------------
    public class ForgotPasswordDto
    {
        // Şifresi sıfırlanacak hesabın kayıtlı e-posta adresi
        public string Email { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // ŞİFREYİ KOD İLE SIFIRLAMA PAKETİ
    // E-postaya gelen kod ile yeni şifreyi belirleyip kaydeden veri paketi.
    // -------------------------------------------------------------
    public class ResetPasswordDto
    {
        // Hesabın e-posta adresi
        public string Email { get; set; } = string.Empty;

        // E-postaya gelen 6 haneli şifre sıfırlama kodu
        public string Code { get; set; } = string.Empty;

        // Kullanıcının belirlediği yeni şifre
        public string NewPassword { get; set; } = string.Empty;

        // Yeni şifrenin doğrulaması için tekrarı
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // YENİ HARCAMA OLUŞTURMA PAKETİ
    // Kullanıcının bir restoranda yaptığı harcamayı sisteme kaydetmesi için kullanılır.
    // -------------------------------------------------------------
    public class CreateExpenditureDto
    {
        // Harcama yapılan restoranın ID numarası (isteğe bağlı)
        public int? RestaurantId { get; set; }

        // Harcanan para tutarı (TL)
        public decimal Amount { get; set; }

        // Harcamanın yapıldığı tarih ve saat
        public DateTime? SpentAt { get; set; }
    }

    // -------------------------------------------------------------
    // HARCAMA DETAYINI KULLANICIYA GÖSTERME PAKETİ
    // Kullanıcının geçmiş harcamalarını ekrana listelemek için gönderilen zengin veri paketi.
    // -------------------------------------------------------------
    public class UserExpenditureResponseDto
    {
        // Harcamanın kendi benzersiz numarası
        public int Id { get; set; }

        // Harcamayı yapan kullanıcının numarası
        public int UserId { get; set; }

        // Restoran numarası
        public int? RestaurantId { get; set; }

        // Restoranın ekranda gözükecek adı (örn: Çiya Sofrası)
        public string? RestaurantName { get; set; }

        // Restoranın kategorisi (örn: Ev Yemekleri, Kebap)
        public string? RestaurantCategory { get; set; }

        // Harcanan para tutarı (TL)
        public decimal Amount { get; set; }

        // Harcamanın yapıldığı tarih
        public DateTime SpentAt { get; set; }
    }

    // -------------------------------------------------------------
    // PROFİL İSMİ GÜNCELLEME PAKETİ
    // Kullanıcı profil sayfasında adını değiştirmek istediğinde bu paketi gönderir.
    // -------------------------------------------------------------
    public class UpdateProfileDto
    {
        // Yeni ad soyad
        public string Name { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // OTURUM AÇIKKEN KOD İLE ŞİFRE DEĞİŞTİRME PAKETİ
    // Kullanıcı profilde şifresini değiştirmek istediğinde e-postasına gelen kodu ve yeni şifresini taşır.
    // -------------------------------------------------------------
    public class ChangePasswordWithCodeDto
    {
        // E-postaya gönderilen 6 haneli kod
        public string Code { get; set; } = string.Empty;

        // İstenen yeni şifre
        public string NewPassword { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // E-POSTA DEĞİŞTİRME İÇİN KOD İSTEME PAKETİ
    // Kullanıcı profilinden e-posta değiştirmek istediğinde yeni adresi taşır.
    // -------------------------------------------------------------
    public class SendEmailChangeCodeDto
    {
        // Kullanıcının geçmek istediği yeni e-posta adresi
        public string NewEmail { get; set; } = string.Empty;
    }

    // -------------------------------------------------------------
    // E-POSTA DEĞİŞTİRMEYİ ONAYLAMA PAKETİ
    // Yeni e-postaya gelen onay kodunu ve güvenlik için mevcut şifreyi taşır.
    // -------------------------------------------------------------
    public class ChangeEmailWithCodeDto
    {
        // Yeni e-posta adresi
        public string NewEmail { get; set; } = string.Empty;

        // Yeni e-postaya gelen 6 haneli onay kodu
        public string Code { get; set; } = string.Empty;

        // Güvenlik doğrulaması için mevcut şifre
        public string CurrentPassword { get; set; } = string.Empty;
    }
}
