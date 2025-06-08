using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Conversion error details.
/// </summary>
public interface IConversionError
{
    /// <summary>
    /// Error type.
    /// </summary>
    ConversionErrorType ErrorType { get; }

    /// <summary>
    /// If the value is not null, parsed JSON value path associated with error.
    /// </summary>
    IJsonPath? JsonPath { get; }

     /// <summary>
     /// If the value is not null, a JSON path that points out to the original JSON value.
     /// </summary>
     IJsonPath? PathInReferencedJson { get; }

    /// <summary>
    /// Error message.
    /// </summary>
    string Error { get; }

    /// <summary>
    /// If the value is not null, path describing the object path.
    /// Example: ["Employees, "[0]", "Address", "Street"]
    /// </summary>
    IReadOnlyList<string>? ObjectPath { get; }
}

/// <inheritdoc />
public class ConversionError : IConversionError
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="errorType">Error type.</param>
    /// <param name="error">Error message.</param>
    /// <param name="objectPath">
    /// If the value is not null, path describing the object path.
    /// Example: ["Employees, "[0]", "Address", "Street"]
    /// </param>
    /// <param name="jsonPath">If the value is not null, parsed JSON value path associated with error.</param>
    /// <param name="pathInReferencedJson">If the value is not null, a JSON path that points out to the original JSON value.</param>
    public ConversionError(ConversionErrorType errorType, string error, IReadOnlyList<string> objectPath, 
        IJsonPath? jsonPath, IJsonPath? pathInReferencedJson)
    {
        ErrorType = errorType;
        Error = error;
        ObjectPath = objectPath;
        JsonPath = jsonPath;
        PathInReferencedJson = pathInReferencedJson;
    }

    /// <inheritdoc />
    public ConversionErrorType ErrorType { get; }

    /// <inheritdoc />
    public IJsonPath? JsonPath { get; }

    /// <inheritdoc />
    public IJsonPath? PathInReferencedJson { get; }

    /// <inheritdoc />
    public string Error { get; }

    /// <inheritdoc />
    public IReadOnlyList<string>? ObjectPath { get; }
}