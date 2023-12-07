namespace DTO.Response.Auth;

public class CheckTokenResponse(string token): BaseResponse
{
    public string Token { get; set; } = token;
}