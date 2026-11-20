using AIPoweredEcomm.Application;
using AIPoweredEcomm.Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIPoweredEcomm.UnitTests;

public class CompositionRootTests
{
    [Fact]
    public void AddApplicationAndAddInfrastructure_ChainTogetherAndBuildSuccessfully()
    {
        var services = new ServiceCollection();
        var configuration = new ConfigurationBuilder().Build();

        services.AddApplication().AddInfrastructure(configuration);

        using var provider = services.BuildServiceProvider();
    }
}
