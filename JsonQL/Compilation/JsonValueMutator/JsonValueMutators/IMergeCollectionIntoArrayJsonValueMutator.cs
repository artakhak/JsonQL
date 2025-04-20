using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonFunction;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

public interface IMergeCollectionIntoArrayJsonValueMutator : IJsonValueMutator
{

}

public class MergeCollectionIntoArrayJsonValueMutator : CalculatedJsonValueMutatorAbstr, IMergeCollectionIntoArrayJsonValueMutator
{
    private readonly IParsedValueCopy _parsedValueCopy;

    public MergeCollectionIntoArrayJsonValueMutator(IParsedSimpleValue parsedSimpleValue, 
        IJsonFunction jsonFunction, IParsedValueCopy parsedValueCopy,
        IParsedJsonVisitor parsedJsonVisitor, IStringFormatter stringFormatter) : base(parsedSimpleValue, jsonFunction, parsedJsonVisitor, stringFormatter)
    {
        _parsedValueCopy = parsedValueCopy;
    }

    /// <inheritdoc />
    protected override void MutateValue(IParsedSimpleValue currentParsedSimpleValueWithPath, IParsedValue referencedParsedValue, List<IJsonObjectParseError> errors)
    {
        if (currentParsedSimpleValueWithPath.ParentJsonValue is not IParsedArrayValue parentArray)
        {
            errors.Add(new JsonObjectParseError($"[{JsonMutatorKeywords.MergedArrayItems}] function for copying array items into another array can be used ony inside arrays. Json object at [{currentParsedSimpleValueWithPath.GetPath()}] is not an array.",
                currentParsedSimpleValueWithPath.LineInfo));
            return;
        }

        IReadOnlyList<IParsedValue> mergedParsedValues;

        if (referencedParsedValue is IParsedArrayValue parsedArrayValue)
        {
            mergedParsedValues = parsedArrayValue.Values;
        }
        else
        {
            mergedParsedValues = new List<IParsedValue>
            {
                referencedParsedValue
            };
        }

        if (!parentArray.TryGetValueIndex(currentParsedSimpleValueWithPath.Id, out var insertionIndex))
        {
            LogHelper.Context.Log.ErrorFormat("Value not found in array. Line info: [{0}]. Value Id: [{1}]",
                currentParsedSimpleValueWithPath.LineInfo!, currentParsedSimpleValueWithPath.Id);

            errors.Add(new JsonObjectParseError(ParseErrorsConstants.InvalidSymbol,
                currentParsedSimpleValueWithPath.LineInfo));
            return;
        }

        parentArray.RemoveValueAt(insertionIndex.Value);

        foreach (var mergedParsedValue in mergedParsedValues)
        {
            var copiedValue = _parsedValueCopy.CopyWithNewParent(mergedParsedValue, parentArray, null);
            parentArray.AddValueAt(insertionIndex.Value, copiedValue);
            ++insertionIndex;
        }
    }
}