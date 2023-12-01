using System.ComponentModel.DataAnnotations;

namespace WebAPIApplication.Services.Auth;

public static class JwtServiceOptions
{
    public const string JwtService = "JwtService";

    [Required] [Url] public static string ValidAudience { get; set; }

    [Required] [Url] public static string ValidIssuer { get; set; }

    [Required] [MinLength(16)] public static string Secret { get; set; }
}