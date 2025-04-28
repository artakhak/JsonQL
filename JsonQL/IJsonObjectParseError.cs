using JsonQL.JsonObjects;

namespace JsonQL;

/// <summary>
/// Represents an interface for describing an error that occurs while parsing a JSON object.
/// </summary>
public interface IJsonObjectParseError
{
    /// <summary>
    /// Gets the line information associated with the JSON parsing error,
    /// if available. Provides details such as the line number and position
    /// where the error occurred in the JSON document. Can be null if line
    /// information is not provided.
    /// </summary>
    IJsonLineInfo? LineInfo { get; }

    /// <summary>
    /// Gets the error message associated with a JSON parsing or processing error.
    /// Provides a descriptive string that explains the error encountered during
    /// the operation, aiding in debugging and error diagnostics.
    /// </summary>
    string ErrorMessage { get; }
}

/// <inheritdoc />
public class JsonObjectParseError : IJsonObjectParseError
{
    public JsonObjectParseError(string errorMessage, IJsonLineInfo? lineInfo)
    {
        ErrorMessage = errorMessage;
        LineInfo = lineInfo;
    }

    /// <inheritdoc />
    public string ErrorMessage { get; }
    
    /// <inheritdoc />
    public IJsonLineInfo? LineInfo { get; }
}