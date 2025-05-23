using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO.Compression;
using System.Net;
using System.Net.Http;

namespace IGwApi.Extensions
{
    public static class HttpClientExtensions
    {
        public static IServiceCollection AddHttpClientPPN(this IServiceCollection services)
        {
            services.AddHttpClient("Av", client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new SocketsHttpHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };
                return handler;
            }).SetHandlerLifetime(new TimeSpan(0, 30, 0));


            services.AddHttpClient("Booking", client =>
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.Timeout = TimeSpan.FromMilliseconds(120000);
            })
            .ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new SocketsHttpHandler
                {
                    AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                };
                return handler;
            }).SetHandlerLifetime(new TimeSpan(0, 30, 0)); ;

            return services;


        }

        public static IServiceCollection AddResponseCompressionGw(this IServiceCollection services)
        {
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.Configure<GzipCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });

            return services;
        }
    }
}
