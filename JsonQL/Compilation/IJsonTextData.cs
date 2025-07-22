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

/// <inheritdoc />
public class JsonTextData: IJsonTextData
{
    /// <summary>
    /// Represents JSON text data and allows for hierarchical structuring of JSON text.
    /// Used across the JSON compilation and query processes for identifying and handling JSON text.
    /// This class provides:
    /// - The ability to store the raw JSON text content.
    /// - Identification through a unique text identifier.
    /// - Linking to a parent JSON text data instance for hierarchical relationships.
    /// </summary>
    public JsonTextData(string textIdentifier, string jsonText, IJsonTextData? parentJsonTextData = null)
    {
        TextIdentifier = textIdentifier;
        JsonText = jsonText;
        ParentJsonTextData = parentJsonTextData;
    }

    /// <inheritdoc />
    public string TextIdentifier { get; }
    
    /// <inheritdoc />
    public string JsonText { get; }
    
    /// <inheritdoc />
    public IJsonTextData? ParentJsonTextData { get; }
}
