// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class StringJsonComparable : IJsonComparable
{
    /// <summary>
    /// Represents a JSON-compatible string value that implements the IJsonComparable interface.
    /// </summary>
    public StringJsonComparable(string value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.String;

    /// <inheritdoc />
    public IComparable Value { get; }
}