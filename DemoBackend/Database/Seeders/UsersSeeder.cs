using DemoBackend.Database.Entities.Auth;
using DemoBackend.Database.Services.Seeding;
using Microsoft.EntityFrameworkCore;

namespace DemoBackend.Database.Seeders;

public class UsersSeeder : ISeeder
{
    public IReadOnlyCollection<Type> DependsOn => [typeof(RolesSeeder)];

    public void Add(DbContext context)
    {
        List<UserAccount> users = [];

        if (context.Set<UserAccount>()
                .FirstOrDefault(account => account.Email == "admin@syntrice.com") == null)
        {
            users.Add(new UserAccount()
            {
                Email = "admin@syntrice.com",
                PasswordHash =
                    "6AA761094DFFDC1A63C340CFCCFC64651E4976294F0ABE1427B30BD00A32F6E1-27B48FC46ABB52B7AA08E6A2A0BF40E0",
                HashSaltSize = 16,
                HashSize = 32,
                HashIterations = 100000,
                HashAlgorithm = "SHA512",
                UserProfile = new UserProfile
                {
                    DisplayName = "admin",
                },
                Roles = [context.Set<Role>().First(role => role.Name == "Admin")],
            });
        }

        context.AddRange(users);
    }
}