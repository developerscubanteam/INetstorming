using Application.System.Contracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;

public class BasicAuthenticationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IAuthorizationService _authorizationService;

    public BasicAuthenticationMiddleware(RequestDelegate next, IAuthorizationService authorizationService)
    {
        _next = next;
        _authorizationService = authorizationService;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!RequiresAuthentication(context.Request.Path.Value))
        {
            await _next.Invoke(context);
            return;
        }

        string? authHeader = context.Request.Headers["x-api-key"];
        if (authHeader != null)
        {
            if (_authorizationService.IsAuthorized(authHeader) == false)
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            else
            {
                await _next.Invoke(context);
                return;
            }
        }
        else
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
    }

    private bool RequiresAuthentication(string? uri)
    {
        return false;
        if (uri != null)
        {
            if (uri == "/api/Probe" || uri == "/api/probe" || uri == "/api/ControlPanel")
                return false;
        }

        return true;

    }

}

public static class BasicAuthenticationMiddlewareExtensions
{
    public static IApplicationBuilder UseBasicAuthentication(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<BasicAuthenticationMiddleware>();
    }
}