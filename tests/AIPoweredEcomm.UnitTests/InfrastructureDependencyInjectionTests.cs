using AIPoweredEcomm.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIPoweredEcomm.UnitTests;

public class InfrastructureDependencyInjectionTests
{
    private static IConfiguration EmptyConfiguration => new ConfigurationBuilder().Build();

    [Fact]
    public void AddInfrastructure_ReturnsSameServiceCollectionInstance()
    {
        var services = new ServiceCollection();

        var result = services.AddInfrastructure(EmptyConfiguration);

        Assert.Same(services, result);
    }

    [Fact]
    public void AddInfrastructure_AllowsServiceProviderToBeBuilt()
    {
        var services = new ServiceCollection();

        services.AddInfrastructure(EmptyConfiguration);

        using var provider = services.BuildServiceProvider();
    }
}
