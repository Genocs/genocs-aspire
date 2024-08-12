namespace GenocsAspire.Multitenancy.Application.Identity.Tokens;

public record RefreshTokenRequest(string Token, string RefreshToken);