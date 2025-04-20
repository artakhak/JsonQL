namespace JsonQL.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class BooleanJsonComparable : IJsonComparable
{
    public BooleanJsonComparable(bool value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.Boolean;

    /// <inheritdoc />
    public IComparable Value { get; }
}