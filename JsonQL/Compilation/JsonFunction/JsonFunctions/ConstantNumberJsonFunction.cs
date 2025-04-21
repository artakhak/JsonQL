using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class ConstantNumberJsonFunction : DoubleJsonFunctionAbstr
{
    private readonly double _number;

    public ConstantNumberJsonFunction(string functionName, double number, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : 
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _number = number;
    }

    /// <inheritdoc />
    protected override IParseResult<double?> GetDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<double?>(_number);
    }
}