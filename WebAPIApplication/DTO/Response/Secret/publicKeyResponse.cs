namespace DTO.Response.Secret;

public class PublicKeyResponse(string key) : BaseResponse
{
    public string PublicKey { get; set; } = key;

    public DateTime ReturnTime { get; set; } = DateTime.Now;
}