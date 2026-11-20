using System.ComponentModel.DataAnnotations;

namespace AIPoweredEcomm.Infrastructure.Options;

/// <summary>
/// JWT signing configuration bound from the "Jwt" configuration section.
/// <see cref="Key"/> must never be committed; supply it via user-secrets (Development)
/// or an environment variable / secret store (other environments).
/// </summary>
public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    [Required]
    public string Issuer { get; init; } = string.Empty;

    [Required]
    public string Audience { get; init; } = string.Empty;

    [Required]
    [MinLength(32)]
    public string Key { get; init; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ExpiryMinutes { get; init; } = 60;
}
