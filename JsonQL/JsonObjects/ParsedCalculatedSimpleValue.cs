// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.JsonObjects;

/// <summary>
/// An implementation of <see cref="IParsedSimpleValue"/> for calculated values for which there is no actual JSON value.
/// </summary>
public class ParsedCalculatedSimpleValue : ParsedSimpleValue, IParsedCalculatedValue
{
    public ParsedCalculatedSimpleValue(IRootParsedValue rootParsedValue, string? value, bool isString) : base(rootParsedValue, null, null, null, value, isString)
    {
    }
}