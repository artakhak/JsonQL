// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation;

/// <summary>
/// Information about Json text.
/// </summary>
public interface IJsonTextData
{
    /// <summary>
    /// Unique identifier. Example: "File1.Json" or "549B9915-9768-4DC4-BE15-B17A394ED7B3"
    /// </summary>
    string TextIdentifier { get; }

    /// <summary>
    /// Represents JSON text data as a string. Typically contains the raw JSON content
    /// that can be processed or manipulated further in various operations.
    /// </summary>
    string JsonText { get; }

    /// <summary>
    /// Represents the parent JSON text data associated with the current instance.
    /// Can be null if there is no parent data.
    /// </summary>
    IJsonTextData? ParentJsonTextData { get; }
}