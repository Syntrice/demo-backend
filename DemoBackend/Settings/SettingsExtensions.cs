namespace DemoBackend.Settings;

public static class SettingsExtensions
{
    public static WebApplicationBuilder BindSettings(this WebApplicationBuilder builder)
    {
        builder.Services
            .AddOptions<DatabaseSettings>()
            .Bind(builder.Configuration.GetSection(DatabaseSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<JWTSettings>()
            .Bind(builder.Configuration.GetSection(JWTSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        builder.Services
            .AddOptions<PasswordHashingSettings>()
            .Bind(builder.Configuration.GetSection(PasswordHashingSettings.SectionName))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        return builder;
    }
}