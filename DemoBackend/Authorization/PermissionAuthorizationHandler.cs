using DemoBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.JsonWebTokens;

namespace DemoBackend.Authorization;

public class PermissionAuthorizationHandler(IServiceScopeFactory scopeFactory)
    : AuthorizationHandler<PermissionRequirement>
{
    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        string? userId = context.User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

        if (!Guid.TryParse(userId, out var parsedGuid))
        {
            return;
        }

        using IServiceScope serviceScope = scopeFactory.CreateScope();

        IPermissionService permissionService =
            serviceScope.ServiceProvider.GetRequiredService<IPermissionService>();

        // TOOO: Cache this to avoid hitting the database each time
        var result = await permissionService.GetPermissionNamesAsync(parsedGuid);

        if (result.IsFailure)
        {
            return;
        }

        if (result.Value.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}