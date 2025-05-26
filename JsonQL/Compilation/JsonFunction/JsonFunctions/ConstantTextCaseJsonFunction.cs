using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that evaluates to a constant string value.
/// </summary>
public class ConstantTextJsonFunction : StringJsonFunctionAbstr
{
    private readonly string _text;

    /// <summary>
    /// Represents a JSON function that evaluates to a constant string value.
    /// </summary>
    /// <param name="functionName">The name of the function as defined within the JSON context.</param>
    /// <param name="text">The constant string value that the function evaluates to.</param>
    /// <param name="jsonFunctionContext">The functional context in which this JSON function is executed.</param>
    /// <param name="lineInfo">Optional positional metadata related to the JSON source.</param>
    public ConstantTextJsonFunction(string functionName, string text, IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _text = text;
    }
    
    /// <inheritdoc />
    public override IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<string?>(_text);
    }
}