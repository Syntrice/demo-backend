using System.ComponentModel.DataAnnotations;

namespace DemoBackend.Settings;

public class DatabaseSettings
{
    [Required] public string ConnectionString { get; init; } = null!;
}