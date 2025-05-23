using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using System.Text.Json;

namespace IGwApi.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public class UncontrolledExceptionRaised
        {
            public string? Code { get; set; }
            public string? Message { get; set; }
        }

        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if (contextFeature != null)
                    {
                        var errors = new List<UncontrolledExceptionRaised>();
                        errors.Add(new UncontrolledExceptionRaised() { Code = "UncontroledException", Message = contextFeature.Error.GetFullMessage() + ". StackTrace:" + contextFeature.Error.StackTrace });
                        var response = BuildHttpErrorResponse(errors);

                        await context.Response.WriteAsync(response);
                    }
                });
            });
        }

        private static string BuildHttpErrorResponse(List<UncontrolledExceptionRaised> errors)
        {
            return JsonSerializer.Serialize(errors);
        }

    }
}
