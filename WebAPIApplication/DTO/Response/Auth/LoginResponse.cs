namespace DTO.Response.Auth;

public class LoginResponse(string message = "success", int code = 0, string token = "") : BaseResponse(code, message)
{
    public string Token { get; set; } = token;
}