using ContaBancariaAPI.Exceptions;

namespace ContaBancariaAPI.Middlewares;
public class TratamentoDeExcecoes
{
    private readonly RequestDelegate _next;

    public TratamentoDeExcecoes(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);  
        }

        catch (ApiException ex) 
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            await context.Response.WriteAsJsonAsync(new { Message = ex.Message });
        }
    }
}
