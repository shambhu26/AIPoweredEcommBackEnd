using Microsoft.Extensions.DependencyInjection;

namespace AIPoweredEcomm.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Registers application-layer services (use cases, validators, mappers).
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
