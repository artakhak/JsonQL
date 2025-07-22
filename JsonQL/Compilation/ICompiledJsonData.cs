// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

/// <summary>
/// Represents compiled JSON data with associated metadata and parsed values.
/// </summary>
public interface ICompiledJsonData
{
    /// <summary>
    /// Gets the identifier used to uniquely represent a piece of JSON text within
    /// the compilation or query management processes. This property is leveraged
    /// to distinguish and relate JSON text data and its associated compiled results.
    /// </summary>
    string TextIdentifier { get; }

    /// <summary>
    /// Gets the raw JSON text associated with the compiled data. This property serves
    /// as the initial source input for parsing and compilation processes and is
    /// integral to representational and validation operations.
    /// </summary>
    string JsonText { get; }

    /// <summary>
    /// Gets the parsed representation of a JSON data structure after it has been compiled.
    /// This property encapsulates the result of parsing the root structure of a JSON document,
    /// enabling downstream operations to access and manipulate the structured data for tasks
    /// such as query execution or further transformations.
    /// </summary>
    IRootParsedValue CompiledParsedValue { get; }
}

/// <inheritdoc />
public class CompiledJsonData : ICompiledJsonData
{
    /// <summary>
    /// Represents a compiled JSON data object that contains the text identifier,
    /// the JSON text, and the corresponding compiled parsed value.
    /// </summary>
    public CompiledJsonData(string textIdentifier, string jsonText, IRootParsedValue compiledParsedValue)
    {
        TextIdentifier = textIdentifier;
        CompiledParsedValue = compiledParsedValue;
        JsonText = jsonText;
    }

    /// <inheritdoc />
    public string TextIdentifier { get; }

    /// <inheritdoc />
    public string JsonText { get; }

    /// <inheritdoc />
    public IRootParsedValue CompiledParsedValue { get; }
}
