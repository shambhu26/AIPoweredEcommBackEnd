using AIPoweredEcomm.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace AIPoweredEcomm.UnitTests;

public class InfrastructureDependencyInjectionTests
{
    [Fact]
    public void AddInfrastructure_ReturnsSameServiceCollectionInstance()
    {
        var services = new ServiceCollection();

        var result = services.AddInfrastructure();

        Assert.Same(services, result);
    }

    [Fact]
    public void AddInfrastructure_AllowsServiceProviderToBeBuilt()
    {
        var services = new ServiceCollection();

        services.AddInfrastructure();

        using var provider = services.BuildServiceProvider();
    }
}
