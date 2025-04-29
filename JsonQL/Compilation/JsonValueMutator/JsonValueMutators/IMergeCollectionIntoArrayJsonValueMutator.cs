using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Represents a mutator responsible for merging a collection of JSON objects into an array during
/// the JSON value mutation process.
/// This interface is used in scenarios where multiple JSON values must be aggregated
/// into a single JSON array structure.
/// </summary>
public interface IMergeCollectionIntoArrayJsonValueMutator : IJsonValueMutator
{

}

/// <summary>
/// Represents a concrete implementation of a JSON value mutator responsible for merging
/// a collection of JSON objects or values into an array during JSON handling processes.
/// This class facilitates the transformation or aggregation of JSON elements
/// into a unified array structure, ensuring that the mutator operates
/// exclusively within a valid array context.
/// </summary>
public class MergeCollectionIntoArrayJsonValueMutator : CalculatedJsonValueMutatorAbstr, IMergeCollectionIntoArrayJsonValueMutator
{
    private readonly IParsedValueCopy _parsedValueCopy;

    /// <summary>
    /// Represents a JSON value mutator that merges a collection into an array within a JSON structure.
    /// This mutator utilizes a combination of parsed values, functions, visitors, and formatting logic
    /// to produce a calculated JSON value.
    /// </summary>
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
            ThreadStaticLoggingContext.Context.ErrorFormat("Value not found in array. Line info: [{0}]. Value Id: [{1}]",
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