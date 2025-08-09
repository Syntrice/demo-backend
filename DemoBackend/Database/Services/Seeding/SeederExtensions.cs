namespace DemoBackend.Database.Services.Seeding;

public static class SeederExtensions
{
    public static IServiceCollection AddSeedersFromAssemblyContaining<T>(
        this IServiceCollection services)
    {
        var assembly = typeof(T).Assembly;
        var seederTypes = assembly.GetTypes().Where(t =>
            typeof(ISeeder).IsAssignableFrom(t) && t is { IsInterface: false, IsAbstract: false });

        foreach (var seederType in seederTypes)
        {
            services.AddTransient(typeof(ISeeder), seederType);
        }

        return services;
    }
}