using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Represents a specialized mutator for JSON values that are calculated.
/// This interface extends the base contract provided by <see cref="IJsonValueMutator"/>
/// and is responsible for handling the mutation of JSON values that involve calculated properties or functions.
/// Implementations of this interface may process values, apply calculations, and mutate the resulting JSON structure accordingly.
/// </summary>
public interface ICalculatedValueJsonValueMutator : IJsonValueMutator
{
}

/// <summary>
/// Represents an implementation of <see cref="ICalculatedValueJsonValueMutator"/>, designed to handle the mutation of JSON values
/// that involve calculated properties or functions. This class extends the functionality provided by <see cref="CalculatedJsonValueMutatorAbstr"/>.
/// </summary>
/// <remarks>
/// This class processes JSON values and modifies their structure or content based on calculations or transformations.
/// It can handle scenarios involving parent JSON structures like arrays or objects, ensuring appropriate updates
/// based on the provided parsed values and functions. Errors encountered during the mutation process are collected and managed.
/// </remarks>
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