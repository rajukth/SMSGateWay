using SMSGateway.Base.DataContext;
using SMSGateway.Base.DataContext.Interface;
using SMSGateway.Base.Providers.IProviders;
using Microsoft.Extensions.DependencyInjection;
using SMSGateway.Base.Providers;

namespace SMSGateway.Base;

public static class DiConfig
{
    public static IServiceCollection BaseConfig(this IServiceCollection services)
    {
        services.AddSingleton<IConnectionStringProviders, ConnectionStringProviders>();
        services.AddScoped<IUow, Uow>();
        return services;
    }
}