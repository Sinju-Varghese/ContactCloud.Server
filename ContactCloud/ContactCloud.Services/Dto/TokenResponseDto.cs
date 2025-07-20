namespace ContactCloud.Services.Dto;

public class TokenResponseDto
{
    public string Token { get; set; }
    public DateTime Expires { get; set; }

    public static implicit operator string(TokenResponseDto v)
    {
        return v?.Token ?? string.Empty;
    }
}
