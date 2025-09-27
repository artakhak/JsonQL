// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;

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
    /// NOTE: In most cases, no value should be provided for <param name="index"></param>. The only reason this
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