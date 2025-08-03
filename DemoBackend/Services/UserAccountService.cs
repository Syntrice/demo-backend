using DemoBackend.Common.Results;
using DemoBackend.Database;
using DemoBackend.Database.Entities;
using DemoBackend.Models.JWTs.Requests;
using DemoBackend.Models.UserAccounts.Requests;
using DemoBackend.Models.UserAccounts.Responses;
using DemoBackend.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DemoBackend.Services;

public interface IUserAccountService
{
    Task<Result<Unit>> RegisterAsync(RegisterRequest request);
    Task<Result<LoginResponse>> LoginAsync(LoginRequest request);
    Task<Result<RefreshResponse>> RefreshAsync(RefreshRequest model);
    Task<Result<Unit>> RevokeRefreshTokenFamilyAsync(RevokeRefreshTokenFamilyRequest request);
}

public class UserAccountService(
    ApplicationDbContext db,
    IPasswordHashingService passwordHasher,
    IJWTService jwtService,
    IOptions<PasswordHashingSettings> hashingSettings,
    IOptions<JWTSettings> jwtSettings)
    : IUserAccountService
{
    public async Task<Result<Unit>> RegisterAsync(RegisterRequest request)
    {
        // TODO: Fix race condition risk
        if (await db.UserAccounts.FirstOrDefaultAsync(account => account.Email == request.Email) !=
            null)
            return Error.Conflict("Email address already taken");

        var hashedPassword = passwordHasher.Hash(request.Password, hashingSettings.Value);

        var account = new UserAccount
        {
            Email = request.Email,
            PasswordHash = hashedPassword,
            HashSize = hashingSettings.Value.HashSize,
            HashSaltSize = hashingSettings.Value.SaltSize,
            HashIterations = hashingSettings.Value.Iterations,
            HashAlgorithm = hashingSettings.Value.Algorithm
        };

        db.UserAccounts.Add(account);

        var profile = new UserProfile
        {
            DisplayName = request.DisplayName,
            UserAccount = account
        };

        db.UserProfiles.Add(profile);

        await db.SaveChangesAsync();

        return Unit.Value;
    }

    async Task<Result<LoginResponse>> IUserAccountService.LoginAsync(LoginRequest request)
    {
        var user =
            await db.UserAccounts.Include(account => account.UserProfile)
                .FirstOrDefaultAsync(account => account.Email == request.Email);

        if (user == null) return Error.Forbidden("Incorrect email or password");

        var hasherSettings = new PasswordHashingSettings
        {
            SaltSize = user.HashSaltSize,
            HashSize = user.HashSize,
            Iterations = user.HashIterations,
            Algorithm = user.HashAlgorithm
        };

        if (!passwordHasher.Verify(request.Password, user.PasswordHash, hasherSettings))
            return Error.Forbidden("Incorrect email or password");

        // Create a new refresh token family
        var tokenFamily = new RefreshTokenFamily
        {
            CreatedAt = DateTime.UtcNow,
            ExpiresAt =
                DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenFamilyExpirationInDays)
        };

        // Create a refresh token
        var refreshToken = new RefreshToken
        {
            Hash = jwtService.GenerateRefreshTokenHash(),
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationInDays)
        };

        // Save to database
        tokenFamily.RefreshTokens.Add(refreshToken);
        user.RefreshTokenFamilies.Add(tokenFamily);
        await db.SaveChangesAsync();

        // Create an access token
        var accessToken = jwtService.GenerateToken(new GenerateTokenRequest
        {
            Email = user.Email,
            DisplayName = user.UserProfile.DisplayName,
            Id = user.Id.ToString(),
            RefreshTokenFamilyId = tokenFamily.Id.ToString()
        });

        return new LoginResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Hash
        };
    }

    public async Task<Result<RefreshResponse>> RefreshAsync(RefreshRequest request)
    {
        // Check if the refresh token is valid
        var refreshToken = await db.RefreshTokens.Include(e => e.Family)
            .ThenInclude(e => e.UserAccount)
            .ThenInclude(e => e.UserProfile)
            .FirstOrDefaultAsync(e => e.Hash == request.RefreshToken);

        if (refreshToken == null || refreshToken.ExpiresAt < DateTime.UtcNow ||
            refreshToken.Family.ExpiresAt < DateTime.UtcNow)
            return Error.Forbidden("Refresh token invalid or expired");

        // Reuse detection
        if (refreshToken.NextId != null)
        {
            db.RefreshTokenFamilies.Remove(refreshToken.Family);
            await db.SaveChangesAsync();
            return Error.Forbidden("Refresh token already used");
        }

        // Create a refresh token
        var newRefreshToken = new RefreshToken
        {
            Hash = jwtService.GenerateRefreshTokenHash(),
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(jwtSettings.Value.RefreshTokenExpirationInDays),
            Previous = refreshToken,
            PreviousId = refreshToken.Id
        };

        // Save to database
        refreshToken.Family.RefreshTokens.Add(newRefreshToken);
        refreshToken.Next = newRefreshToken;
        await db.SaveChangesAsync();

        // Create an access token
        var accessToken = jwtService.GenerateToken(new GenerateTokenRequest
        {
            Email = refreshToken.Family.UserAccount.Email,
            DisplayName = refreshToken.Family.UserAccount.UserProfile.DisplayName,
            Id = refreshToken.Family.UserAccount.Id.ToString(),
            RefreshTokenFamilyId = refreshToken.Family.Id.ToString()
        });

        return new RefreshResponse
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken.Hash
        };
    }

    public async Task<Result<Unit>> RevokeRefreshTokenFamilyAsync(
        RevokeRefreshTokenFamilyRequest request)
    {
        var family = await db.RefreshTokenFamilies.FindAsync(request.RefreshTokenFamilyId);

        if (family == null)
            return Error.Forbidden("Unable to sign out. Perhaps you are already logged out.");

        db.RefreshTokenFamilies.Remove(family);
        await db.SaveChangesAsync();
        return Unit.Value;
    }
}