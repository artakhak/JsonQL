using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public static class IsNullUndefinedFunctionHelpers
{
    public static IParseResult<bool?> IsUndefined(IRootParsedValue rootParsedValue, 
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData,
        IJsonFunction jsonFunction)
    {
        var valueResult = jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (valueResult.Errors.Count > 0)
            return new ParseResult<bool?>(valueResult.Errors);

        if (valueResult.Value is IJsonValuePathLookupResult jsonValuePathLookupResult)
        {
            if (jsonValuePathLookupResult is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult &&
                !singleItemJsonValuePathLookupResult.IsValidPath)
                return new ParseResult<bool?>(true);

            return new ParseResult<bool?>(false);
        }

        return new ParseResult<bool?>(valueResult.Value == null);
    }

    public static IParseResult<bool?> IsNull(IRootParsedValue rootParsedValue,
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData,
        IJsonValuePathJsonFunction jsonValuePathJsonFunction)
    {
        var pathResult = jsonValuePathJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (pathResult.Errors.Count > 0)
            return new ParseResult<bool?>(pathResult.Errors);

        return new ParseResult<bool?>(pathResult.Value is ISingleItemJsonValuePathLookupResult 
            {ParsedValue: IParsedSimpleValue {IsString: false, Value: null}}
        );
    }
}