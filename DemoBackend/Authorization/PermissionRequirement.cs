using Microsoft.AspNetCore.Authorization;

namespace DemoBackend.Authorization;

public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    public string Permission { get; } = permission;
}