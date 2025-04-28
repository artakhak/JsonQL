namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents an exception that occurs specifically during JSON-to-object conversion.
/// </summary>
/// <remarks>
/// This exception is commonly thrown when the conversion process encounters an invalid,
/// incorrect, or incompatible JSON structure or when a conversion to a specific type fails.
/// </remarks>
public class JsonConversionException : ApplicationException
{
    /// <summary>
    /// Represents an exception that occurs during JSON-to-object conversion.
    /// </summary>
    /// <remarks>
    /// This exception is typically thrown when the JSON-to-object conversion process fails.
    /// Common reasons for failure include:
    /// - Invalid or improperly structured JSON for the target type.
    /// - Incompatibility between JSON data and the expected object type.
    /// - Errors encountered during type conversion.
    /// </remarks>
    public JsonConversionException(string message): base(message)
    {
    }

    /// <summary>
    /// Represents an exception thrown during the process of converting JSON to an object.
    /// </summary>
    /// <remarks>
    /// This exception is raised when the JSON-to-object conversion fails due to various reasons, such as:
    /// - Invalid JSON or mismatched data structures.
    /// - The inability to convert JSON data to the specified .NET types.
    /// - Failures in the underlying serializer or deserializer.
    /// It is commonly utilized in scenarios where detailed context-specific error handling for JSON conversion is required.
    /// </remarks>
    public JsonConversionException()
    {
    }
}