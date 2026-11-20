using System.Text.Json;

namespace AIPoweredEcomm.UnitTests;

public class AppSettingsFilesTests
{
    [Theory]
    [InlineData("appsettings.json")]
    [InlineData("appsettings.Development.json")]
    [InlineData("appsettings.Staging.json")]
    [InlineData("appsettings.Production.json")]
    public void AppSettingsFile_IsValidJson(string fileName)
    {
        var path = Path.Combine(FindApiProjectDirectory(), fileName);

        Assert.True(File.Exists(path), $"Expected {path} to exist.");
        using var document = JsonDocument.Parse(File.ReadAllText(path));
    }

    [Fact]
    public void AppSettingsJson_DoesNotCommitJwtKey()
    {
        var path = Path.Combine(FindApiProjectDirectory(), "appsettings.json");
        using var document = JsonDocument.Parse(File.ReadAllText(path));

        var hasJwtKey = document.RootElement.TryGetProperty("Jwt", out var jwt)
            && jwt.TryGetProperty("Key", out _);

        Assert.False(hasJwtKey, "appsettings.json must not contain a committed Jwt:Key.");
    }

    private static string FindApiProjectDirectory()
    {
        var directory = new DirectoryInfo(AppContext.BaseDirectory);

        while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "AIPoweredEcomm.slnx")))
        {
            directory = directory.Parent;
        }

        if (directory is null)
        {
            throw new InvalidOperationException("Could not locate repository root (AIPoweredEcomm.slnx) from test output directory.");
        }

        return Path.Combine(directory.FullName, "src", "AIPoweredEcomm.Api");
    }
}
