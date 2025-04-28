using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function implementation that evaluates whether a given value
/// is not undefined. This function extends the functionality of a Boolean JSON function.
/// </summary>
/// <remarks>
/// The primary purpose of this class is to perform a logical check to determine
/// if a specific JSON value is defined (i.e., not undefined). This is achieved
/// by leveraging the helper methods and evaluation logic provided in the base class
/// and related utilities.
/// </remarks>
/// <example>
/// This function relies on a provided JSON function and evaluation context.
/// Any parsing errors during evaluation are encapsulated and included in the
/// resulting parse result.
/// </example>
/// <typeparam>
/// Utilizes JSON function types and error handling from the extended hierarchy
/// and external helpers to carry out the function's execution.
/// </typeparam>
public class IsNotUndefinedOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents a JSON function implementation that evaluates whether a given value
    /// is not undefined.
    /// </summary>
    /// <param name="operatorName">The name of the operator.</param>
    /// <param name="jsonFunction">The JSON function to evaluate the value.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function values.</param>
    /// <param name="lineInfo">The line information for error reporting in JSON operations, if available.</param>
    public IsNotUndefinedOperatorFunction(string operatorName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var isUndefinedResult = IsNullUndefinedFunctionHelpers.IsUndefined(rootParsedValue, compiledParentRootParsedValues, contextData, _jsonFunction);

        if (isUndefinedResult.Errors.Count > 0)
            return isUndefinedResult;

        if (isUndefinedResult.Value == null)
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("The value failed to parse.", this.LineInfo)));

        return new ParseResult<bool?>(!isUndefinedResult.Value);
    }
}