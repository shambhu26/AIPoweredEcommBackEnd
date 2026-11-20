using AIPoweredEcomm.Infrastructure;
using AIPoweredEcomm.Infrastructure.Options;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace AIPoweredEcomm.UnitTests;

public class JwtOptionsTests
{
    private static readonly Dictionary<string, string?> ValidJwtConfig = new()
    {
        ["Jwt:Issuer"] = "AIPoweredEcomm",
        ["Jwt:Audience"] = "AIPoweredEcomm.Clients",
        ["Jwt:Key"] = new string('k', 32),
        ["Jwt:ExpiryMinutes"] = "60",
    };

    private static IOptions<JwtOptions> BuildOptions(Dictionary<string, string?> configValues)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues)
            .Build();

        var services = new ServiceCollection();
        services.AddInfrastructure(configuration);

        return services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>();
    }

    [Fact]
    public void JwtOptions_BindsFromConfiguration()
    {
        var options = BuildOptions(ValidJwtConfig);

        Assert.Equal("AIPoweredEcomm", options.Value.Issuer);
        Assert.Equal("AIPoweredEcomm.Clients", options.Value.Audience);
        Assert.Equal(32, options.Value.Key.Length);
        Assert.Equal(60, options.Value.ExpiryMinutes);
    }

    [Fact]
    public void JwtOptions_MissingKey_ThrowsOnResolve()
    {
        var configValues = new Dictionary<string, string?>(ValidJwtConfig);
        configValues.Remove("Jwt:Key");

        var options = BuildOptions(configValues);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void JwtOptions_KeyShorterThanMinimumLength_ThrowsOnResolve()
    {
        var configValues = new Dictionary<string, string?>(ValidJwtConfig)
        {
            ["Jwt:Key"] = "too-short",
        };

        var options = BuildOptions(configValues);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }

    [Fact]
    public void JwtOptions_NonPositiveExpiryMinutes_ThrowsOnResolve()
    {
        var configValues = new Dictionary<string, string?>(ValidJwtConfig)
        {
            ["Jwt:ExpiryMinutes"] = "0",
        };

        var options = BuildOptions(configValues);

        Assert.Throws<OptionsValidationException>(() => options.Value);
    }
}
