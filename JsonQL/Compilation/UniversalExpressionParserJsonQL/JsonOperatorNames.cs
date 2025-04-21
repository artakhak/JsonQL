namespace JsonQL.Compilation.UniversalExpressionParserJsonQL;

public static class JsonOperatorNames
{
    // Binary operators

    public const string JsonPathSeparator = ".";
    public const string LambdaOperator = "=>";
    public const string DefaultValueOperator = ":";
    public const string JsonFunctionParameterValueOperator = "->";

    public const string EqualsOperator = "==";
    public const string NotEqualsOperator = "!=";
    public const string LessThanOperator = "<";
    public const string LessThanOrEqualOperator = "<=";
    public const string GreaterThanOperator = ">";
    public const string GreaterThanOrEqualOperator = ">=";
    public const string ContainsOperator = "contains";

    public static readonly IReadOnlyList<string> StartsWith = CollectionExpressionHelpers.Create("starts", "with");
    public static readonly IReadOnlyList<string> EndsWith = CollectionExpressionHelpers.Create("ends", "with");
  
    public const string AndOperator = "&&";
    public const string OrOperator = "||";

    public const string AddOperator = "+";
    public const string SubtractOperator = "-";
    public const string MultiplyOperator = "*";
    public const string DivideOperator = "/";

    public const string QuotientOperator = "%";

    // Unary postfix operators
    /// <summary>
    /// Operator for checking if the value is json null
    /// </summary>
    public static readonly IReadOnlyList<string> IsNullOperator = CollectionExpressionHelpers.Create("is", "null");

    /// <summary>
    /// Operator for checking if the value is not json null
    /// </summary>
    public static readonly IReadOnlyList<string> IsNotNullOperator = CollectionExpressionHelpers.Create("is", "not", "null");

    /// <summary>
    /// Operator for checking if the value is missing. For example json path references missing value
    /// </summary>
    public static readonly IReadOnlyList<string> IsUndefinedOperator = CollectionExpressionHelpers.Create("is", "undefined");

    /// <summary>
    /// Operator for checking if the value is not missing. For example json path references missing value
    /// </summary>
    public static readonly IReadOnlyList<string> IsNotUndefinedOperator = CollectionExpressionHelpers.Create("is", "not", "undefined");

    /// <summary>
    /// Asserts that the value is not null or undefined.
    /// </summary>
    public const string AssertNotNull = "assert";

    // Unary prefix operators
    public const string NegateOperator = "!";
    public const string NegativeValueOperator = "-";
    public const string TypeOfOperator = "typeof";
}