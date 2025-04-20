namespace JsonQL;

public interface IParseResult<out TValue>
{
    TValue? Value { get; }
    IReadOnlyList<IJsonObjectParseError> Errors { get; }
}

/// <inheritdoc />
public class ParseResult<TValue> : IParseResult<TValue>
{
    public ParseResult(TValue value)
    {
        Value = value;
    }

    public ParseResult(IReadOnlyList<IJsonObjectParseError> errors)
    {
        Errors = errors;
    }

    /// <inheritdoc />
    public TValue? Value { get; }

    /// <inheritdoc />
    public IReadOnlyList<IJsonObjectParseError> Errors { get; } = Array.Empty<IJsonObjectParseError>();
}