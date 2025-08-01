namespace DemoBackend.Models.UserAccounts.Requests;

public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}