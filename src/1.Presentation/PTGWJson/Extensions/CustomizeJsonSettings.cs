using System.Text.Json.Serialization;

namespace IGwApi.Extensions
{
    public static class CustomizeJsonSettings
    {
        public static IServiceCollection AddJsonSettings(this IServiceCollection services)
        {
            services.AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                x.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
            return services;
        }
    }
}
