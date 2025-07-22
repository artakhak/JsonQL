// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents the "GreaterThan" operator in the <see cref="BinaryNonEqualityComparisonOperatorType"/> enumeration.
/// Used to determine if the left-hand operand is strictly greater than the right-hand operand.
/// </summary>
public enum BinaryNonEqualityComparisonOperatorType
{
    /// <summary>
    /// Represents the "LessThan" operator in the <see cref="BinaryNonEqualityComparisonOperatorType"/> enumeration.
    /// Used to determine if the left-hand operand is strictly less than the right-hand operand.
    /// </summary>
    LessThan,

    /// <summary>
    /// Represents the "LessThanOrEqual" operator in the <see cref="BinaryNonEqualityComparisonOperatorType"/> enumeration.
    /// Used to determine if the left-hand operand is less than or equal to the right-hand operand.
    /// </summary>
    LessThanOrEqual,

    /// <summary>
    /// Represents the "GreaterThan" operator in the <see cref="BinaryNonEqualityComparisonOperatorType"/> enumeration.
    /// Used to determine if the left-hand operand is strictly greater than the right-hand operand.
    /// </summary>
    GreaterThan,

    /// <summary>
    /// Represents the "GreaterThanOrEqual" operator in the <see cref="BinaryNonEqualityComparisonOperatorType"/> enumeration.
    /// Used to determine if the left-hand operand is greater than or equal to the right-hand operand.
    /// </summary>
    GreaterThanOrEqual
}
