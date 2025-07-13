using System.Security.Cryptography;
using DemoBackend.Settings;

namespace DemoBackend.Services;

public class PasswordHashingService : IPasswordHashingService
{
    public string Hash(string password, PasswordHashingSettings settings)
    {
        var salt = RandomNumberGenerator.GetBytes(settings.SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, settings.Iterations,
            new HashAlgorithmName(settings.Algorithm), settings.HashSize);
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    public bool Verify(string password, string hashedPassword, PasswordHashingSettings settings)
    {
        var parts = hashedPassword.Split("-");
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, settings.Iterations,
            new HashAlgorithmName(settings.Algorithm), settings.HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }
}

public interface IPasswordHashingService
{
    string Hash(string password, PasswordHashingSettings settings);
    bool Verify(string password, string hashedPassword, PasswordHashingSettings settings);
}