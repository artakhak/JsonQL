namespace JsonQL.JsonObjects;

public interface IParsedValueCopy
{
    IParsedValue CopyWithNewParent(IParsedValue parsedValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue);
}

public class ParsedValueCopy : IParsedValueCopy
{
    private readonly IParsedJsonVisitor _parsedJsonVisitor;

    public ParsedValueCopy(IParsedJsonVisitor parsedJsonVisitor)
    {
        _parsedJsonVisitor = parsedJsonVisitor;
    }

    public IParsedValue CopyWithNewParent(IParsedValue parsedValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue)
    {
        switch (parsedValue)
        {
            case IParsedJson parsedJson:
                return CopyParsedJson(parsedJson, parentParsedValue, jsonKeyValue);
            case IParsedArrayValue parsedArrayValue:
                return CopyParsedArrayValue(parsedArrayValue, parentParsedValue, jsonKeyValue);
            case IParsedSimpleValue parsedSimpleValue:
                return CopyParsedSimpleValue(parsedSimpleValue, parentParsedValue, jsonKeyValue);
            default:
                throw new ApplicationException($"Failed to copy object of type [{parsedValue.GetType().FullName}].");
        }
    }

    private IParsedJson CopyParsedJson(IParsedJson parsedJson, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue)
    {
        var copiedParsedJson = new ParsedJson(_parsedJsonVisitor, parentParsedValue.RootParsedValue, parentParsedValue, jsonKeyValue);

        foreach (var keyValue in parsedJson.KeyValues)
        {
            var copiedKeyValue = new JsonKeyValue(keyValue.Key, copiedParsedJson);
            copiedKeyValue.Value = CopyWithNewParent(keyValue.Value, copiedParsedJson, copiedKeyValue);
            copiedParsedJson[keyValue.Key] = copiedKeyValue;
        }

        return copiedParsedJson;
    }

    private IParsedArrayValue CopyParsedArrayValue(IParsedArrayValue parsedArrayValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue)
    {
        var copiedParsedArrayValue = new ParsedArrayValue(_parsedJsonVisitor, parentParsedValue.RootParsedValue, parentParsedValue, jsonKeyValue);

        foreach (var arrayItem in parsedArrayValue.Values)
        {
            var copiedValue = CopyWithNewParent(arrayItem, copiedParsedArrayValue, null);
            copiedParsedArrayValue.AddValue(copiedValue);
        }

        return copiedParsedArrayValue;
    }

    private IParsedSimpleValue CopyParsedSimpleValue(IParsedSimpleValue parsedSimpleValue, IParsedValue parentParsedValue, IJsonKeyValue? jsonKeyValue)
    {
        return new ParsedSimpleValue(parentParsedValue.RootParsedValue, parentParsedValue, jsonKeyValue,
            parsedSimpleValue.Value, parsedSimpleValue.IsString);
    }
}