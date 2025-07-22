// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Diagnostics.CodeAnalysis;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonValueLookup;

// These classes will either be deleted or used in next release.
[Obsolete("TODO: Either use this for value transformations or delete")]
internal interface IParsedSimpleValueAttachedValues
{
    /// <summary>
    /// Maps an instance of <see cref="IParsedSimpleValue"/> to a new mutated value.<br/>
    /// Attached value might be set by mutation value path elements, as in this example<br/>
    /// parent.Array1.Select(x => x.Age).Transform(x => x + 10).
    /// </summary>
    /// <param name="parsedSimpleValue">Current value</param>
    /// <param name="attachedValue">Attached value</param>
    /// <returns></returns>
    internal bool TryGetAttachedValue(IParsedSimpleValue parsedSimpleValue, [NotNullWhen(true)] out IParsedSimpleValue? attachedValue);

    /// <summary>
    /// Associates the value in <param name="attachedValue"></param> with <param name="parsedSimpleValue"></param>.
    /// The original value <param name="parsedSimpleValue"></param> will be unchanged, but the new values will be used when
    /// <see cref="IJsonValuePathLookupResult"/> is used.
    /// </summary>
    /// <param name="parsedSimpleValue">Current value</param>
    /// <param name="attachedValue">Attached value</param>
    internal void AttachValue(IParsedSimpleValue parsedSimpleValue, IParsedSimpleValue attachedValue);
}

[Obsolete("TODO: Either use this for value transformations or delete")]
internal class ParsedSimpleValueAttachedValues : IParsedSimpleValueAttachedValues
{
    private readonly Dictionary<IParsedSimpleValue, IParsedSimpleValue> _valueToAttachedValueMap = new();

    /// <inheritdoc />
    public bool TryGetAttachedValue(IParsedSimpleValue parsedSimpleValue, [NotNullWhen(true)] out IParsedSimpleValue? attachedValue)
    {
        return _valueToAttachedValueMap.TryGetValue(parsedSimpleValue, out attachedValue);
    }

    /// <inheritdoc />
    public void AttachValue(IParsedSimpleValue parsedSimpleValue, IParsedSimpleValue attachedValue)
    {
        _valueToAttachedValueMap[parsedSimpleValue] = attachedValue;
    }
}
