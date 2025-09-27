namespace JsonQL.JsonObjects;

/// <summary>
/// Provides extension methods for the <see cref="IParsedArrayValue"/> interface, simplifying operations such as adding parsed values to a JSON array structure.
/// </summary>
public static class ParsedArrayValueExtensions
{
    /// <summary>
    /// Adds a parsed value to the end of the <see cref="IParsedArrayValue.Values"/> collection.
    /// This method simplifies appending values to the underlying JSON array structure represented
    /// by an <see cref="IParsedArrayValue"/>.
    /// </summary>
    /// <param name="parsedArrayValue">The <see cref="IParsedArrayValue"/> instance to which the parsed value will be added.</param>
    /// <param name="parsedValue">The <see cref="IParsedValue"/> to add to the <see cref="IParsedArrayValue.Values"/> collection.</param>
    /// <remarks>The method utilizes <see cref="IParsedArrayValue.AddValueAt(int, IParsedValue)"/> to append the value
    /// at the end of the collection, by passing the current count of the collection as the index.</remarks>
    public static void AddValue(this IParsedArrayValue parsedArrayValue, IParsedValue parsedValue)
    {
        parsedArrayValue.AddValueAt(parsedArrayValue.Values.Count, parsedValue);
    }
}