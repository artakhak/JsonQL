namespace JsonQL.Demos.CustomJsonQL.Compilation.UniversalExpressionParserJsonQL;

/// <summary>
/// <b>NOTE: Strange operator names are used for demo custom operators to avoid conflicts with future non-custom operators in JsonQL. </b>
/// </summary>
public static class CustomJsonOperatorNames
{
    // Custom binary operators
    /// <summary>
    /// Adds two numbers and reverses teh sign. For example [3+-5] will be evaluated to -8.
    /// </summary>
    public const string AndNumbersAndReverseSign = "+-";

    // Custom unary prefix operators
    /// <summary>
    /// Increments a number by 2
    /// </summary>
    public const string IncrementByTwoPrefixOperator = "++2";

    // Custom unary postfix operators
    /// <summary>
    /// Postfix operator that returns true if the operand is even number.
    /// </summary>
    public static readonly IReadOnlyList<string> IsEvenPostfixOperators = ["is", "even"];
}
