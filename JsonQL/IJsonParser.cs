using JsonQL.JsonObjects;
using JsonQL.NewtonsoftImplementations;
using Newtonsoft.Json.Linq;
using OROptimizer.Diagnostics.Log;
using IJsonLineInfo = Newtonsoft.Json.IJsonLineInfo;

namespace JsonQL;

public interface IJsonParser
{
    /// <summary>
    /// Parses a JSON text into <see cref="IRootParsedValue"/>.<br/>
    /// The parsed value is either <see cref="IRootParsedJson"/> if the root object is JSON object,
    /// or <see cref="IRootParsedArrayValue"/> if the root object is an array.
    /// </summary>
    /// <param name="jsonText">Parsed text.</param>
    /// <exception cref="Exception">Throws this exception.</exception>
    IRootParsedValue Parse(string jsonText);
}

/// <inheritdoc />
public class JsonParser : IJsonParser
{
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly ICustomNewtonsoftJToken _customNewtonsoftJToken;

    private readonly ILog _logger;

    public JsonParser(IParsedJsonVisitor parsedJsonVisitor, ICustomNewtonsoftJToken customNewtonsoftJToken, ILog logger)
    {
        _parsedJsonVisitor = parsedJsonVisitor;
        _customNewtonsoftJToken = customNewtonsoftJToken;
        _logger = logger;
    }

    /// <inheritdoc />
    public IRootParsedValue Parse(string jsonText)
    {
        ThreadStaticLoggingContext.Context = _logger;

        var parsedJToken = _customNewtonsoftJToken.Parse(jsonText, new JsonLoadSettings
        {
            CommentHandling = CommentHandling.Ignore,
            DuplicatePropertyNameHandling = DuplicatePropertyNameHandling.Error,
            LineInfoHandling = LineInfoHandling.Load
        });
       
        switch (parsedJToken.Type)
        {
            case JTokenType.Object:
                var jObject = ConvertToJTokenTypeOrThrowException<JObject>(parsedJToken);

                var rootParsedJson = new RootParsedJson(_parsedJsonVisitor)
                {
                    LineInfo = GetJsonLineInfo(jObject, 0)
                };
                PopulateParsedJson(rootParsedJson, rootParsedJson, jObject.Children());
                return rootParsedJson;

            case JTokenType.Array:
                var jArray = ConvertToJTokenTypeOrThrowException<JArray>(parsedJToken);

                var rootParsedArrayValue = new RootParsedArrayValue(_parsedJsonVisitor)
                {
                    LineInfo = GetJsonLineInfo(jArray, 0)
                };

                PopulateParsedArrayValue(rootParsedArrayValue, rootParsedArrayValue, jArray.Children());
                return rootParsedArrayValue;

            default:
                throw new ArgumentException($"Invalid json object type [{parsedJToken.Type}]. The value of  ", nameof(jsonText));
        }
    }

    private ParsedJson ConvertParsedJson(IRootParsedValue rootParsedValue, IParsedValue parentJsonValue, IJsonKeyValue? jsonKeyValue, JObject jObject)
    {
        var parsedJson = new ParsedJson(_parsedJsonVisitor, rootParsedValue, parentJsonValue, jsonKeyValue)
        {
            LineInfo = GetJsonLineInfo(jObject, 0)
        };

        PopulateParsedJson(rootParsedValue, parsedJson, jObject.Children());
        return parsedJson;
    }

    private void PopulateParsedJson(IRootParsedValue rootParsedValue, IParsedJson parsedJson, JEnumerable<JToken> childrenTokens)
    {
        foreach (var jToken in childrenTokens)
        {
            var jProperty = ConvertToJTokenTypeOrThrowException<JProperty>(jToken);

            if (parsedJson.TryGetJsonKeyValue(jProperty.Name, out _))
                throw new ApplicationException($"Json object at path [{parsedJson.GetPath()}] has multiple occurrences of key [{jProperty.Name}]");

            var jsonKeyValue = new JsonKeyValue(jProperty.Name, parsedJson);
            var parsedValue = ConvertToParsedValue(rootParsedValue, parsedJson, jsonKeyValue, jProperty.Value);
            jsonKeyValue.Value = parsedValue;
            jsonKeyValue.LineInfo = GetJsonLineInfo(jProperty, -jsonKeyValue.Key.Length);
            parsedJson[jProperty.Name] = jsonKeyValue;
        }
    }

    private ParsedArrayValue ConvertArrayValue(IRootParsedValue rootParsedValue, IParsedValue parentJsonValue, IJsonKeyValue? jsonKeyValue, JArray jArray)
    {
        var parsedArrayValue = new ParsedArrayValue(_parsedJsonVisitor, rootParsedValue, parentJsonValue, jsonKeyValue)
        {
            LineInfo = GetJsonLineInfo(jArray, 0)
        };

        PopulateParsedArrayValue(rootParsedValue, parsedArrayValue, jArray.Children());
        return parsedArrayValue;
    }

