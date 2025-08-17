using CoffeeShop.Infrastructure.Shared.ErrorCodes;
using CoffeeShop.Utilities;

namespace CoffeeShop.Infrastructure.Shared.Wrapper
{
    public class Response<T>
    {
        public Response()
        {
        }

        public Response(T data, string message = null)
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }

        public Response(string errorCode, string message)
        {
            Succeeded = false;
            Message = message;
            ErrorCode = string.IsNullOrWhiteSpace(errorCode) ? ErrorCodeEnum.COM_ERR_000.ToString() : errorCode;
        }

        public Response(ErrorCodeEnum errorCode)
        {
            Succeeded = false;
            Message = errorCode.GetDescription();
            ErrorCode = errorCode.ToString();
        }

        public bool Succeeded { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; } = null;
        public List<string> Errors { get; set; }
        public T Data { get; set; }
    }
}