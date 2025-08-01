namespace DemoBackend.Models.UserAccounts.Requests;

public class RevokeRefreshTokenFamilyRequest
{
    public Guid RefreshTokenFamilyId { get; set; } = Guid.Empty;
}