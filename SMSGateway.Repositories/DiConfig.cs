using Microsoft.Extensions.DependencyInjection;
using SMSGateway.Repositories.Interfaces;

namespace SMSGateway.Repositories;

public static class DiConfig
{
    public static IServiceCollection SMSRepository(this IServiceCollection services)
    {
        services.AddScoped<ISMSMessageRepository,SMSMessageRepository>();
        services.AddScoped<ISMSSetupRepository,SMSSetupRepository>();
        return services;
    }
}