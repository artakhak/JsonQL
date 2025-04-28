namespace JsonQL;

/// <summary>
/// Represents the result of a parsing operation, including the parsed value
/// and any errors encountered during parsing.
/// This interface is designed to encapsulate the output of parsing operations
/// where a value may or may not be successfully parsed.
/// </summary>
public interface IParseResult<out TValue>
{
    /// <summary>
    /// Gets the parsed value resulting from an operation.
    /// This value may be null if parsing fails or if the operation
    /// does not result in a valid output. Ensure to check for associated
    /// errors and nullability before using this property.
    /// </summary>
    TValue? Value { get; }

    /// <summary>
    /// Gets the collection of errors encountered during the parsing operation.
    /// This collection may contain detailed information about issues such as invalid
    /// syntax, missing required properties, or type mismatches during the parsing process.
    /// It is recommended to check for the presence of errors before utilizing the parsed value.
    /// </summary>
    IReadOnlyList<IJsonObjectParseError> Errors { get; }
}

/// <inheritdoc />
public class ParseResult<TValue> : IParseResult<TValue>
{
    /// <summary>
    /// Represents the result of a parsing operation. Encapsulates the parsed value or a collection of errors encountered during parsing.
    /// </summary>
    /// <typeparam name="TValue">The type of the parsed value.</typeparam>
    public ParseResult(TValue value)
    {
        Value = value;
    }
    
    /// <summary>
    /// Represents the result of a parsing operation. This class encapsulates either a parsed value or a collection of errors.
    /// </summary>
    /// <typeparam name="TValue">The type of the parsed value.</typeparam>
    public ParseResult(IReadOnlyList<IJsonObjectParseError> errors)
    {
        Errors = errors;
    }

    /// <inheritdoc />
    public TValue? Value { get; }

    /// <inheritdoc />
    public IReadOnlyList<IJsonObjectParseError> Errors { get; } = Array.Empty<IJsonObjectParseError>();
}