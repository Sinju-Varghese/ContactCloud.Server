namespace ContactCloud.Services.Dto;

public class UserResponseDto
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public TokenResponseDto TokenResponse { get; set; }
}
