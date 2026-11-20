using AIPoweredEcomm.Application;
using AIPoweredEcomm.Infrastructure;

using Microsoft.Extensions.DependencyInjection;

namespace AIPoweredEcomm.UnitTests;

public class CompositionRootTests
{
    [Fact]
    public void AddApplicationAndAddInfrastructure_ChainTogetherAndBuildSuccessfully()
    {
        var services = new ServiceCollection();

        services.AddApplication().AddInfrastructure();

        using var provider = services.BuildServiceProvider();
    }
}
