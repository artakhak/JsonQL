using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public class TypeOfJsonFunctionResultFunction : StringJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    public TypeOfJsonFunctionResultFunction(string functionName, IJsonFunction jsonFunction, 
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
        functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
    }

    private IParseResult<string?> GetUndefinedType()
    {
        return new ParseResult<string?>(JsonFunctionResultType.Undefined.ToString());
    }

    /// <inheritdoc />
    protected override IParseResult<string?> GetStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var parseResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (parseResult.Errors.Count > 0)
            return new ParseResult<string?>(parseResult.Errors);

        if (parseResult.Value == null)
            return GetUndefinedType();

        if (parseResult.Value is ICollectionJsonValuePathLookupResult collectionJsonValuePathLookupResult)
        {
            if (collectionJsonValuePathLookupResult.ParsedValues.Count == 0)
                return GetUndefinedType();

            return new ParseResult<string?>(JsonFunctionResultType.Collection.ToString());
        }

        JsonFunctionResultType? typeOfFunctionValue = null;

        if (parseResult.Value is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult)
        {
            if (singleItemJsonValuePathLookupResult.ParsedValue == null)
                return GetUndefinedType();

            typeOfFunctionValue = GetJsonFunctionResultType(singleItemJsonValuePathLookupResult.ParsedValue);
        }
        else if (parseResult.Value is IParsedValue parsedValue)
        {
            typeOfFunctionValue = GetJsonFunctionResultType(parsedValue);
        }
        else
        {
            typeOfFunctionValue = GetJsonFunctionResultType(parseResult.Value);
        }

        if (typeOfFunctionValue == null)
            return GetUndefinedType();

        return new ParseResult<string?>(typeOfFunctionValue.ToString());
    }

    private JsonFunctionResultType? GetJsonFunctionResultType(IParsedValue parsedValue)
    {
        switch (parsedValue)
        {
            case IParsedJson:
                return JsonFunctionResultType.JsonObject;

            case IParsedArrayValue:
                return JsonFunctionResultType.JsonArray;

            case IParsedSimpleValue parsedSimpleValue:
                return GetJsonFunctionResultType(parsedSimpleValue);
        }

        return null;

    }
    private JsonFunctionResultType GetJsonFunctionResultType(IParsedSimpleValue parsedSimpleValue)
    {
        if (parsedSimpleValue.IsString)
            return JsonFunctionResultType.String;

        if (parsedSimpleValue.Value == Constants.JsonTrueValue || parsedSimpleValue.Value == Constants.JsonFalseValue)
            return JsonFunctionResultType.Boolean;

        if (parsedSimpleValue.Value == null)
            return JsonFunctionResultType.JsonNull;

        return JsonFunctionResultType.Number;
    }

    private JsonFunctionResultType? GetJsonFunctionResultType(object value)
    {
        if (value is bool)
            return JsonFunctionResultType.Boolean;
        
        if (value is string)
            return JsonFunctionResultType.String;
        
        if (value is DateTime)
            return JsonFunctionResultType.DateTime;
        
        if (value is double)
            return JsonFunctionResultType.Number;

        return null;
    }
}