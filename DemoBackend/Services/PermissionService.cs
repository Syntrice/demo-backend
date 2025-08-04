using DemoBackend.Common.Results;
using DemoBackend.Database;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Services;

public interface IPermissionService
{
    Task<Result<HashSet<string>>> GetPermissionNamesAsync(Guid userId);
}

public class PermissionService(ApplicationDbContext db) : IPermissionService
{
    public async Task<Result<HashSet<string>>> GetPermissionNamesAsync(Guid userId)
    {
        var user = await db.UserAccounts.Include(e => e.Roles)
            .FirstOrDefaultAsync(e => e.Id == userId);
        if (user == null)
            return Error.Forbidden("You do not have permission to perform this operation");
        return user.Roles.SelectMany(e => e.Permissions).Select(p => p.ToString()).ToHashSet();
    }
}