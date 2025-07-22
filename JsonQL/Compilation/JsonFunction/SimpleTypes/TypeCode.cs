// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents the type codes for simple types within the system.
/// </summary>
public enum TypeCode
{
    /// <summary>
    /// Represents a string type in the <see cref="TypeCode"/> enumeration.
    /// This type code is used to indicate that the corresponding value or entity
    /// is of a textual string type.
    /// </summary>
    String,

    /// <summary>
    /// Represents a double-precision floating-point type in the <see cref="TypeCode"/> enumeration.
    /// This type code is used to indicate that the corresponding value or entity
    /// is of a double-precision numeric type.
    /// </summary>
    Double,

    /// <summary>
    /// Represents a boolean type in the <see cref="TypeCode"/> enumeration.
    /// This type code is used to indicate that the corresponding value or entity
    /// is of a Boolean (true/false) type.
    /// </summary>
    Boolean,

    /// <summary>
    /// Represents a date and time type in the <see cref="TypeCode"/> enumeration.
    /// This type code is used to indicate entities or values that correspond
    /// to date and time data.
    /// </summary>
    DateTime
}
