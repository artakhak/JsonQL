// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <inheritdoc />
public class DateTimeJsonComparable : IJsonComparable
{
    /// <summary>
    /// Represents a JSON comparable object for DateTime values. Implements the <see cref="IJsonComparable"/>
    /// interface to enable comparison operations for DateTime values in JSON use cases.
    /// </summary>
    public DateTimeJsonComparable(DateTime value)
    {
        Value = value;
    }

    /// <inheritdoc />
    public TypeCode TypeCode { get; } = TypeCode.DateTime;

    /// <inheritdoc />
    public IComparable Value { get; }
}
