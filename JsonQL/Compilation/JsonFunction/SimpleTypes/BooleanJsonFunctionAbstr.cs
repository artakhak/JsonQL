using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an abstract base class for JSON functions that evaluate to a boolean value.
/// </summary>
/// <remarks>
/// This class provides a common framework for implementing JSON functions that return
/// boolean results. It ensures that derived classes provide specific implementations
/// of boolean evaluation logic.
/// </remarks>
public abstract class BooleanJsonFunctionAbstr : JsonFunctionAbstr, IBooleanJsonFunction
{
    /// <summary>
    /// Constructor.
    /// </summary>
    /// <param name="functionName">Function name.</param>
    /// <param name="jsonFunctionContext">Function context object.</param>
    /// <param name="lineInfo">Function position.</param>
    protected BooleanJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<bool?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.DoEvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToBoolean(this.LineInfo);
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.GetBooleanValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <summary>
    /// Evaluates and returns a boolean result based on the provided root parsed value,
    /// parent root parsed values, and evaluation context data.
    /// </summary>
    /// <param name="rootParsedValue">
    /// The root parsed value being evaluated.
    /// </param>
    /// <param name="compiledParentRootParsedValues">
    /// The collection of parent root parsed values relevant to the current evaluation.
    /// </param>
    /// <param name="contextData">
    /// Contextual data available for the evaluation process.
    /// </param>
    /// <returns>
    /// A parse result containing the evaluated boolean value or null.
    /// </returns>
    protected abstract IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}