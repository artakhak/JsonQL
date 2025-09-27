// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.JsonObjects;

namespace JsonQL.Compilation;

/// <summary>
/// Represents a JSON object data structure that provides access to associated text data,
/// root parsed value, and a reference to its parent JSON object, if one exists.
/// </summary>
public interface IJsonObjectData
{
    /// <summary>
    /// Provides access to the text data associated with the JSON object.
    /// This is represented by an instance of <see cref="IJsonTextData"/>.
    /// </summary>
    IJsonTextData JsonTextData { get; }

    /// <summary>
    /// Parsed value. An instance of either <see cref="IRootParsedJson"/> if the compiled object is JSON object,
    /// or <see cref="IRootParsedArrayValue"/> if compiled JSON is an array.
    /// </summary>
    IRootParsedValue RootParsedValue { get; }

    /// <summary>
    /// Provides a reference to the parent JSON object associated with the current JSON object data.
    /// This property is nullable, indicating that a JSON object may not have a parent.
    /// </summary>
    IJsonObjectData? ParentJsonObjectData { get; }
}