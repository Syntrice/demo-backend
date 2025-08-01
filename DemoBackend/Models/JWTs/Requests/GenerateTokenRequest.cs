namespace DemoBackend.Models.JWTs.Requests;

public class GenerateTokenRequest
{
    public string Email { get; set; } = string.Empty;

    public string DisplayName { get; set; } = string.Empty;

    public string Id { get; set; } = string.Empty;

    public string RefreshTokenFamilyId { get; set; } = string.Empty;
}