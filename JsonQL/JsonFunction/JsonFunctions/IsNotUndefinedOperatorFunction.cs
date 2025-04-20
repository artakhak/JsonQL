using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class IsNotUndefinedOperatorFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    public IsNotUndefinedOperatorFunction(string operatorName, IJsonFunction jsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        operatorName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var isUndefinedResult = IsNullUndefinedFunctionHelpers.IsUndefined(rootParsedValue, compiledParentRootParsedValues, contextData, _jsonFunction);

        if (isUndefinedResult.Errors.Count > 0)
            return isUndefinedResult;

        if (isUndefinedResult.Value == null)
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("The value failed to parse.", this.LineInfo)));

        return new ParseResult<bool?>(!isUndefinedResult.Value);
    }
}