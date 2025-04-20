namespace JsonQL.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class DateTimeJsonComparable : IJsonComparable
{
    public DateTimeJsonComparable(DateTime value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.DateTime;

    /// <inheritdoc />
    public IComparable Value { get; }
}