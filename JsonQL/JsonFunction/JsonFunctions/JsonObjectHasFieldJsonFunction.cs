using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class JsonObjectHasFieldJsonFunction: BooleanJsonFunctionAbstr
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;
    private readonly IJsonFunction _fieldNameJsonFunction;

    public JsonObjectHasFieldJsonFunction(string functionName, IJsonValuePathJsonFunction jsonValuePathJsonFunction, IJsonFunction fieldNameJsonFunction,  IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        functionName, jsonFunctionContext, lineInfo)
    {
        _jsonValuePathJsonFunction = jsonValuePathJsonFunction;
        _fieldNameJsonFunction = fieldNameJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> GetBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var pathResult = _jsonValuePathJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (pathResult.Errors.Count > 0)
            return new ParseResult<bool?>(pathResult.Errors);

        if (pathResult.Value == null || pathResult.Value is not ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult ||
            singleItemJsonValuePathLookupResult.ParsedValue is not IParsedJson parsedJson)
            return new ParseResult<bool?>(false);

        var jsonKeyParseResult = _fieldNameJsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (jsonKeyParseResult.Errors.Count > 0)
            return new ParseResult<bool?>(jsonKeyParseResult.Errors);

        if (jsonKeyParseResult.Value is not string jsonKey)
        {
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(
                new JsonObjectParseError("Failed to evaluate the expression to a string Json key value.", _fieldNameJsonFunction.LineInfo)));
        }

        return new ParseResult<bool?>(parsedJson.HasKey(jsonKey));
    }
}