using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.AssertFunctions;

public class AssertOperatorDateTimeFunction : DateTimeJsonFunctionAbstr
{
    private readonly IDateTimeJsonFunction _assertedOperatorFunction;

    public AssertOperatorDateTimeFunction(string functionName, IDateTimeJsonFunction assertedOperatorFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _assertedOperatorFunction = assertedOperatorFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<DateTime?> GetDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return AssertOperatorFunctionHelpers.GetParseResultWithErrorIfValueIsNull(
            _assertedOperatorFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData), LineInfo);
    }
}