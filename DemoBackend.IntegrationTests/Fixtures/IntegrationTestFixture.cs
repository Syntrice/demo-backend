using DemoBackend.IntegrationTests.Utility;
using Testcontainers.PostgreSql;

namespace DemoBackend.IntegrationTests.Fixtures;

public class IntegrationTestFixture : IAsyncLifetime
{
    private TestApiFactory _factory = null!;
    public HttpClient Client => _factory.CreateClient();
    public IServiceProvider Services => _factory.Services;

    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("demobackend-postgres-test-container")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .WithCleanUp(true)
        .Build();

    public async Task InitializeAsync()
    {
        await _dbContainer.StartAsync();
        _factory = new TestApiFactory(_dbContainer.GetConnectionString());
    }

    public async Task DisposeAsync()
    {
        await _factory.DisposeAsync();
        await _dbContainer.DisposeAsync();
    }
}