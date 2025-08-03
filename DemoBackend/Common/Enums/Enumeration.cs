using System.Reflection;

namespace DemoBackend.Common.Enums;

public abstract class Enumeration<TEnum>(int value, string name) : IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    private static readonly Dictionary<int, TEnum> Enumerations = CreateEnumerations();

    public int Value { get; protected init; } = value;
    public string Name { get; protected init; } = name;

    public bool Equals(Enumeration<TEnum>? other)
    {
        if (other is null) return false;

        return GetType() == other.GetType() && Value == other.Value;
    }

    public static TEnum? FromValue(int value)
    {
        return Enumerations.GetValueOrDefault(value);
    }

    public static TEnum? FromName(string name)
    {
        return Enumerations.Values.SingleOrDefault(x => x.Name == name);
    }

    public static IReadOnlyCollection<TEnum> GetValues()
    {
        return Enumerations.Values;
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Value.GetHashCode();
    }

    private static Dictionary<int, TEnum> CreateEnumerations()
    {
        var enumerationType = typeof(TEnum);

        var fieldsForType = enumerationType
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fieldInfo => enumerationType.IsAssignableFrom(fieldInfo.FieldType))
            .Select(fieldInfo => (TEnum)fieldInfo.GetValue(null)!);
        return fieldsForType.ToDictionary(item => item.Value);
    }
}