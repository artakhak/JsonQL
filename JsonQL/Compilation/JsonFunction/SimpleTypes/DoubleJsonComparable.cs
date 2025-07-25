// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class DoubleJsonComparable : IJsonComparable
{
    /// <summary>
    /// Represents a comparable value of type double within JSON context, allowing comparisons
    /// and type code identification for double values.
    /// </summary>
    public DoubleJsonComparable(double value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.Double;

    /// <inheritdoc />
    public IComparable Value { get; }
}
