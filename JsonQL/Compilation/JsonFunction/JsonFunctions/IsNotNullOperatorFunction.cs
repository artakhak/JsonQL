using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class IsNotNullOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;

    public IsNotNullOperatorFunction(string operatorName, IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonValuePathJsonFunction = jsonValuePathJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var valueResult = IsNullUndefinedFunctionHelpers.IsNull(rootParsedValue, compiledParentRootParsedValues, contextData,
            _jsonValuePathJsonFunction);

        if (valueResult.Errors.Count > 0)
            return valueResult;

        if (valueResult.Value == null)
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("The value failed to parse.", this.LineInfo)));

        return new ParseResult<bool?>(!valueResult.Value.Value);
    }
}