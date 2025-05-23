using Crosscutting.AppSettings;

namespace IGwApi.Extensions
{
    public static class AppSettingsExtensions
    {
        public static IServiceCollection AddAppSettings(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettingsConfiguration = new AppSettings();
            configuration.GetSection("AppSettings").Bind(appSettingsConfiguration);
            services.AddSingleton(appSettingsConfiguration);

            return services;
        }
    }
}
