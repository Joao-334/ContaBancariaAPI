namespace ContaBancariaAPI.Exceptions;
public class ApiException : Exception
{
    public int StatusCode { get; set; }
    public string Message { get; set; }
    public ApiException(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
    }
}
