using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that computes the absolute value of a numeric input.
/// This function evaluates a given JSON function and ensures the output is a non-negative value
/// by applying the absolute value operation.
/// </summary>
public class AbsoluteValueJsonFunction : DoubleJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents a JSON function responsible for calculating the absolute value of a numeric value within a JSON function context.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="jsonFunction">The underlying JSON function representing the input value to process.</param>
    /// <param name="jsonFunctionContext">The context in which the JSON function operates, including details about variables and scope.</param>
    /// <param name="lineInfo">Optional information about the source code line associated with the function for debugging or error reporting.</param>
    public AbsoluteValueJsonFunction(string functionName, IJsonFunction jsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<double?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Double, out var comparableValue) ||
            comparableValue.Value is not double doubleValue)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>(Math.Abs(doubleValue));
    }
}