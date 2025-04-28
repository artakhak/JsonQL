using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Abstract base class for DateTime-based JSON function implementations.
/// Provides common functionality for evaluating and extracting DateTime values
/// from JSON data by implementing and extending functionality defined in JsonFunctionAbstr.
/// </summary>
public abstract class DateTimeJsonFunctionAbstr : JsonFunctionAbstr, IDateTimeJsonFunction
{
    /// <summary>
    /// Serves as an abstract base class for JSON functions that handle DateTime operations.
    /// </summary>
    /// <remarks>
    /// This class defines the foundation for implementing specific DateTime-related JSON functions.
    /// It provides a mechanism to evaluate DateTime values and enforces derived classes to implement
    /// the logic for retrieving a valid DateTime value.
    /// </remarks>
    protected DateTimeJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {

    }

    /// <inheritdoc />
    public IParseResult<DateTime?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.DoEvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDateTime(this.LineInfo);
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.GetDateTimeValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <summary>
    /// Retrieves a parsed DateTime value based on the provided root parsed value, its parent values,
    /// and optional evaluation context data.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value from which the DateTime value is evaluated.</param>
    /// <param name="compiledParentRootParsedValues">The list of compiled parent parsed values to aid in the evaluation process.</param>
    /// <param name="contextData">Optional data for evaluating the function in a specific context.</param>
    /// <returns>A parse result containing the evaluated DateTime value, or null if the value is not available or cannot be determined.</returns>
    protected abstract IParseResult<DateTime?> GetDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}