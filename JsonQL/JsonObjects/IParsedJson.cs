// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;
using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a parsed JSON object providing access to key-value pairs and related functionalities.
/// </summary>
public interface IParsedJson : IParsedValue
{
    IReadOnlyList<IJsonKeyValue> KeyValues { get; }

    /// <summary>
    /// Returns true if json object has key <param name="key"></param>
    /// </summary>
    /// <param name="key">Key to check.</param>
    bool HasKey(string key);

    /// <summary>
    /// Gets or sets a key value.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="index">
    /// If the value is not null, and the value of this parameter is between 0, and number of items in <see cref="KeyValues"/>,<br/>
    /// and key <param name="key"></param> is not in this json object, the value will be inserted at position <param name="index"></param>.<br/>
    /// Otherwise, the value is ignored.<br/>
    /// NOTE: IN most cases, no value should be provided for <param name="index"></param>. The only reason this
    /// parameter was added was to preserve the structure of json, when doing transformations.
    /// </param>
    /// <remarks>The method is not thread safe. The caller is responsible for providing synchronized access to this method.</remarks>
    /// <exception cref="Exception">Throws this exception.</exception>
    IJsonKeyValue this[string key, int? index = null] { get; set; }

    /// <summary>
    /// Tries to get the value with key <param name="key"></param>.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="jsonKeyValue">Retrieved key value, if key exists.</param>
    /// <returns>If key exists, returns true, and the value of <param name="jsonKeyValue"></param> is set to the value of retrieved value.</returns>
    /// <remarks>The method is not thread safe. The caller is responsible for providing synchronized access to this method.</remarks>
    bool TryGetJsonKeyValue(string key, [NotNullWhen(true)] out IJsonKeyValue? jsonKeyValue);

    /// <summary>
    /// Tries to remove a value with key <param name="key"></param>.
    /// </summary>
    /// <param name="key">Key.</param>
    /// <param name="jsonKeyValue">Retrieved key value, if key exists.</param>
    /// <returns>If key exists, removes the key, returns true, and the value of <param name="jsonKeyValue"></param> is set to the value of removed key value.</returns>
    /// <remarks>The method is not thread safe. The caller is responsible for providing synchronized access to this method.</remarks>
    bool TryRemoveKey(string key, [NotNullWhen(true)] out IJsonKeyValue? jsonKeyValue);
}

/// <summary>
/// Represents a concrete implementation of a parsed JSON object, inheriting from <see cref="ParsedJsonAbstr"/>.
/// Provides access to key-value pairs, parent JSON value, and root parsed value in the JSON hierarchy.
/// </summary>
public class ParsedJson : ParsedJsonAbstr
{
    /// <summary>
    /// Represents a concrete implementation of a parsed JSON object.
    /// Provides access to its associated root parsed value, its parent JSON value, and the associated key-value pair.
    /// </summary>
    /// <remarks>
    /// This class is a specialized implementation of <see cref="ParsedJsonAbstr"/>, leveraging an <see cref="IParsedJsonVisitor"/>
    /// to traverse or process JSON values.
    /// </remarks>
    /// <param name="parsedJsonVisitor">The visitor used to process the parsed JSON structure.</param>
    /// <param name="rootParsedValue">The root parsed value in the JSON hierarchy.</param>
    /// <param name="parentJsonValue">The parent JSON value of this object in the hierarchy, or null for root objects.</param>
    /// <param name="jsonKeyValue">The key-value pair associated with this JSON object, or null if none.</param>
    /// <param name="pathInReferencedJson">
    /// If the value is not null, a json path that points out to original json value.<br/>
    /// For example, consider the following two json files "Employees.json" that has a JSON array value at JSON key "Employees" (e.g., {"Employees": [...]}<br/>
    /// Also, lets assume we have another JSON "EmployeeExpressions.json" shown below that has an expression that references the second employee in "Employees.json".<br/>
    /// {"SecondEmployee": "$value(Employees[1])"}.<br/>
    /// In this example the value returned by method call <see cref="IParsedValue.GetPath"/> for JSON value "Employees[1]" will be "Root.SecondEmployee" since it is a json value in<br/>
    /// "EmployeeExpressions.json" mapped to key "Root.SecondEmployee". However, the value of <see cref="IParsedValue.PathInReferencedJson"/> will be<br/>
    /// "Root.Employees[1]" and <see cref="IParsedValue.PathInReferencedJson"/>.<see cref="IJsonPath.JsonTextIdentifier"/> will be "Employees.json".
    /// </param>
    public ParsedJson(IParsedJsonVisitor parsedJsonVisitor, IRootParsedValue rootParsedValue, IParsedValue? parentJsonValue, IJsonKeyValue? jsonKeyValue,
        IJsonPath? pathInReferencedJson) : 
        base(parsedJsonVisitor, parentJsonValue, jsonKeyValue, pathInReferencedJson)
    {
        RootParsedValue = rootParsedValue;
    }
    
    /// <inheritdoc />
    public override IRootParsedValue RootParsedValue { get; }
}