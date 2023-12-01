using Masuit.Tools.Security;

namespace WebAPIApplication.Services.Auth.Secret;

public static class RsaService
{
    public static string RsaEncrypt(string data)
    {
        return data.RSAEncrypt(RsaServiceOption.PublicKey);
    }

    public static string? RsaDecrypt(string data)
    {
        return data.RSADecrypt(RsaServiceOption.PrivateKey);
    }
}