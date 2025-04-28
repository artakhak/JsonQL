using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an abstract base class for JSON functions that evaluate to a double or nullable double value.
/// </summary>
/// <remarks>
/// This class provides a base implementation for JSON functions that handle numeric computations or operations.
/// Subclasses are expected to implement the abstract method <c>GetDoubleValue</c>, which defines the specific logic
/// for computing the double value based on the input parameters.
/// </remarks>
public abstract class DoubleJsonFunctionAbstr : JsonFunctionAbstr, IDoubleJsonFunction
{
    /// <summary>
    /// Represents an abstract base class for JSON functions that operate on or return double values.
    /// </summary>
    /// <remarks>
    /// This class serves as a foundation for implementing specific types of JSON functions that deal with double precision numerical values.
    /// Derived classes are expected to define specific behavior in the context of JSON evaluation.
    /// </remarks>
    protected DoubleJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.DoEvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(this.LineInfo);
    }

    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.GetDoubleValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <summary>
    /// Retrieves a double-precision numerical value based on the provided JSON parsing context and parent values.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value representing the current JSON node being evaluated.</param>
    /// <param name="compiledParentRootParsedValues">A readonly list of parent root parsed values within the evaluation context.</param>
    /// <param name="contextData">Optional evaluation context data that provides additional information for the function execution.</param>
    /// <returns>An object implementing <see cref="IParseResult{T}"/> representing the parsed double-precision value. Returns null if the evaluation does not yield a valid result.</returns>
    protected abstract IParseResult<double?> GetDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}