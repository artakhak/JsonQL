using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function whose result provides the type of a JSON data value.
/// </summary>
/// <remarks>
/// This class operates as a specific implementation of <see cref="StringJsonFunctionAbstr"/> by evaluating
/// the type of a given JSON value evaluated through a JSON function during compilation or data processing.
/// The result of this function is typically a string representation of the data type of the JSON value,
/// such as a collection type, single item type, or the "undefined" type if no valid type can be derived.
/// The evaluation is performed using the provided <see cref="IJsonFunction"/> which processes the value,
/// along with a context and any relevant inputs from the JSON structure or compiled parent values.
/// </remarks>
/// <example>
/// This class is not intended to be used directly. It is typically utilized as part of
/// the JSON Query Language (JsonQL) compilation process for type determination of JSON values.
/// </example>
/// <threadsafety>
/// This class is not guaranteed to be thread-safe. Concurrent usage might require external synchronization.
/// </threadsafety>
public class TypeOfJsonFunctionResultFunction : StringJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;

    /// <summary>
    /// Represents a JSON function that evaluates the type of a JSON function result.
    /// </summary>
    /// <remarks>
    /// This class extends from <see cref="StringJsonFunctionAbstr"/> to provide functionality for determining
    /// the type of the result produced by another JSON function.
    /// </remarks>
    /// <param name="functionName">The name of the JSON function.</param>
    /// <param name="jsonFunction">The JSON function whose result type is to be evaluated.</param>
    /// <param name="jsonFunctionContext">The context for evaluating JSON function values.</param>
    /// <param name="lineInfo">Optional line information for debugging or error tracking purposes.</param>
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
    public override IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
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