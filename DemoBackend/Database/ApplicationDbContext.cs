using DemoBackend.Database.Entities;
using DemoBackend.Database.Services;
using DemoBackend.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DemoBackend.Database;

public class ApplicationDbContext : DbContext
{
    private readonly IConfiguration _configuration;
    private readonly IOptions<DatabaseSettings> _settings;
    private readonly ILogger<ApplicationDbContext> _logger;
    private readonly IDatabaseSeedingService _seeder;

    public DbSet<Book> Books { get; set; }
    public DbSet<Author> Authors { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Auth> Auths { get; set; }
    public DbSet<RefreshTokenFamily> RefreshTokenFamilies { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IOptions<DatabaseSettings> databaseSettings, ILogger<ApplicationDbContext> logger, IConfiguration configuration,
        IOptions<DatabaseSettings> settings, IDatabaseSeedingService seeder) : base(options)
    {
        _logger = logger;
        _configuration = configuration;
        _settings = settings;
        _seeder = seeder;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseNpgsql(_settings.Value.ConnectionString);
        options.UseSeeding((context, _) => { _seeder.Seed(context); });
        options.UseAsyncSeeding(async (context, _, cancellationToken) => { await _seeder.SeedAsync(context); });
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}