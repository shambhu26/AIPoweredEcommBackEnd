using AIPoweredEcomm.Application;

using Microsoft.Extensions.DependencyInjection;

namespace AIPoweredEcomm.UnitTests;

public class ApplicationDependencyInjectionTests
{
    [Fact]
    public void AddApplication_ReturnsSameServiceCollectionInstance()
    {
        var services = new ServiceCollection();

        var result = services.AddApplication();

        Assert.Same(services, result);
    }

    [Fact]
    public void AddApplication_AllowsServiceProviderToBeBuilt()
    {
        var services = new ServiceCollection();

        services.AddApplication();

        using var provider = services.BuildServiceProvider();
    }
}
