namespace JsonQL.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class StringJsonComparable : IJsonComparable
{
    public StringJsonComparable(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.String;

    /// <inheritdoc />
    public IComparable Value { get; }
}