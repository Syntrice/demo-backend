using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;

namespace DemoBackend.IntegrationTests.Utility;

public class TestApiFactory(string connString) : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((ctx, cfg) =>
        {
            var mem = new Dictionary<string, string?>
            {
                ["DatabaseSettings:ConnectionString"] = connString
            };
            cfg.AddInMemoryCollection(mem);
        });
    }
}