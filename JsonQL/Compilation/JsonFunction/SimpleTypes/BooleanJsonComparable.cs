namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class BooleanJsonComparable : IJsonComparable
{
    /// <summary>
    /// A class representing a Boolean type that implements the IJsonComparable interface,
    /// providing compatibility for JSON comparison operations.
    /// </summary>
    public BooleanJsonComparable(bool value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.Boolean;

    /// <inheritdoc />
    public IComparable Value { get; }
}