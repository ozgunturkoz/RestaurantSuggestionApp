namespace RestaurantSuggestionApp.Core.Dtos.Responses
{
    // =========================================================================================
    // BU SINIF (CLASS) NE İŞE YARAR? (KODLAMA BİLMEYENLER İÇİN ANLATIM)
    // Backend'in kullanıcıya verdiği standart "Cevap Zarfıdır".
    // Bir kargo paketi gibi; içinde taşınan asıl veri (Data), işlemin başarılı olup olmadığı (IsSuccess)
    // ve varsa kullanıcıya gösterilecek mesaj/hata metni (ErrorMessage / Message) yer alır.
    // =========================================================================================
    public class BaseResponse<T>
    {
        // İstenen asıl veri (örneğin kullanıcı bilgisi, restoran listesi vb.)
        public T? Data { get; set; }

        // İşlem başarılı mı bitti? (True: Evet, False: Hayır)
        public bool IsSuccess { get; set; }

        // Hata varsa hatanın sebebi, başarı varsa bilgilendirme mesajı
        public string ErrorMessage { get; set; } = string.Empty;

        // Ön yüzle tam uyum için mesaj alanı
        public string Message => ErrorMessage;

        // Kurucu Metot
        public BaseResponse(T? data, bool isSuccess, string errorMessage)
        {
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage ?? string.Empty;
        }

        // Başarılı durumlar için pratik üretim metotları
        public static BaseResponse<T> Success(T? data) => new BaseResponse<T>(data, true, string.Empty);
        public static BaseResponse<T> Success(T? data, string message) => new BaseResponse<T>(data, true, message);

        // Hatalı durumlar için pratik üretim metodu
        public static BaseResponse<T> Fail(string errorMessage) => new BaseResponse<T>(default, false, errorMessage);
    }
}
