using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class IsUndefinedOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    public IsUndefinedOperatorFunction(string operatorName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return IsNullUndefinedFunctionHelpers.IsUndefined(rootParsedValue, compiledParentRootParsedValues, contextData, _jsonFunction);
    }
}