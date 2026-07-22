namespace RestaurantSuggestionApp.Core.Dtos.Responses
{
    public class BaseResponse<T>
    {
        public T? Data { get; set; }

        public bool IsSuccess { get; set; }

        public string ErrorMessage { get; set; }

        public string ErrorMessagea { get; set; }

        public BaseResponse(T data, bool isSuccess, string errorMessage)
        {
            Data = data;
            IsSuccess = isSuccess;
            ErrorMessage = errorMessage;

            ErrorMessagea = errorMessage;

        }
        public static BaseResponse<T> Success(T data) => new BaseResponse<T>(data, true, string.Empty);
        public static BaseResponse<T> Fail(string errorMessage) => new BaseResponse<T>(default, false, errorMessage);
    }
}
