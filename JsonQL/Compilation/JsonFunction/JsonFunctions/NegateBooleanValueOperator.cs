using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function for negating a boolean value.
/// </summary>
/// <remarks>
/// This class inherits from <c>BooleanJsonFunctionAbstr</c> and implements the functionality
/// to evaluate and negate a boolean value.
/// </remarks>
/// <example>
/// This operator takes a boolean value, evaluates it, and returns its negated value.
/// If the value cannot be converted to a boolean, the result will be null.
/// </example>
/// <threadsafety>
/// This class is not thread-safe. Concurrent access to its methods or properties
/// should be managed by the caller to avoid unexpected behavior.
/// </threadsafety>
public class NegateBooleanValueOperator : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents a boolean negation operator within a JSON function system.
    /// </summary>
    /// <param name="operatorName">The name of the operator.</param>
    /// <param name="jsonFunction">The JSON function to be negated.</param>
    /// <param name="jsonFunctionContext">The evaluation context for the JSON function.</param>
    /// <param name="lineInfo">Optional information about the line in the source where this operator is defined.</param>
    public NegateBooleanValueOperator(string operatorName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<bool?>(valueResult.Errors);

        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(valueResult.Value, TypeCode.Boolean, out var comparableValue) ||
            comparableValue.Value is not bool boolValue)
            return new ParseResult<bool?>((bool?)null);

        return new ParseResult<bool?>(!boolValue);
    }
}