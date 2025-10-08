// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
namespace JsonQL.Compilation.UniversalExpressionParserJsonQL;

public static class JsonOperatorNames
{
    // Binary operators
    /// <summary>
    /// Represents the character sequence used to separate elements in a JSON path.
    /// </summary>
    public const string JsonPathSeparator = ".";

    /// <summary>
    /// Represents the operator used to define a lambda expression in JSON queries.
    /// </summary>
    public const string LambdaOperator = "=>";

    /// <summary>
    /// Represents the operator used to define default values in JSON expressions.
    /// </summary>
    public const string DefaultValueOperator = ":";

    /// <summary>
    /// Represents the operator used to indicate a relationship or mapping between a function parameter and its
    /// value in JSON processing.
    /// </summary>
    public const string JsonFunctionParameterValueOperator = "->";

    /// <summary>
    /// Represents the operator used to signify equality comparison in JSON expressions.
    /// </summary>
    public const string EqualsOperator = "==";

    /// <summary>
    /// Represents the operator used to denote inequality in a JSON parsing or querying context.
    /// </summary>
    public const string NotEqualsOperator = "!=";

    /// <summary>
    /// Represents the operator used to denote a "less than" comparison in JSON query language.
    /// </summary>
    public const string LessThanOperator = "<";

    /// <summary>
    /// Represents the less than or equal to operator ("&lt;=").
    /// </summary>
    public const string LessThanOrEqualOperator = "<=";

    /// <summary>
    /// Represents the operator used to indicate a "greater than" comparison within a JSON expression.
    /// </summary>
    public const string GreaterThanOperator = ">";

    /// <summary>
    /// Represents the operator used to evaluate if the left operand is greater than or equal to the right operand.
    /// </summary>
    public const string GreaterThanOrEqualOperator = ">=";

    /// <summary>
    /// Represents the operator name used to indicate a "contains" operation in JSON query expressions.
    /// </summary>
    public const string ContainsOperator = "contains";

    /// <summary>
    /// Represents the list of operator names used for specifying that a value begins with a given prefix in JSON queries.
    /// </summary>
    public static readonly IReadOnlyList<string> StartsWith = CollectionExpressionHelpers.Create("starts", "with");

    /// <summary>
    /// Represents the sequence of strings that define the "ends with" operator,
    /// commonly used to check if a string ends with a specified substring in JSON expressions.
    /// </summary>
    public static readonly IReadOnlyList<string> EndsWith = CollectionExpressionHelpers.Create("ends", "with");

    /// <summary>
    /// Represents the operator string for logical "AND" operations in a JSON query.
    /// </summary>
    public const string AndOperator = "&&";

    /// <summary>
    /// Represents the logical OR operator used for conditional expressions.
    /// </summary>
    public const string OrOperator = "||";

    /// <summary>
    /// Represents the addition operator symbol used in JSON expressions.
    /// </summary>
    public const string AddOperator = "+";

    /// <summary>
    /// Represents the subtraction operator symbol used in JSON-based expressions.
    /// </summary>
    public const string SubtractOperator = "-";

    /// <summary>
    /// Represents the string symbol used to denote the multiplication operation in JSON expressions.
    /// </summary>
    public const string MultiplyOperator = "*";

    /// <summary>
    /// Represents the division operator symbol used in JSON path expressions.
    /// </summary>
    public const string DivideOperator = "/";

    /// <summary>
    /// Represents the operator name used to denote the modulus operation in JSON expressions.
    /// </summary>
    public const string ModulusOperator = "%";

    // Unary postfix operators
    /// <summary>
    /// Represents the operator used to check if a value is JSON null.
    /// </summary>
    public static readonly IReadOnlyList<string> IsNullOperator = CollectionExpressionHelpers.Create("is", "null");

    /// <summary>
    /// Operator for checking if the value is not JSON null.
    /// </summary>
    public static readonly IReadOnlyList<string> IsNotNullOperator = CollectionExpressionHelpers.Create("is", "not", "null");

    /// <summary>
    /// Operator for checking if the value is missing. For example, JSON path references a missing value.
    /// </summary>
    public static readonly IReadOnlyList<string> IsUndefinedOperator = CollectionExpressionHelpers.Create("is", "undefined");

    /// <summary>
    /// Operator for checking if the value is not missing. For example, JSON path references a missing value.
    /// </summary>
    public static readonly IReadOnlyList<string> IsNotUndefinedOperator = CollectionExpressionHelpers.Create("is", "not", "undefined");

    /// <summary>
    /// Asserts that the value is not null or undefined.
    /// </summary>
    public const string AssertNotNull = "assert";

    // Unary prefix operators
    /// <summary>
    /// Defines the unary prefix operator used to represent logical negation.
    /// </summary>
    public const string NegateOperator = "!";

    /// <summary>
    /// Represents the operator used to denote negative numerical values.
    /// </summary>
    public const string NegativeValueOperator = "-";

    /// <summary>
    /// Represents the operator name for retrieving the type of a value in a JSON query language.
    /// </summary>
    public const string TypeOfOperator = "typeof";
}
