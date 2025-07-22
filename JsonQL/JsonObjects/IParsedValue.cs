// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonObjects;

/// <summary>
/// Represents a parsed value from a JSON object.
/// Provides access to associated metadata such as the value's unique identifier,
/// its path within the JSON structure, and related parent and root objects.
/// </summary>
public interface IParsedValue
{
    /// <summary>
    /// Value path. To avoid the path getting out of sync when the json is modified, the value
    /// might be dynamically calculated from parent. Therefore, this method should not be called frequently. 
    /// </summary>
    /// <returns>Returns value path.</returns>
    IJsonPath GetPath();

    /// <summary>
    /// If the value is not null, a json path that points out to original json value.<br/>
    /// For example, consider the following two json files "Employees.json" that has a JSON array value at JSON key "Employees" (e.g., {"Employees": [...]}<br/>
    /// Also, lets assume we have another JSON "EmployeeExpressions.json" shown below that has an expression that references the second employee in "Employees.json".<br/>
    /// {"SecondEmployee": "$value(Employees[1])"}.<br/>
    /// In this example the value returned by method call <see cref="IParsedValue.GetPath"/> for JSON value "Employees[1]" will be "Root.SecondEmployee" since it is a json value in<br/>
    /// "EmployeeExpressions.json" mapped to key "Root.SecondEmployee". However, the value of <see cref="PathInReferencedJson"/> will be<br/>
    /// "Root.Employees[1]" and <see cref="PathInReferencedJson"/>.<see cref="IJsonPath.JsonTextIdentifier"/> will be "Employees.json".
    /// </summary>
    IJsonPath? PathInReferencedJson { get; }

    /// <summary>
    /// Unique Id identifying the value. Use <see cref="Guid.NewGuid"/> method in constructor of an implementation
    /// of <see cref="IParsedValue"/> to set this value.
    /// </summary>
    Guid Id { get; }

    /// <summary>
    /// Root json value that owns this value.
    /// </summary>
    IRootParsedValue RootParsedValue { get; }

    /// <summary>
    /// If the value of <see cref="ParentJsonValue"/> is not null, then the value of <see cref="JsonKeyValue"/> should be null, and vice versa.
    /// </summary>
    IParsedValue? ParentJsonValue { get; }

    /// <summary>
    /// If the value of <see cref="ParentJsonValue"/> is not null, then the value of <see cref="JsonKeyValue"/> should be null, and vice versa.
    /// </summary>
    IJsonKeyValue? JsonKeyValue { get; }

    /// <summary>
    /// If the value is not null, specifies the position of parsed value in json file. The value 
    /// </summary>
    IJsonLineInfo? LineInfo { get; }
}
