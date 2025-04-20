namespace JsonQL.JsonObjects;

public interface IParsedSimpleValue : IParsedValue
{
    string? Value { get; set; }

    /// <summary>
    /// If the value is true, the serialized value will be enclosed onm apostrophes. Example: "BusName": "DefaultMessageHandler".
    /// Otherwise, the value will not be enclosed in  apostrophes. Example: "RebusNumberOfWorkers": 10.
    /// </summary>
    public bool IsString { get; }
}

public class ParsedSimpleValue : ParsedValueAbstr, IParsedSimpleValue
{
    public ParsedSimpleValue(IRootParsedValue rootParsedValue, IParsedValue? parentJsonValue, IJsonKeyValue? jsonKeyValue, string? value, bool isString): 
        base(parentJsonValue, jsonKeyValue)
    {
        RootParsedValue = rootParsedValue;
        Value = value;
        IsString = isString;
    }

    /// <inheritdoc />
    public string? Value { get; set; }

    /// <inheritdoc />
    public bool IsString { get; }

    /// <inheritdoc />
    public override IRootParsedValue RootParsedValue { get; }
}