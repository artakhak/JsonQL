namespace JsonQL.JsonToObjectConversion;

/// <summary>
/// Represents a JSON property naming convention where the first letter of each word, except the first word, is capitalized.
/// </summary>
public enum JsonPropertyFormat
{
    /// <summary>
    /// Specifies a JSON property naming convention where words are concatenated without spaces,
    /// and each word following the first starts with an uppercase letter.
    /// </summary>
    CamelCase,

    /// <summary>
    /// Specifies a JSON property naming convention where the first letter of each word,
    /// including the first word, is capitalized.
    /// </summary>
    PascalCase
}