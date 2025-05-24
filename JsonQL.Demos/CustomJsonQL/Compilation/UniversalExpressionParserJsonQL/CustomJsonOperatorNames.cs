namespace JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;

/// <summary>
/// <b>NOTE: Strange operator names are used for demo custom operators to avoid conflicts with future non-custom operators in JsonQL. </b>
/// </summary>
public static class CustomJsonOperatorNames
{
    // Custom binary operators
    public const string AddAndIncrementByTwo = "+2";

    // Custom unary prefix operators
    public const string IncrementByTwoPrefixOperator = "++2";

    // Custom unary postfix operators
    public const string DecrementByTwoPostfixOperator = "--2";
}