using DemoBackend.Common.Results;
using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.UserAccounts.Requests;
using DemoBackend.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DemoBackend.Services;

public interface IUserAccountService
{
    Task<Result<Unit>> RegisterAsync(RegisterRequest request);
}

public class UserAccountService(
    ApplicationDbContext db,
    IPasswordHashingService passwordHasher,
    IOptions<PasswordHashingSettings> hashingSettings)
    : IUserAccountService
{
    public async Task<Result<Unit>> RegisterAsync(RegisterRequest request)
    {
        if (await db.UserAccounts.FirstOrDefaultAsync(account => account.Email == request.Email) !=
            null)
        {
            return Error.Conflict("Email address already taken");
        }

        var hashedPassword = passwordHasher.Hash(request.Password, hashingSettings.Value);

        var account = new UserAccount()
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            HashSize = hashingSettings.Value.HashSize,
            HashSaltSize = hashingSettings.Value.SaltSize,
            HashIterations = hashingSettings.Value.Iterations,
            HashAlgorithm = hashingSettings.Value.Algorithm,
        };

        db.UserAccounts.Add(account);

        var profile = new UserProfile()
        {
            DisplayName = request.DisplayName,
            UserAccount = account,
        };

        db.UserProfiles.Add(profile);

        await db.SaveChangesAsync();

        return Unit.Value;
    }
}