// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects.JsonPath;
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a parsed array value in a JSON structure that acts as a collection of other parsed values.
/// </summary>
public interface IParsedArrayValue : IParsedValue
{
    public IReadOnlyList<IParsedValue> Values { get; }

    /// <summary>
    /// Gets 0 based index of <see cref="IParsedValue"/> with <see cref="IParsedValue.Id"/> equal to<br/>
    /// <param name="valueId"></param> in <see cref="Values"/>.<br/>
    /// Note, we can look up the index in <see cref="Values"/> without this method too.<br/>
    /// However, the implementation might choose to use internal mapping of values to indexes for quicker lookups.<br/>
    /// This might help when arrays are large.<br/>
    /// </summary>
    /// <param name="valueId">Value Id.</param>
    /// <param name="index">If the return value is true, index of <see cref="IParsedValue"/> with <see cref="IParsedValue.Id"/><br/>
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

/// <summary>
/// Represents a parsed array value within a JSON structure, inheriting behavior for manipulating and interacting
/// with collections of parsed values. This class is a concrete implementation built on top of the abstract
/// <see cref="ParsedArrayValueAbstr"/> type.
/// </summary>
/// <remarks>
/// The class serves as a concrete representation of an array parsed from JSON. It provides mechanisms for
/// accessing the root parsed value, managing the parent hierarchy, and associating optional key-value pairs.
/// Instances of this class are typically constructed using a visitor pattern for parsing operations.
/// </remarks>
public class ParsedArrayValue : ParsedArrayValueAbstr
{
    /// <summary>
    /// Represents a parsed array value derived from the JSON input, implemented as a class extending <see cref="ParsedArrayValueAbstr"/>.<br/>
    /// Encapsulates array elements within a JSON structure and ties them to a provided root parsed value and optional JSON key-value metadata.
    /// </summary>
    /// <remarks>
    /// This class facilitates operations that require parsing or manipulation of JSON arrays within the application's object model.<br/>
    /// It is constructed with references to a root parsed value, its visitor, parent parsed object, and any associated metadata (key-value pair).<br/>
    /// Instances of this class are used during JSON parsing and internal representation building.
    /// </remarks>
    public ParsedArrayValue(IParsedJsonVisitor parsedJsonVisitor, IRootParsedValue rootParsedValue, IParsedValue parentJsonValue, IJsonKeyValue? jsonKeyValue,
        IJsonPath? pathInReferencedJson) : 
        base(parsedJsonVisitor, parentJsonValue, jsonKeyValue, pathInReferencedJson)
    {
        RootParsedValue = rootParsedValue;
    }

    /// <inheritdoc />
    public override IRootParsedValue RootParsedValue { get; }
}