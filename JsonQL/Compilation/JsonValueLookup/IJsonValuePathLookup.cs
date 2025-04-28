using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonValueLookup.JsonValuePathElements;
using JsonQL.Compilation.UniversalExpressionParserJsonQL;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueLookup;

public interface IJsonValuePathLookup
{
    /// <summary>
    /// Looks up a JSON value in one of JSON value in <param name="rootParsedValue"></param> and if value not found, looks up
    /// JSON value in parsed parent JSON files in <param name="compiledParentRootParsedValues"></param>.
    /// The value will be looked up only in <param name="compiledParentRootParsedValues"></param> if the path starts with 'parent', and
    /// only in <param name="rootParsedValue"></param> if the path starts with 'this'.
    /// </summary>
    /// <param name="rootParsedValue">Root parsed value.</param>
    /// <param name="compiledParentRootParsedValues">Parent root parsed value.</param>
    /// <param name="jsonFunctionValueEvaluationContext">Context.</param>
    /// <param name="jsonValuePath">Path</param>
    /// <returns>
    /// If the returned value has no error in <see cref="IParseResult{TValue}.Errors"/> or if the value of <see cref="IParseResult{TValue}.Value"/> is null,
    /// then lookup didn't produce any results and no errors were reported.<br/>
    /// Otherwise, the result in <see cref="IParseResult{TValue}.Value"/> is either an instance  of<br/>
    /// <see cref="ICollectionJsonValuePathLookupResult"/> which contains a collection of <see cref="IParsedValue"/> values, or is an instance of<br/>
    /// <see cref="ISingleItemJsonValuePathLookupResult"/> which contains a single <see cref="IParsedValue"/> value.
    /// </returns>
    IParseResult<IJsonValuePathLookupResult> LookupJsonValue(
        IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionValueEvaluationContext jsonFunctionValueEvaluationContext,
        IJsonValuePath jsonValuePath);

    /// <summary>
    /// Looks up a JSON value in <param name="parentParsedValue"></param>.
    /// </summary>
    /// <param name="parentParsedValue">Parent parsed value where to look up the value.</param>
    /// <param name="jsonFunctionValueEvaluationContext">Context.</param>
    /// <param name="jsonValuePath">Path</param>
    /// <returns>
    /// If the returned value has no error in <see cref="IParseResult{TValue}.Errors"/> or if the value of <see cref="IParseResult{TValue}.Value"/> is null,
    /// then lookup didn't produce any results and no errors were reported.<br/>
    /// Otherwise, the result in <see cref="IParseResult{TValue}.Value"/> is either an instance  of<br/>
    /// <see cref="ICollectionJsonValuePathLookupResult"/> which contains a collection of <see cref="IParsedValue"/> values, or is an instance of<br/>
    /// <see cref="ISingleItemJsonValuePathLookupResult"/> which contains a single <see cref="IParsedValue"/> value.
    /// </returns>
    IParseResult<IJsonValuePathLookupResult> LookupJsonValue(IParsedValue parentParsedValue,
        IJsonFunctionValueEvaluationContext jsonFunctionValueEvaluationContext,
        IJsonValuePath jsonValuePath);
}

