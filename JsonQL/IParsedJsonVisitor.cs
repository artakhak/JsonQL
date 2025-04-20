using JsonQL.JsonObjects;

namespace JsonQL;

public delegate bool VisitJsonValueDelegate(IParsedValue parsedValue);

public interface IParsedJsonVisitor
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="parsedValue">json value to recursively visit.</param>
    /// <param name="visitJsonValue"></param>
    /// <exception cref="ArgumentException">Throws this exception if <param name="parsedValue"></param> is not
    /// an instance of <see cref="IParsedJson"/> or  <see cref="IParsedArrayValue"/>.
    /// </exception>
    void Visit(IParsedValue parsedValue, VisitJsonValueDelegate visitJsonValue);
}

public class ParsedJsonVisitor : IParsedJsonVisitor
{
    public void Visit(IParsedValue parsedValue, VisitJsonValueDelegate visitJsonValue)
    {
        if (parsedValue is IParsedArrayValue parsedArrayValue)
            VisitValue(parsedArrayValue, visitJsonValue);
        else if (parsedValue is IParsedJson parsedJson)
            VisitParsedJson(parsedJson, visitJsonValue);
        else if (parsedValue is IParsedSimpleValue parsedSimpleValue)
            VisitValue(parsedSimpleValue, visitJsonValue);
        else
            throw new ArgumentException($"Expected an instance of [{typeof(IParsedArrayValue).FullName}] or [{typeof(IParsedJson).FullName}].", nameof(parsedValue));
    }
   
    private bool VisitParsedJson(IParsedJson parsedJson, VisitJsonValueDelegate visitJsonValue)
    {
        foreach (var keyValue in parsedJson.KeyValues)
        {
            if (!VisitValue(keyValue.Value, visitJsonValue))
                return false;
        }

        return true;
    }

    private bool VisitValue(IParsedValue parsedValue, VisitJsonValueDelegate visitJsonValue)
    {
        if (!visitJsonValue(parsedValue))
            return false;

        if (parsedValue is IParsedArrayValue parsedArrayValue)
        {
            for (var i = 0; i < parsedArrayValue.Values.Count; ++i)
            {
                if (!VisitValue(parsedArrayValue.Values[i], visitJsonValue))
                    return false;
            }

            return true;
        }

        if (parsedValue is IParsedJson parsedJson)
        {
            if (!VisitParsedJson(parsedJson, visitJsonValue))
                return false;
        }

        return true;
    }
}