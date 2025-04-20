namespace JsonQL.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class DoubleJsonComparable : IJsonComparable
{
    public DoubleJsonComparable(double value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.Double;

    /// <inheritdoc />
    public IComparable Value { get; }
}