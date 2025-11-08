using Microsoft.Extensions.DependencyInjection;
using SMSGateway.Managers.Interfaces;

namespace SMSGateway.Managers;

public static class DiConfig
{
    public static IServiceCollection SMSManager(this IServiceCollection services)
    {
        services.AddScoped<IStartingNumberProvider, StartingNumberProvider>();
        return services;
    }
}