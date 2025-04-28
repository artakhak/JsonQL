namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class DoubleJsonComparable : IJsonComparable
{
    /// <summary>
    /// Represents a comparable value of type double within JSON context, allowing comparisons
    /// and type code identification for double values.
    /// </summary>
    public DoubleJsonComparable(double value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.Double;

    /// <inheritdoc />
    public IComparable Value { get; }
}