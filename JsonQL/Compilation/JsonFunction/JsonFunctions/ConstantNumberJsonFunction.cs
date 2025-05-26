using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that evaluates to a constant numerical value.
/// This class essentially encapsulates a single double value that can be utilized
/// within JSON evaluation contexts as a constant.
/// </summary>
public class ConstantNumberJsonFunction : DoubleJsonFunctionAbstr
{
    private readonly double _number;

    /// <summary>
    /// Represents a JSON function that evaluates to a constant number.
    /// </summary>
    /// <param name="functionName">The name of the function as defined within the JSON context.</param>
    /// <param name="number">The constant number value that this function evaluates to.</param>
    /// <param name="jsonFunctionContext">The context used for evaluating the function values within the JSON framework.</param>
    /// <param name="lineInfo">Optional information about the specific line within the JSON document where the function is located.</param>
    public ConstantNumberJsonFunction(string functionName, double number, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _number = number;
    }

    /// <inheritdoc />
    public override IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<double?>(_number);
    }
}