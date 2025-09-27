using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.JsonObjects;

/// <inheritdoc />
public class ParsedValueCopy : IParsedValueCopy
{
    private readonly IParsedJsonVisitor _parsedJsonVisitor;

    /// <summary>
    /// Provides functionality to create copies of <see cref="IParsedValue"/> implementations while associating them with new parent values and optional key-value pairs.
    /// </summary>
    public ParsedValueCopy(IParsedJsonVisitor parsedJsonVisitor)
    {
        _parsedJsonVisitor = parsedJsonVisitor;
    }

    /// <inheritdoc />
    public IParsedValue CopyWithNewParent(IParsedValue parsedValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue)
    {
        var pathInReferencedJson = parsedValue.PathInReferencedJson ?? parsedValue.GetPath();

        switch (parsedValue)
        {
            case IParsedJson parsedJson:
                return CopyParsedJson(parsedJson, parentParsedValue, jsonKeyValue, pathInReferencedJson);
            case IParsedArrayValue parsedArrayValue:
                return CopyParsedArrayValue(parsedArrayValue, parentParsedValue, jsonKeyValue, pathInReferencedJson);
            case IParsedSimpleValue parsedSimpleValue:
                return CopyParsedSimpleValue(parsedSimpleValue, parentParsedValue, jsonKeyValue, pathInReferencedJson);
            default:
                throw new ApplicationException($"Failed to copy object of type [{parsedValue.GetType().FullName}].");
        }
    }

    private IParsedJson CopyParsedJson(IParsedJson parsedJson, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue, IJsonPath? pathInReferencedJson)
    {
        var copiedParsedJson = new ParsedJson(_parsedJsonVisitor, parentParsedValue.RootParsedValue, parentParsedValue, jsonKeyValue, pathInReferencedJson);

        foreach (var keyValue in parsedJson.KeyValues)
        {
            var copiedKeyValue = new JsonKeyValue(keyValue.Key, copiedParsedJson);
            copiedKeyValue.Value = CopyWithNewParent(keyValue.Value, copiedParsedJson, copiedKeyValue);
            copiedParsedJson[keyValue.Key] = copiedKeyValue;
        }

        return copiedParsedJson;
    }

    private IParsedArrayValue CopyParsedArrayValue(IParsedArrayValue parsedArrayValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue, 
        IJsonPath? pathInReferencedJson)
    {
        var copiedParsedArrayValue = new ParsedArrayValue(_parsedJsonVisitor, parentParsedValue.RootParsedValue, parentParsedValue, jsonKeyValue,
            pathInReferencedJson);

        foreach (var arrayItem in parsedArrayValue.Values)
        {
            var copiedValue = CopyWithNewParent(arrayItem, copiedParsedArrayValue, null);
            copiedParsedArrayValue.AddValue(copiedValue);
        }

        return copiedParsedArrayValue;
    }

    private IParsedSimpleValue CopyParsedSimpleValue(IParsedSimpleValue parsedSimpleValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue,
        IJsonPath? pathInReferencedJson)
    {
        return new ParsedSimpleValue(parentParsedValue.RootParsedValue, parentParsedValue, jsonKeyValue, pathInReferencedJson,
            parsedSimpleValue.Value, parsedSimpleValue.IsString);
    }
}