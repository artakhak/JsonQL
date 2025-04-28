using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that evaluates whether a JSON object contains a specific field.
/// </summary>
public class JsonObjectHasFieldJsonFunction: BooleanJsonFunctionAbstr
{
    private readonly IJsonValuePathJsonFunction _jsonValuePathJsonFunction;
    private readonly IJsonFunction _fieldNameJsonFunction;

    /// <summary>
    /// Represents a JSON function that checks whether a specific field exists within a JSON object.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="jsonValuePathJsonFunction">The JSON value path function used to locate the target JSON object.</param>
    /// <param name="fieldNameJsonFunction">The function that provides the field name to check for existence.</param>
    /// <param name="jsonFunctionContext">The context in which the JSON function is being evaluated.</param>
    /// <param name="lineInfo">Debugging information related to the source code location.</param>
    public JsonObjectHasFieldJsonFunction(string functionName, IJsonValuePathJsonFunction jsonValuePathJsonFunction, IJsonFunction fieldNameJsonFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
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