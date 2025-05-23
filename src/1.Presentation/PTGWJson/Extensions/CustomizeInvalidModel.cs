using Domain.Error;
using Microsoft.AspNetCore.Mvc;
using PTGWJuniper.Models;

namespace IGwApi.Extensions
{
    public static class CustomizeInvalidModel
    {
        public static IServiceCollection AddCustomizedInvalidResponse(this IServiceCollection services)
        {
            services.AddMvc().ConfigureApiBehaviorOptions(options =>
            {           
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var modelState = actionContext.ModelState;

                    var errors = new ExceptionResponse();
                    foreach (var state in modelState)                    
                        foreach (var error in state.Value.Errors)                        
                            errors.Errors.Add(new Domain.Error.Error("Parsing", error.ErrorMessage, ErrorType.Parse, CategoryErrorType.Hub));                        
                    
                    return new OkObjectResult(errors);

                };
            });
            return services;
        }
    }
}