    private void PopulateParsedArrayValue(IRootParsedValue rootParsedValue, IParsedArrayValue parsedArrayValue, JEnumerable<JToken> childrenTokens)
    {
        foreach (var arrayItem in childrenTokens)
            parsedArrayValue.AddValue(ConvertToParsedValue(rootParsedValue, parsedArrayValue, null, arrayItem));
    }

    private IParsedValue ConvertToParsedValue(IRootParsedValue rootParsedValue, IParsedValue parentJsonValue, IJsonKeyValue? jsonKeyValue, JToken jToken)
    {
        switch (jToken.Type)
        {
            case JTokenType.Boolean:
                return ConvertToSimpleParsedValueOrThrow(rootParsedValue, parentJsonValue, jsonKeyValue, jToken, 
                    jValue => jValue.Value?.ToString()!.ToLower()??"false", false);
            case JTokenType.Integer:
            case JTokenType.Float:
            case JTokenType.Undefined:
                return ConvertToSimpleParsedValueOrThrow(rootParsedValue, parentJsonValue, jsonKeyValue, jToken,
                    jValue => jValue.Value?.ToString()!,
                    false);
            case JTokenType.Null:
                return ConvertToSimpleParsedValueOrThrow(rootParsedValue, parentJsonValue, jsonKeyValue, jToken,
                    jValue => "null",
                    false);

            case JTokenType.String:
            case JTokenType.Date:
            case JTokenType.Bytes:
            case JTokenType.Uri:
            case JTokenType.Guid:
            case JTokenType.TimeSpan:
                return ConvertToSimpleParsedValueOrThrow(rootParsedValue, parentJsonValue, jsonKeyValue, jToken, 
                    jValue => jValue.Value?.ToString()!, true);

            case JTokenType.Object:
            {
                var jObject = ConvertToJTokenTypeOrThrowException<JObject>(jToken);
                return ConvertParsedJson(rootParsedValue, parentJsonValue, jsonKeyValue, jObject);
            }

            case JTokenType.Array:
            {
                return ConvertArrayValue(rootParsedValue, parentJsonValue, jsonKeyValue, ConvertToJTokenTypeOrThrowException<JArray>(jToken));
            }

            case JTokenType.Property:
            case JTokenType.Constructor:
            case JTokenType.Comment:
            case JTokenType.None:
            case JTokenType.Raw:
            default:
                throw new NotImplementedException($"Type [{jToken.Type}] is not implemented.");
        }
    }

    private IJsonLineInfo? GetNewtonSoftJsonLineInfo(JToken jToken)
    {
        return jToken is IJsonLineInfo jsonLineInfo && jsonLineInfo.HasLineInfo() ? jsonLineInfo : null;
    }

    private JsonLineInfo? GetJsonLineInfo(JToken jToken, IParsedSimpleValue parsedSimpleValue)
    {
        if (parsedSimpleValue.Value == null)
            return null;

        if (parsedSimpleValue.IsString)
        {
            // The value of Newtonsoft IJsonLineInfo.LinePosition is the index before the apostrophe.
            // For example in case of ("copyFields": "parent:AppSettings")
            // the value of IJsonLineInfo.LinePosition is 34.
            return GetJsonLineInfo(jToken,
                // Get the index of value without apostrophes.
                -parsedSimpleValue.Value.Length);
        }

        // For non-strings the value of Newtonsoft IJsonLineInfo.LinePosition is the index of the last digit.
        // For example in case of ("DefaultInt1": 175)
        // the value of IJsonLineInfo.LinePosition is 17.
        return GetJsonLineInfo(jToken,
            // End of value text
            1
            // Value index
            - parsedSimpleValue.Value.Length);
    }

    private JsonLineInfo? GetJsonLineInfo(JToken jToken, int adjustment)
    {
        var newtonSoftLineInfo = GetNewtonSoftJsonLineInfo(jToken);

        if (newtonSoftLineInfo == null)
            return null;

        return new JsonLineInfo(newtonSoftLineInfo.LineNumber, newtonSoftLineInfo.LinePosition + adjustment);
    }

    private T ConvertToJTokenTypeOrThrowException<T>(JToken jToken) where T : JToken
    {
        if (jToken is not T convertedValue)
            throw new ApplicationException($"Filed to convert [{nameof(jToken)}] to [{typeof(T)}]");

        return convertedValue;
    }

    private IParsedValue ConvertToSimpleParsedValueOrThrow(IRootParsedValue rootParsedValue, IParsedValue parentJsonValue, 
        IJsonKeyValue? jsonKeyValue, JToken jToken, Func<JValue, string> getSimpleValueAsString, bool isString)
    {
        var jValue = ConvertToJTokenTypeOrThrowException<JValue>(jToken);

        var parsedSimpleValue = jValue.Value == null ?
            new ParsedSimpleValue(rootParsedValue, parentJsonValue, jsonKeyValue, null, false) :
            new ParsedSimpleValue(rootParsedValue, parentJsonValue, jsonKeyValue, getSimpleValueAsString(jValue), isString);

        parsedSimpleValue.LineInfo = GetJsonLineInfo(jToken, parsedSimpleValue);
        return parsedSimpleValue;
    }
}