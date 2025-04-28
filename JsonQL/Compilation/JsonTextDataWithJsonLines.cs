namespace JsonQL.Compilation;

/// <summary>
/// Represents JSON text data that is split into individual lines for processing.
/// This class encapsulates JSON text data and provides functionality to split
/// the JSON content into individual lines. It acts as a wrapper around the
/// IJsonTextData to facilitate line-by-line error tracking or processing.
/// The JSON text lines are derived by splitting the original JSON text content
/// on the system-specific newline character.
/// This class is commonly used in scenarios where JSON compilation errors need
/// to be tracked or logged with reference to specific lines.
/// Thread Safety: This class is immutable and therefore thread-safe.
/// </summary>
public class JsonTextDataWithJsonLines
{
    /// <summary>
    /// Represents a wrapper for JSON text data that provides functionality
    /// to split the JSON content into individual lines.
    /// </summary>
    /// <remarks>
    /// This class is intended to work alongside <see cref="IJsonTextData"/>
    /// to split JSON text into lines for easier processing or troubleshooting.
    /// Each line of JSON is derived by splitting the original JSON text content
    /// based on the system-specific newline character sequence. This is particularly
    /// useful for scenarios requiring detailed tracking of JSON text by line,
    /// such as when logging or debugging JSON compilation results.
    /// This class is immutable and thread-safe.
    /// </remarks>
    public JsonTextDataWithJsonLines(IJsonTextData jsonTextData)
    {
        JsonTextData = jsonTextData;
        JsonLines = jsonTextData.JsonText.Split(Environment.NewLine);
    }

    /// <summary>
    /// Gets the JSON text data being processed.
    /// This property encapsulates an instance of <see cref="IJsonTextData"/> which represents
    /// the original JSON content. The data may be utilized directly or further processed
    /// as needed. It serves as the primary source text for the lines split and returned via
    /// the JsonLines property in the encapsulating class.
    /// </summary>
    public IJsonTextData JsonTextData { get; }

    /// <summary>
    /// Gets the collection of JSON text split into individual lines.
    /// This property provides a read-only list of strings, where each string
    /// corresponds to a line within the original JSON text. The lines are derived
    /// by splitting the JSON content based on the system-specific newline character.
    /// It is useful for scenarios such as line-by-line compilation error reporting
    /// or detailed processing of JSON data.
    /// </summary>
    public IReadOnlyList<string> JsonLines { get; }
}