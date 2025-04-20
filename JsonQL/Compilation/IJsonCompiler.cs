using JsonQL.Compilation.JsonValueMutator;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation;

public interface IJsonCompiler
{
    ICompilationResult Compile(IJsonTextData jsonTextData);
}

/// <inheritdoc />
public class JsonCompiler : IJsonCompiler
{
    private readonly IJsonParser _jsonParser;
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IJsonValueMutatorFunctionTemplatesParser _jsonValueMutatorFunctionTemplatesParser;
    private readonly IJsonValueMutatorFactory _jsonValueMutatorFactory;

    private readonly ICompilationResultLogger _compilationResultLogger;

    public JsonCompiler(IJsonCompilerParameters parameters)
    {
        _jsonParser = parameters.JsonParser;
        _parsedJsonVisitor = parameters.ParsedJsonVisitor;
        _jsonValueMutatorFunctionTemplatesParser = parameters.JsonValueMutatorFunctionTemplatesParser;
        _jsonValueMutatorFactory = parameters.JsonValueMutatorFactory;
        _compilationResultLogger = parameters.CompilationResultLogger;
        LogAmbientContext.Context = parameters.Logger;
        DateTimeOperationsAmbientContext.Context = parameters.DateTimeOperations;
    }

    /// <inheritdoc />
    public ICompilationResult Compile(IJsonTextData jsonTextData)
    {
        var compilationResult = new CompilationResult();

        IJsonObjectData? jsonObjectData = Convert(jsonTextData, compilationResult.CompilationErrors);

        if (jsonObjectData == null)
            return compilationResult;

        Compile(jsonObjectData, compilationResult);

        if (compilationResult.CompilationErrors.Count == 0 &&
            compilationResult.CompiledJsonFiles.All(x => !string.Equals(x.TextIdentifier, jsonTextData.TextIdentifier, StringComparison.OrdinalIgnoreCase)))
            compilationResult.CompilationErrors.Add(new CompilationErrorItem(jsonTextData.TextIdentifier, "Failed to compile json file.",
                new JsonLineInfo(1, 1)));

        _compilationResultLogger.LogCompilationResult(jsonTextData, compilationResult);
        return compilationResult;
    }

    private IJsonObjectData? Convert(IJsonTextData jsonTextData, List<ICompilationErrorItem> compilationErrorItems)
    {
        IRootParsedValue rootParsedValue;

        try
        {
            rootParsedValue = _jsonParser.Parse(jsonTextData.JsonText);
        }
        catch (Exception e)
        {
            LogHelper.Context.Log.Error($"Failed to parse json text file with [{nameof(IJsonTextData.TextIdentifier)}]=[{jsonTextData.TextIdentifier}].", e);

            JsonLineInfo? errorLineInfo = null;

            if (e is Newtonsoft.Json.JsonReaderException jsonReaderException)
            {
                errorLineInfo = new JsonLineInfo(jsonReaderException.LineNumber, jsonReaderException.LinePosition);
            }

            compilationErrorItems.Add(new CompilationErrorItem(jsonTextData.TextIdentifier, e.Message, errorLineInfo));
            return null;
        }

        if (jsonTextData.ParentJsonTextData == null)
            return new JsonObjectData(jsonTextData, rootParsedValue);

        var parentJsonObjectData = Convert(jsonTextData.ParentJsonTextData, compilationErrorItems);
        return new JsonObjectData(jsonTextData, rootParsedValue, parentJsonObjectData);
    }

    private void Compile(IJsonObjectData jsonObjectData, CompilationResult compilationResult)
    {
        var jsonObjectDataItems = new List<IJsonObjectData> { jsonObjectData };

        var parentJsonObjectData = jsonObjectData.ParentJsonObjectData;

        while (parentJsonObjectData != null)
        {
            jsonObjectDataItems.Add(parentJsonObjectData);
            parentJsonObjectData = parentJsonObjectData.ParentJsonObjectData;
        }

        var compiledRootParsedValues = new LinkedList<IRootParsedValue>();

        for (var i = jsonObjectDataItems.Count - 1; i >= 0; --i)
        {
            var jsonObjectDataItem = jsonObjectDataItems[i];

            var compiledRootParsedValue = Compile(jsonObjectDataItem, compiledRootParsedValues.ToList(), compilationResult.CompilationErrors);

            compilationResult.CompiledJsonFiles.Add(new CompiledJsonData(jsonObjectDataItem.JsonTextData.TextIdentifier, compiledRootParsedValue));

            if (compilationResult.CompilationErrors.Count > 0)
                return;

            compiledRootParsedValues.AddFirst(compiledRootParsedValue);
        }
    }

