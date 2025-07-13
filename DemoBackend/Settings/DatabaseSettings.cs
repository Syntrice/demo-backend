using System.ComponentModel.DataAnnotations;

namespace DemoBackend.Settings;

public class DatabaseSettings
{
    public const string SectionName = "DatabaseSettings";

    [Required] public string ConnectionString { get; init; } = null!;
}