using System.Reflection;

using NetArchTest.Rules;

namespace AIPoweredEcomm.ArchitectureTests;

/// <summary>
/// Enforces the dependency direction Api -> Application -> Domain,
/// with Infrastructure depending only on Application and Domain.
/// </summary>
public class LayerDependencyTests
{
    private const string ApiNamespace = "AIPoweredEcomm.Api";
    private const string ApplicationNamespace = "AIPoweredEcomm.Application";
    private const string DomainNamespace = "AIPoweredEcomm.Domain";
    private const string InfrastructureNamespace = "AIPoweredEcomm.Infrastructure";

    private static readonly Assembly ApiAssembly = typeof(Program).Assembly;
    private static readonly Assembly ApplicationAssembly = typeof(Application.DependencyInjection).Assembly;
    private static readonly Assembly DomainAssembly = typeof(Domain.Common.BaseEntity).Assembly;
    private static readonly Assembly InfrastructureAssembly = typeof(Infrastructure.DependencyInjection).Assembly;

    [Fact]
    public void Domain_ShouldNotDependOnAnyOtherLayer()
    {
        var result = Types.InAssembly(DomainAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(ApplicationNamespace, InfrastructureNamespace, ApiNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Application_ShouldNotDependOnInfrastructureOrApi()
    {
        var result = Types.InAssembly(ApplicationAssembly)
            .ShouldNot()
            .HaveDependencyOnAny(InfrastructureNamespace, ApiNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Infrastructure_ShouldNotDependOnApi()
    {
        var result = Types.InAssembly(InfrastructureAssembly)
            .ShouldNot()
            .HaveDependencyOn(ApiNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Api_ShouldNotDependOnInfrastructure_ExceptCompositionRoot()
    {
        var result = Types.InAssembly(ApiAssembly)
            .That()
            .DoNotHaveName(nameof(Program))
            .ShouldNot()
            .HaveDependencyOn(InfrastructureNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    [Fact]
    public void Api_ShouldNotExposeDomainTypes()
    {
        var result = Types.InAssembly(ApiAssembly)
            .ShouldNot()
            .HaveDependencyOn(DomainNamespace)
            .GetResult();

        AssertSuccessful(result);
    }

    private static void AssertSuccessful(TestResult result)
    {
        var failingTypes = result.FailingTypeNames ?? [];
        Assert.True(result.IsSuccessful, $"Layering violated by: {string.Join(", ", failingTypes)}");
    }
}
