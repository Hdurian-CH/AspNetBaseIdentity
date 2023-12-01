namespace DTO.Response.Error;

public class BaseErrorResponse(string[] errorMessage)
{
    public Dictionary<string, string[]> error { get; set; } =
        new()
        {
            { "message", errorMessage }
        };
}