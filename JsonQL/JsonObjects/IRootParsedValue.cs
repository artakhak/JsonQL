// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.JsonObjects;

/// <summary>
/// An interface for root-parsed JSON. Can be either <see cref="IParsedJson"/> or <see cref="IRootParsedArrayValue"/>.
/// </summary>
public interface IRootParsedValue: IParsedValue
{
    
    /// <summary>
    /// If the value is not null, json text identifier for json file that has the json object that resulted in conversion error.
    /// </summary>
    string JsonTextIdentifier { get; }

    /// <summary>
    /// If value with <see cref="IParsedValue.Id"/> is found in <see cref="IRootParsedValue"/>, returns true, and the value of
    /// <param name="parsedValue"></param> is set to this value.
    /// Otherwise, false is returned, and  <param name="parsedValue"></param> is set to null.
    /// </summary>
    /// <param name="valueId">Value Id.</param>
    /// <param name="parsedValue">Value being looked up by Id.</param>
    bool TryGetParsedValue(Guid valueId, [NotNullWhen(true)]out IParsedValue? parsedValue);

    /// <summary>
    /// Called by JSON classes, such as <see cref="IParsedJson"/> or <see cref="IParsedArrayValue"/> when values are added.
    /// The <see cref="IRootParsedValue"/> can implement this method to do some internal maintenance of json objects.
    /// </summary>
    /// <param name="parsedValue"></param>
    /// <remarks>
    /// This call should be done for every JSON object added. In other words, the call is made for parent JSON<br/>
    /// object as well as for every child object, and the implementation of <see cref="ValueAdded"/> does not have<br/>
    /// to recursively process child JSON values of <param name="parsedValue"></param>.<br/>
    /// The reasoning behind this is that the call to <see cref="ValueAdded"/> is normally done in JSON classes that have<br/>
    /// methods for adding child JSON values (e.g., <see cref="IParsedJson.this[string]"/>,<br/>
    /// <see cref="IParsedArrayValue.AddValueAt"/>, etc.), and the methods to add child JSON values will be called for every<br/>
    /// child value.
    /// </remarks>
    void ValueAdded(IParsedValue parsedValue);

    /// <summary>
    /// Called by JSON classes, such as <see cref="IParsedJson"/> or <see cref="IParsedArrayValue"/> when values are removed.
    /// The <see cref="IRootParsedValue"/> can implement this method to do some internal maintenance of json objects.
    /// </summary>
    /// <param name="parsedValue"></param>
    /// <remarks>
    /// This call should be done for every JSON object removed. In other words, the call is made for parent JSON<br/>
    /// object as well as for every child object, and the implementation of <see cref="ValueRemoved"/> does not have<br/>
    /// to recursively process child JSON values of <param name="parsedValue"></param>.<br/>
    /// </remarks>
    void ValueRemoved(IParsedValue parsedValue);
}