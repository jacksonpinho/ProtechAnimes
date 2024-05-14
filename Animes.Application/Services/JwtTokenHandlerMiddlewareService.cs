using Microsoft.AspNetCore.Http;

public class JwtTokenHandlerMiddlewareService
{
    private readonly RequestDelegate _next;

    public JwtTokenHandlerMiddlewareService(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        var token = context.Request.Cookies["authToken"];

        // Verifica se o token está presente
        if (!string.IsNullOrEmpty(token))
        {
            // Se o token estiver presente, consideramos o usuário autenticado
            context.Request.Headers.Add("Authorization", "Bearer " + token);
            Console.WriteLine(token);
        }

        await _next(context);
    }
}
