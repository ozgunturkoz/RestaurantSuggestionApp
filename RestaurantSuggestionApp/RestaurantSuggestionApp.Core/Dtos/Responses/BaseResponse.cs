namespace RestaurantSuggestionApp.Core.Dtos.Responses
{
    public class BaseResponse<T>
    {
        public T? Data { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public string Message => ErrorMessage;

        public BaseResponse(T? data, bool isSuccess, string errorMessage)
        {
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage ?? string.Empty;
        }

        public static BaseResponse<T> Success(T? data) => new BaseResponse<T>(data, true, string.Empty);
        public static BaseResponse<T> Success(T? data, string message) => new BaseResponse<T>(data, true, message);

        public static BaseResponse<T> Fail(string errorMessage) => new BaseResponse<T>(default, false, errorMessage);
    }
}
