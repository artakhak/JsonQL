using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonFunction;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

public interface ICalculatedValueJsonValueMutator : IJsonValueMutator
{
}

public class CalculatedValueJsonValueMutator : CalculatedJsonValueMutatorAbstr, ICalculatedValueJsonValueMutator
{
    private readonly IParsedValueCopy _parsedValueCopy;

    public CalculatedValueJsonValueMutator(IParsedSimpleValue parsedSimpleValue, IJsonFunction jsonFunction,
        IParsedValueCopy parsedValueCopy, IParsedJsonVisitor parsedJsonVisitor, IStringFormatter stringFormatter) : base(parsedSimpleValue, jsonFunction, parsedJsonVisitor, stringFormatter)
    {
        _parsedValueCopy = parsedValueCopy;
    }

    protected override void MutateValue(IParsedSimpleValue currentParsedSimpleValueWithPath, IParsedValue referencedParsedValue, List<IJsonObjectParseError> errors)
    {
        if (ParsedSimpleValue.ParentJsonValue is IParsedJson parsedJson)
        {
            ReplaceParsedJsonValue(currentParsedSimpleValueWithPath, referencedParsedValue, parsedJson, errors);
        }
        else if (ParsedSimpleValue.ParentJsonValue is IParsedArrayValue parsedArrayValue)
        {
            MergeIntoParentArray(currentParsedSimpleValueWithPath, referencedParsedValue, parsedArrayValue, errors);
        }
    }

    private void ReplaceParsedJsonValue(IParsedSimpleValue currentParsedSimpleValueWithPath, IParsedValue referencedParsedValue, IParsedJson parsedJson, List<IJsonObjectParseError> errors)
    {
        if (ParsedSimpleValue.JsonKeyValue == null)
        {
            // This should never happen. TODO: improve the error message
            errors.Add(new JsonObjectParseError("Invalid state parsed json value",
                currentParsedSimpleValueWithPath.LineInfo));
            return;
        }

        int indexOfKey = -1;
        for (var i = 0; i < parsedJson.KeyValues.Count; ++i)
        {
            if (parsedJson.KeyValues[i].Key == ParsedSimpleValue.JsonKeyValue.Key)
            {
                indexOfKey = i;
                break;
            }
        }

        if (indexOfKey < 0)
        {
            LogHelper.Context.Log.ErrorFormat("Key [{0}] not found.", ParsedSimpleValue.JsonKeyValue.Key);

            errors.Add(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol, currentParsedSimpleValueWithPath.LineInfo));
            return;
        }

        var jsonKeyValue = new JsonKeyValue(ParsedSimpleValue.JsonKeyValue.Key, parsedJson);
        jsonKeyValue.Value = _parsedValueCopy.CopyWithNewParent(referencedParsedValue, parsedJson, jsonKeyValue);
        parsedJson[ParsedSimpleValue.JsonKeyValue.Key, indexOfKey] = jsonKeyValue;
    }

    private void MergeIntoParentArray(IParsedSimpleValue currentParsedSimpleValueWithPath, IParsedValue referencedParsedValue, IParsedArrayValue parsedArrayValue, List<IJsonObjectParseError> errors)
    {
        if (!parsedArrayValue.TryGetValueIndex(currentParsedSimpleValueWithPath.Id, out var insertionIndex))
        {
            LogHelper.Context.Log.ErrorFormat("Value not found in array. Line info: [{0}]. Value Id: [{1}]",
                currentParsedSimpleValueWithPath.LineInfo!, currentParsedSimpleValueWithPath.Id);

            errors.Add(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                currentParsedSimpleValueWithPath.LineInfo));
            return;
        }

        parsedArrayValue.RemoveValueAt(insertionIndex.Value);

        var copiedValue = _parsedValueCopy.CopyWithNewParent(referencedParsedValue, parsedArrayValue, null);
        parsedArrayValue.AddValueAt(insertionIndex.Value, copiedValue);
    }
}