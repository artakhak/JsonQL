using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

public interface IParsedArrayValue : IParsedValue
{
    public IReadOnlyList<IParsedValue> Values { get; }

    /// <summary>
    /// Gets 0 based index of <see cref="IParsedValue"/> with <see cref="IParsedValue.Id"/> equal to<br/>
    /// <param name="valueId"></param> in <see cref="Values"/>.<br/>
    /// Note, we can look-up the index in <see cref="Values"/> without this method too.<br/>
    /// However, the implementation might choose to use internal mapping of values to indexes for quicker lookups.<br/>
    /// This might help when arrays are large.<br/>
    /// </summary>
    /// <param name="valueId">Value Id.</param>
    /// <param name="index">If the returns value is true, index of <see cref="IParsedValue"/> with <see cref="IParsedValue.Id"/><br/>
    /// equal to <param name="valueId"></param> in <see cref="Values"/>.<br/>
    /// Otherwise, the value is null.
    /// </param>
    /// <remarks>The method is not thread safe. The caller is responsible for providing synchronized access to this method.</remarks>
    bool TryGetValueIndex(Guid valueId, [NotNullWhen(true)] out int? index);

    /// <summary>
    /// Adds a value to <see cref="Values"/> at index <param name="index"></param>.
    /// </summary>
    /// <param name="index">Index at which value is added.</param>
    /// <param name="parsedValue">Value to add.</param>
    /// <exception cref="IndexOutOfRangeException">Throws this exception.</exception>
    /// <remarks>The method is not thread safe. The caller is responsible for providing synchronized access to this method.</remarks>
    void AddValueAt(int index, IParsedValue parsedValue);

    /// <summary>
    /// Removes a value from <see cref="Values"/>  at index <param name="index"></param>.
    /// </summary>
    /// <param name="index">Index at which value is removed.</param>
    /// <exception cref="IndexOutOfRangeException">Throws this exception.</exception>
    /// <remarks>The method is not thread safe. The caller is responsible for providing synchronized access to this method.</remarks>
    void RemoveValueAt(int index);
}

public static class ParsedArrayValueExtensions
{
    public static void AddValue(this IParsedArrayValue parsedArrayValue, IParsedValue parsedValue)
    {
        parsedArrayValue.AddValueAt(parsedArrayValue.Values.Count, parsedValue);
    }
}

public class ParsedArrayValue : ParsedArrayValueAbstr
{
    public ParsedArrayValue(IParsedJsonVisitor parsedJsonVisitor, IRootParsedValue rootParsedValue, IParsedValue parentJsonValue, IJsonKeyValue? jsonKeyValue) : 
        base(parsedJsonVisitor, parentJsonValue, jsonKeyValue)
    {
        RootParsedValue = rootParsedValue;
    }

    /// <inheritdoc />
    public override IRootParsedValue RootParsedValue { get; }
}