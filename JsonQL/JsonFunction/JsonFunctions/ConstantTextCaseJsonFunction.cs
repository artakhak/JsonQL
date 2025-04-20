using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class ConstantTextJsonFunction : StringJsonFunctionAbstr
{
    private readonly string _text;

    public ConstantTextJsonFunction(string functionName, string text, IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
        _text = text;
    }
    
    /// <inheritdoc />
    protected override IParseResult<string?> GetStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<string?>(_text);
    }
}