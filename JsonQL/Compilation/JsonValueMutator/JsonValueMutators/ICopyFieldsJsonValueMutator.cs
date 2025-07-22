// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.JsonObjects;
using JsonQL.JsonObjects.JsonPath;

namespace JsonQL.Compilation.JsonValueMutator.JsonValueMutators;

/// <summary>
/// Represents a JSON value mutator that specifically handles field copying operations within a JSON object.
/// This interface defines the contract for implementing functionality that allows fields to be copied
/// from one location to another within the context of JSON value mutation.
/// </summary>
public interface ICopyFieldsJsonValueMutator : IJsonValueMutator
{

}

/// <summary>
/// Represents a JSON value mutator responsible for performing field copying operations within a JSON object.
/// This class facilitates copying fields from a source location to a target location within the JSON structure,
/// while managing path-specific mutations and handling any errors that may occur during the operation.
/// </summary>
public class CopyFieldsJsonValueMutator : PathJsonValueMutatorAbstr, ICopyFieldsJsonValueMutator
{
    private readonly IParsedValueCopy _parsedValueCopy;

    /// <summary>
    /// Represents a class that extends <see cref="PathJsonValueMutatorAbstr"/> and implements
    /// <see cref="ICopyFieldsJsonValueMutator"/> to support copying JSON fields
    /// from a source specified by path to a target within a JSON object structure.
    /// </summary>
    /// <remarks>
    /// This class is typically used to perform field-level transformations or modifications
    /// within JSON objects, ensuring that specific fields are copied or moved to predefined locations
    /// as per the instructions provided by its dependencies.
    /// </remarks>
    public CopyFieldsJsonValueMutator(IParsedSimpleValue parsedSimpleValue, IJsonValuePathJsonFunction jsonValuePathJsonFunction,
        IParsedValueCopy parsedValueCopy) : base(parsedSimpleValue, jsonValuePathJsonFunction)
    {
        _parsedValueCopy = parsedValueCopy;
    }

    /// <inheritdoc />
    protected override void MutateValue(IParsedSimpleValue currentParsedSimpleValueWithPath,
        IReadOnlyList<IParsedValue> referencedParsedValues,
        List<IJsonObjectParseError> errors)
    {
        if (currentParsedSimpleValueWithPath.JsonKeyValue == null || currentParsedSimpleValueWithPath.ParentJsonValue is not IParsedJson parentParsedJson)
        {
            const char dollarSign = '$';
            const char openingBrace = '{';
            const char closingBrace = '}';
            var errorMessage = $@"Function [{JsonMutatorKeywords.MergedJsonObjectFields}] can be used only as a value in json key/value pairs. Example: {openingBrace}""Company"":""Company1"", ""CopiedFields"": ""{dollarSign}(object(Company.Employees[0]))""{closingBrace}.";

            errors.Add(new JsonObjectParseError($"{ParseErrorsConstants.InvalidSymbol}, {errorMessage}",
                currentParsedSimpleValueWithPath.LineInfo));
            return;
        }

        void RemoveCopyFieldsKeyOnError()
        {
            _ = parentParsedJson.TryRemoveKey(currentParsedSimpleValueWithPath.JsonKeyValue.Key, out _);
        }
        
        if (referencedParsedValues.Count != 1)
        {
            ThreadStaticLoggingContext.Context.WarnFormat("The value referenced in [{0}] value used at path [{1}] should be a single json object (e.g., collection of key/value pairs). Actual number of items is [{2}].",
                JsonMutatorKeywords.MergedJsonObjectFields, currentParsedSimpleValueWithPath.GetPath(), referencedParsedValues.Count);

            RemoveCopyFieldsKeyOnError();
            return;
        }

        var referencedParsedValue = referencedParsedValues[0];

        if (JsonPathHelpers.Compare(currentParsedSimpleValueWithPath.GetPath(), referencedParsedValue.GetPath()) == JsonPathComparisonResult.Child)
        {
            errors.Add(new JsonObjectParseError("Parent json value cannot be referenced in this context.", this.JsonValuePathJsonFunction.JsonValuePath.Path[^1].LineInfo));
            return;
        }

        if (referencedParsedValue is not IParsedJson referencedParsedJson)
        {
            ThreadStaticLoggingContext.Context.WarnFormat("The value referenced in [{0}] value used at path [{1}] should be a json object (e.g., collection of key/value pairs).",
                JsonMutatorKeywords.MergedJsonObjectFields, currentParsedSimpleValueWithPath.GetPath());

            RemoveCopyFieldsKeyOnError();
            return;
        }

        var keyIndex = -1;
        var keysToRemove = new List<string>();
        var keysNotToAdd = new HashSet<string>(StringComparer.Ordinal);

        for (var i = 0; i < parentParsedJson.KeyValues.Count; ++i)
        {
            var keyValue = parentParsedJson.KeyValues[i];

            if (keyIndex < 0)
            {
                if (keyValue == currentParsedSimpleValueWithPath.JsonKeyValue)
                    keyIndex = i;
                else if (referencedParsedJson.TryGetJsonKeyValue(keyValue.Key, out _))
                    keysToRemove.Add(keyValue.Key);
            }
            else
            {
                keysNotToAdd.Add(keyValue.Key);
            }
        }

        foreach (var keyToRemove in keysToRemove)
        {
            parentParsedJson.TryRemoveKey(keyToRemove, out _);
        }

        for (var i = 0; i < parentParsedJson.KeyValues.Count; ++i)
        {
            if (parentParsedJson.KeyValues[i] == currentParsedSimpleValueWithPath.JsonKeyValue)
            {
                keyIndex = i;
                break;
            }
        }

        RemoveCopyFieldsKeyOnError();

        foreach (var jsonKeyValue in referencedParsedJson.KeyValues.Where(x => !keysNotToAdd.Contains(x.Key)))
        {
            var copiedJsonKeyValue = new JsonKeyValue(jsonKeyValue.Key, parentParsedJson);
            copiedJsonKeyValue.Value = _parsedValueCopy.CopyWithNewParent(jsonKeyValue.Value, parentParsedJson, copiedJsonKeyValue);
            parentParsedJson[copiedJsonKeyValue.Key, keyIndex] = copiedJsonKeyValue;
            ++keyIndex;
        }
    }
}
