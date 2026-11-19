using Microsoft.Extensions.DependencyInjection;

namespace AIPoweredEcomm.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services (persistence, external integrations).
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        return services;
    }
}
