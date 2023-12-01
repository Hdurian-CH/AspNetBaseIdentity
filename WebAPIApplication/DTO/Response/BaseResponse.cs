namespace DTO.Response;

public class BaseResponse(int code = 0, string message = "success")
{
    public int Code { get; set; } = code;

    public string Message { get; set; } = message;
}