/// <inheritdoc />
public class JsonValuePathLookup : IJsonValuePathLookup
{
    private const string PathLookupFailedErrorMessageTemplate = "Lookup for json path [{0}] failed. Error: {1}";

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> LookupJsonValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionValueEvaluationContext jsonFunctionValueEvaluationContext,
        IJsonValuePath jsonValuePath)
    {
        if (jsonValuePath.Path.Count == 0)
        {
            return new ParseResult<IJsonValuePathLookupResult>(
                CollectionExpressionHelpers.Create(
                new JsonObjectParseError(
                    string.Format(PathLookupFailedErrorMessageTemplate, jsonValuePath, "The path is empty."), jsonValuePath.LineInfo)
            ));
        }

        IReadOnlyList<IRootParsedValue>? jsonFilesToSearch = null;

        var currentPathElementIndex = 0;

        if (jsonValuePath.Path[currentPathElementIndex] is IJsonValuePropertyNamePathElement firstJsonValuePropertyNamePathElement)
        {
            switch (firstJsonValuePropertyNamePathElement.Name)
            {
                case JsonValuePathFunctionNames.ParentFile:
                    {
                        // If the path has only "parent", lets just return te root object in first parent
                        if (jsonValuePath.Path.Count == 1)
                        {
                            if (compiledParentRootParsedValues.Count > 0)
                                return new ParseResult<IJsonValuePathLookupResult>(
                                    SingleItemJsonValuePathLookupResult.CreateForValidPath(compiledParentRootParsedValues[0].RootParsedValue));

                            return new ParseResult<IJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
                        }

                        // Lets skip 'parent'
                        ++currentPathElementIndex;

                        if (jsonValuePath.Path[currentPathElementIndex] is IJsonValuePropertyNamePathElement)
                        {
                            // If we have path like parent.Array1.Where(x => 10)..., we will search all parents for Array1.
                            jsonFilesToSearch = compiledParentRootParsedValues;
                        }
                        else
                        {
                            // If we have path like parent.Where(x => 10), we will always search the first parent only.
                            jsonFilesToSearch = new List<IRootParsedValue>
                            {
                                compiledParentRootParsedValues[0]
                            };
                        }

                        break;
                    }

                case JsonValuePathFunctionNames.ThisJson:
                    {
                        if (currentPathElementIndex == jsonValuePath.Path.Count - 1)
                            return new ParseResult<IJsonValuePathLookupResult>(
                                CollectionExpressionHelpers.Create(
                                    new JsonObjectParseError(
                                        string.Format(PathLookupFailedErrorMessageTemplate, jsonValuePath,
                                            $"Expected a path component after '{firstJsonValuePropertyNamePathElement.Name}' path component. Example: '{JsonValuePathFunctionNames.ParentFile}{JsonOperatorNames.JsonPathSeparator}Object1'."),
                                        jsonValuePath.LineInfo)
                                ));

                        jsonFilesToSearch = new List<IRootParsedValue>
                            {
                                rootParsedValue
                            };

                        // Lets skip 'this'
                        ++currentPathElementIndex;
                        break;
                    }
            }
        }
        else if ((jsonValuePath.Path[currentPathElementIndex] is IJsonValueCollectionItemsSelectorPathElement ||
                  jsonValuePath.Path[currentPathElementIndex] is IJsonArrayIndexesPathElement)  &&
                 rootParsedValue is not IRootParsedArrayValue)
        {
            foreach (var compiledParentRootParsedValue in compiledParentRootParsedValues)
            {
                if (compiledParentRootParsedValue is IRootParsedArrayValue parentRootParsedArrayValue)
                {
                    jsonFilesToSearch = new List<IRootParsedValue>
                    {
                        parentRootParsedArrayValue
                    };

                    break;
                }
            }
        }

        if (jsonFilesToSearch == null)
        {
            var jsonFilesList = new List<IRootParsedValue>(compiledParentRootParsedValues.Count + 1);
            jsonFilesList.Add(rootParsedValue);

            if (compiledParentRootParsedValues.Count > 0 && jsonValuePath.Path[currentPathElementIndex] is IJsonValuePropertyNamePathElement)
            {
                // If we have a path like parent.Array1.Where(x => 10)..., we will search all parents for Array1.
                // Otherwise, we will search only current json.
                jsonFilesList.AddRange(compiledParentRootParsedValues);
            }
            
            jsonFilesToSearch = jsonFilesList;
        }

        foreach (var parentRootParsedValue in jsonFilesToSearch)
        {
            var lookupResult = TryLookupValueInParent(rootParsedValue, compiledParentRootParsedValues,
                SingleItemJsonValuePathLookupResult.CreateForValidPath(parentRootParsedValue), jsonFunctionValueEvaluationContext, jsonValuePath, currentPathElementIndex);

            if (lookupResult.Errors.Count > 0)
                return lookupResult;

            if (!lookupResult.Value?.HasValue ?? false)
                continue;

            if (currentPathElementIndex == jsonValuePath.Path.Count - 1)
                return lookupResult;

            return LookupJsonValueLocal(rootParsedValue, compiledParentRootParsedValues,
                lookupResult.Value!, jsonFunctionValueEvaluationContext, jsonValuePath,
                // currentLookedUpValue was looked up using index currentPathElementIndex. Lets move to next index
                currentPathElementIndex + 1);
        }

        if (jsonValuePath.Path[currentPathElementIndex] is IJsonValueCollectionItemsSelectorPathElement jsonValueCollectionItemsSelectorPathElement2 &&
            !jsonValueCollectionItemsSelectorPathElement2.SelectsSingleItem)
            return new ParseResult<IJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(Array.Empty<IParsedValue>()));

        return new ParseResult<IJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
    }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> LookupJsonValue(IParsedValue parentParsedValue,
        IJsonFunctionValueEvaluationContext jsonFunctionValueEvaluationContext,
        IJsonValuePath jsonValuePath)
    {
        if (jsonValuePath.Path.Count == 0)
        {
            return new ParseResult<IJsonValuePathLookupResult>(
                    CollectionExpressionHelpers.Create(
                new JsonObjectParseError(
                    string.Format(PathLookupFailedErrorMessageTemplate, jsonValuePath, "The path is empty."), jsonValuePath.LineInfo)
            ));
        }

        return LookupJsonValueLocal(parentParsedValue.RootParsedValue, Array.Empty<IRootParsedValue>(),
            SingleItemJsonValuePathLookupResult.CreateForValidPath(parentParsedValue), jsonFunctionValueEvaluationContext, jsonValuePath, 0);
    }

    private IParseResult<IJsonValuePathLookupResult> LookupJsonValueLocal(
        IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonValuePathLookupResult parentResult,
        IJsonFunctionValueEvaluationContext jsonFunctionValueEvaluationContext,
        IJsonValuePath jsonValuePath,
        int currentPathElementIndex)
    {
        var currentResult = parentResult;

        while (currentPathElementIndex < jsonValuePath.Path.Count)
        {
            var currentJsonValuePathElement = jsonValuePath.Path[currentPathElementIndex];

            if (jsonValuePath.Path[currentPathElementIndex] is IJsonValuePropertyNamePathElement jsonValuePropertyNamePathElement2 &&
                string.Equals(jsonValuePropertyNamePathElement2.Name, JsonValuePathFunctionNames.ParentFile, StringComparison.Ordinal))
            {
                return new ParseResult<IJsonValuePathLookupResult>(
                    CollectionExpressionHelpers.Create(
                    new JsonObjectParseError(
                        string.Format(PathLookupFailedErrorMessageTemplate, jsonValuePath, $"{JsonValuePathFunctionNames.ParentFile} component should be the first component in path."),
                        currentJsonValuePathElement.LineInfo)
                ));
            }

            var lookedUpValuesResult = TryLookupValueInParent(rootParsedValue, compiledParentRootParsedValues, currentResult, jsonFunctionValueEvaluationContext, jsonValuePath, currentPathElementIndex);

            if (lookedUpValuesResult.Errors.Count > 0)
                return lookedUpValuesResult;

            if (!(lookedUpValuesResult.Value?.HasValue ?? false))
            {
                return lookedUpValuesResult;
            }

            currentResult = lookedUpValuesResult.Value;

            ++currentPathElementIndex;
        }

        if (currentResult == null)
            return new ParseResult<IJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());

        return new ParseResult<IJsonValuePathLookupResult>(currentResult);
    }

    private IParseResult<IJsonValuePathLookupResult> TryLookupValueInParent(
        IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonValuePathLookupResult parentResult,
        IJsonFunctionValueEvaluationContext jsonFunctionValueEvaluationContext,
        IJsonValuePath jsonValuePath, int currentPathElementIndex)
    {
        if (currentPathElementIndex >= jsonValuePath.Path.Count)
        {
            var errorMessage = $"The value of index [{currentPathElementIndex}] should smaller than [{jsonValuePath.Path.Count}].";

            ThreadStaticLogging.Log.Error(errorMessage);

            return new ParseResult<IJsonValuePathLookupResult>(
                    CollectionExpressionHelpers.Create(
                new JsonObjectParseError(
                    string.Format(PathLookupFailedErrorMessageTemplate, jsonValuePath, errorMessage),
                    jsonValuePath.Path[^1].LineInfo)
            ));
        }

        if (!parentResult.HasValue)
            return new ParseResult<IJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());

        var currentJsonValuePathElement = jsonValuePath.Path[currentPathElementIndex];

        IParseResult<IReadOnlyList<IParsedValue>>? collectionResult;
        if (currentJsonValuePathElement is IJsonValueCollectionItemsSelectorPathElement jsonValueCollectionItemsSelectorPathElement)
        {
            collectionResult = parentResult.GetResultAsParsedValuesList(true, currentJsonValuePathElement.LineInfo);

            if (collectionResult.Errors.Count > 0)
                return new ParseResult<IJsonValuePathLookupResult>(collectionResult.Errors);

            if (collectionResult.Value == null)
                return new ParseResult<IJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(Array.Empty<IParsedValue>()));

            var resolvesVariableValue = jsonValueCollectionItemsSelectorPathElement as IResolvesVariableValue;

            try
            {
                if (resolvesVariableValue != null)
                    jsonFunctionValueEvaluationContext.VariablesManager.Register(resolvesVariableValue);

                return jsonValueCollectionItemsSelectorPathElement.Select(collectionResult.Value!,
                    rootParsedValue, compiledParentRootParsedValues);
            }
            finally
            {
                if (resolvesVariableValue != null)
                    jsonFunctionValueEvaluationContext.VariablesManager.UnRegister(resolvesVariableValue);
            }
        }

        collectionResult = parentResult.GetResultAsParsedValuesList(false, currentJsonValuePathElement.LineInfo);

        if (collectionResult.Errors.Count > 0)
            return new ParseResult<IJsonValuePathLookupResult>(collectionResult.Errors);

        if (collectionResult.Value == null || collectionResult.Value.Count == 0)
            return new ParseResult<IJsonValuePathLookupResult>(new CollectionJsonValuePathLookupResult(Array.Empty<IParsedValue>()));

        switch (currentJsonValuePathElement)
        {
            case IJsonValuePropertyNamePathElement jsonValuePropertyNamePathElement:

                if (collectionResult.Value[0] is IParsedJson parsedJson)
                {
                    return TryLookupValueInJsonObject(parsedJson, jsonValuePropertyNamePathElement);
                }

                break;

            case IJsonArrayIndexesPathElement jsonArrayIndexesPathElement:

                if (collectionResult.Value[0] is IParsedArrayValue parsedArrayValue)
                {
                    return TryLookupValuesInArrayByIndex(parsedArrayValue,
                        jsonValuePath, jsonArrayIndexesPathElement, currentPathElementIndex);
                }

                break;
        }

        return new ParseResult<IJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
    }

    private JsonValuePath GetSubPath(IJsonValuePath jsonValuePath, int numberOfComponents)
    {
        return new JsonValuePath(jsonValuePath.Path.Take(numberOfComponents).ToList());
    }

    private IParseResult<ISingleItemJsonValuePathLookupResult> TryLookupValueInJsonObject(IParsedJson parsedJson,
        IJsonValuePropertyNamePathElement jsonValuePropertyNamePathElement)
    {
        if (!parsedJson.TryGetJsonKeyValue(jsonValuePropertyNamePathElement.Name, out var jsonKeyValue))
            return new ParseResult<ISingleItemJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());

        return new ParseResult<ISingleItemJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForValidPath(jsonKeyValue.Value));
    }

    private IParseResult<ISingleItemJsonValuePathLookupResult> TryLookupValuesInArrayByIndex(
        IParsedArrayValue parenParsedArrayValue,
        IJsonValuePath jsonValuePath,
        IJsonArrayIndexesPathElement jsonArrayIndexesPathElement,
        int currentPathElementIndex)
    {
        IParsedValue currentParent = parenParsedArrayValue;

        for (var i = 0; i < jsonArrayIndexesPathElement.ArrayIndexes.Count; ++i)
        {
            var arrayIndexInfo = jsonArrayIndexesPathElement.ArrayIndexes[i];

            if (currentParent is not IParsedArrayValue currentParentArray)
            {
                ThreadStaticLogging.Log.DebugFormat(
                    "Parent json token at path [{0}] is not a json object. LineInfo: [{1}]",
                    GetSubPath(jsonValuePath, currentPathElementIndex + 1),
                    arrayIndexInfo.LineInfo!);

                return new ParseResult<ISingleItemJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
            }

            if (arrayIndexInfo.Index < 0 || arrayIndexInfo.Index >= currentParentArray.Values.Count)
            {
                ThreadStaticLogging.Log.DebugFormat(
                    "Index is out of bounds. Expected an index between [0] and [{0}]. Failed path: [{1}], Actual index: [{2}]. LineInfo: [{3}]",
                    currentParentArray.Values.Count - 1,
                    GetSubPath(jsonValuePath, currentPathElementIndex + 1),
                    arrayIndexInfo.Index,
                    arrayIndexInfo.LineInfo!);

                return new ParseResult<ISingleItemJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
            }

            var currentLookedUpItem = currentParentArray.Values[arrayIndexInfo.Index];

            if (i == jsonArrayIndexesPathElement.ArrayIndexes.Count - 1)
                return new ParseResult<ISingleItemJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForValidPath(currentLookedUpItem));

            currentParent = currentLookedUpItem;
            ++currentPathElementIndex;
        }

        return new ParseResult<ISingleItemJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForInvalidPath());
    }
}
