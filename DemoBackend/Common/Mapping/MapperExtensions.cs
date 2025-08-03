namespace DemoBackend.Common.Mapping;

public static class MapperExtensions
{
    public static IServiceCollection AddMappersFromAssemblyContaining<T>(
        this IServiceCollection services)
    {
        services.AddSingleton<IMapper>(_ =>
        {
            var mapper = new Mapper();
            var assembly = typeof(T).Assembly;
            var profiles = assembly.GetTypes()
                .Where(t => typeof(IMappingProfile).IsAssignableFrom(t) &&
                            t is { IsInterface: false, IsAbstract: false })
                .Select(Activator.CreateInstance)
                .Cast<IMappingProfile>();

            foreach (var profile in profiles)
                profile.Register(mapper);

            return mapper;
        });

        return services;
    }
}