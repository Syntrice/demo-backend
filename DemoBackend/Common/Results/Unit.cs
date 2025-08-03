namespace DemoBackend.Common.Results;

/// <summary>
///     Unit represents a compositable placeholder / nill value
/// </summary>
public readonly struct Unit
{
    public static readonly Unit Value = new();
}