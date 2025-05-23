using Application.System.Contracts;
using Application.System;
using Application.WorkFlow.Contracts;
using Application.WorkFlow;
using Infrastructure.Connectivities.Iboosy.Connector.HttpWrapper;
using Infrastructure.Connectivity.Contracts;
using Infrastructure.Connectivity.Connector;

namespace HubAcoApi.Extensions
{
    public static class IocInjection
    {
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
         
            //Application
            services.AddSingleton<IAvailabilityService, AvailabilityService>();
            services.AddSingleton<IValuationService, ValuationService>();
            services.AddSingleton<IBookingCreateService, BookingCreateService>();
            services.AddSingleton<IBookingsService, BookingsService>();
            services.AddSingleton<IBookingCancelService, BookingCancelService>();
            services.AddSingleton<IMappingService, MappingService>();
            services.AddSingleton<IAuthorizationService, AuthorizationService>();

            //Infrastucture.Connectivity
            services.AddSingleton<IConnector, Connector>();
            services.AddSingleton<IHttpWrapper, HttpWrapper>();

            //Crosscutting

            return services;
        }
    }
}
