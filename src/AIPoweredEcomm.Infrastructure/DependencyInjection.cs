using AIPoweredEcomm.Infrastructure.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AIPoweredEcomm.Infrastructure;

public static class DependencyInjection
{
    /// <summary>
    /// Registers infrastructure services (persistence, external integrations)
    /// and binds infrastructure-owned options from configuration.
    /// </summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddOptions<JwtOptions>()
            .Bind(configuration.GetSection(JwtOptions.SectionName))
            .ValidateDataAnnotations();

        return services;
    }
}
