using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class IsNullOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;

    public IsNullOperatorFunction(string operatorName, IJsonValuePathJsonFunction jsonValuePathJsonFunction, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonValuePathJsonFunction = jsonValuePathJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return IsNullUndefinedFunctionHelpers.IsNull(rootParsedValue, compiledParentRootParsedValues, contextData,
            _jsonValuePathJsonFunction);
    }
}