    private IRootParsedValue Compile(IJsonObjectData jsonObjectData, IReadOnlyList<IRootParsedValue> compiledParentJsonObjects, List<ICompilationErrorItem> compilationErrors)
    {
        var jsonValueMutators = new List<IJsonValueMutator>();

        var compiledRootParsedValue = jsonObjectData.RootParsedValue;

        _parsedJsonVisitor.Visit(compiledRootParsedValue, (parsedValue) =>
        {
            if (parsedValue is IParsedSimpleValue { IsString: true } parsedSimpleValue)
            {
                var parsedExpressionsResult = _jsonValueMutatorFunctionTemplatesParser.TryParseExpression(jsonObjectData, parsedSimpleValue);

                if (parsedExpressionsResult.Errors.Count > 0 || parsedExpressionsResult.Value == null)
                {
                    if (parsedExpressionsResult.Errors.Count == 0)
                    {
                        compilationErrors.Add(new CompilationErrorItem(jsonObjectData.JsonTextData.TextIdentifier,
                            "Failed to parse the expression.", parsedSimpleValue.LineInfo));
                    }
                    else
                    {
                        compilationErrors.AddRange(parsedExpressionsResult.Errors.Select(x =>
                            new CompilationErrorItem(jsonObjectData.JsonTextData.TextIdentifier, x.ErrorMessage, x.LineInfo)));
                    }

                    return false;
                }

                if (parsedExpressionsResult.Value.Count == 0)
                    return true;

                var jsonValueMutatorResult = _jsonValueMutatorFactory.Create(jsonObjectData, parsedSimpleValue, parsedExpressionsResult.Value);

                if (jsonValueMutatorResult.Errors.Count > 0 || jsonValueMutatorResult.Value == null)
                {
                    if (jsonValueMutatorResult.Errors.Count == 0)
                    {
                        compilationErrors.Add(new CompilationErrorItem(jsonObjectData.JsonTextData.TextIdentifier,
                            "Failed to create a mutator for expression.", parsedSimpleValue.LineInfo));
                    }
                    else
                    {
                        compilationErrors.AddRange(jsonValueMutatorResult.Errors.Select(x =>
                            new CompilationErrorItem(jsonObjectData.JsonTextData.TextIdentifier, x.ErrorMessage, x.LineInfo)));
                    }

                    return false;
                }

                jsonValueMutators.Add(jsonValueMutatorResult.Value);
                return true;
            }

            return true;
        });

        List<IJsonObjectParseError> mutationErrors = new List<IJsonObjectParseError>();

        foreach (var jsonValueMutatorExpression in jsonValueMutators)
        {
            jsonValueMutatorExpression.Mutate(compiledRootParsedValue, compiledParentJsonObjects, mutationErrors);

            if (mutationErrors.Count > 0)
            {
                compilationErrors.AddRange(
                    mutationErrors.Select(x =>
                        new CompilationErrorItem(jsonObjectData.JsonTextData.TextIdentifier, x.ErrorMessage, x.LineInfo)));
                return compiledRootParsedValue;
            }
        }

        return compiledRootParsedValue;
    }

    /// <inheritdoc />
    private class CompilationResult : ICompilationResult
    {
        /// <inheritdoc />
        IReadOnlyList<ICompilationErrorItem> ICompilationResult.CompilationErrors => CompilationErrors;

        public List<ICompilationErrorItem> CompilationErrors { get; } = new();

        /// <inheritdoc />
        IReadOnlyList<ICompiledJsonData> ICompilationResult.CompiledJsonFiles => CompiledJsonFiles;

        public List<ICompiledJsonData> CompiledJsonFiles { get; } = new();
    }
}