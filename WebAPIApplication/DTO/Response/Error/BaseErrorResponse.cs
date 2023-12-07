namespace DTO.Response.Error;

public class BaseErrorResponse(string[] errorMessage)
{
    public Dictionary<string, string[]> errors { get; set; } =
        new()
        {
            { "message", errorMessage }
        };
}