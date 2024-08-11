namespace GenocsAspire.Identities.Application.DTO;

/// <summary>
/// The AuthDto class.
/// </summary>
public class AuthDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string? Role { get; set; }
    public string? AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public long Expires { get; set; }
}