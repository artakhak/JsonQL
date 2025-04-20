using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.AssertFunctions;

public class AssertOperatorFunction : JsonFunctionAbstr
{
    private readonly IJsonFunction _assertedValueJsonFunction;

    public AssertOperatorFunction(string functionName, IJsonFunction assertedValueJsonFunction,  IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _assertedValueJsonFunction = assertedValueJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return AssertOperatorFunctionHelpers.GetParseResultWithErrorIfValueIsNull(
            _assertedValueJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData), LineInfo);
    }
}