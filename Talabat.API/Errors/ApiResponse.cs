namespace Talabat.API.Errors
{
    public class ApiResponse(int statusCode, string message = null)
    {
        public int StatusCode { get; set; } = statusCode;
        public string Message { get; set; } = message;


        public string? GetDefaultMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource was not found",
                500 => "Looks like something went wrong",
                _ => null
            };
        }
    }
}
