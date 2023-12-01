using Masuit.Tools.Security;

namespace WebAPIApplication.Services.Auth.Secret;

public static class RsaServiceOption
{
    public const string RsaService = "RsaService";
    private static readonly RsaKey RsaKey = RsaCrypt.GenerateRsaKeys();

    public static string PublicKey { get; set; } = RsaKey.PublicKey;

    public static string PrivateKey { get; set; } = RsaKey.PrivateKey;
}