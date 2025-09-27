using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation.JsonValueMutator;
using JsonQL.JsonObjects;
using Newtonsoft.Json;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation;

/// <inheritdoc />
public class JsonCompiler : IJsonCompiler
{
    private readonly IJsonParser _jsonParser;
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IJsonValueMutatorFunctionTemplatesParser _jsonValueMutatorFunctionTemplatesParser;
    private readonly IJsonValueMutatorFactory _jsonValueMutatorFactory;

    private readonly ICompilationResultMapper _compilationResultMapper;
    private readonly ICompilationResultLogger _compilationResultLogger;
    private readonly ILog _logger;
    private readonly IDateTimeOperations _dateTimeOperations;

    /// <summary>
    /// Provides functionality to compile JSON strings with embedded expressions into structured data.
    /// </summary>
    /// <remarks>
    /// The <see cref="JsonCompiler"/> utilizes a set of implementations specified in <see cref="IJsonCompilerParameters"/>
    /// to parse JSON, handle parsed JSON objects, apply mutators, and log compilation results. It supports creating
    /// resulting JSON data by resolving references to objects within the input JSON or its compiled parent JSONs.
    /// </remarks>
    public JsonCompiler(IJsonCompilerParameters parameters)
    {
        _jsonParser = parameters.JsonParser;
        _parsedJsonVisitor = parameters.ParsedJsonVisitor;
        _jsonValueMutatorFunctionTemplatesParser = parameters.JsonValueMutatorFunctionTemplatesParser;
        _jsonValueMutatorFactory = parameters.JsonValueMutatorFactory;
        _compilationResultMapper = parameters.CompilationResultMapper;
        _compilationResultLogger = parameters.CompilationResultLogger;
        
        _logger = parameters.Logger;
        _dateTimeOperations = parameters.DateTimeOperations;
    }

    /// <inheritdoc />
    public ICompilationResult Compile(string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents)
    {
        ThreadStaticLoggingContext.Context = _logger;
        ThreadStaticDateTimeOperationsContext.Context = _dateTimeOperations;

        var compilationResult = new CompilationResult();
      
        if (!TryParse(jsonText, jsonTextIdentifier, compilationResult.CompilationErrors, out var rootParsedValue))
            return compilationResult;

        // ReSharper disable once UseObjectOrCollectionInitializer
        var jsonTextIdentifiers = new List<string>(compiledParents.Count + 1);
        jsonTextIdentifiers.Add(jsonTextIdentifier);
        jsonTextIdentifiers.AddRange(compiledParents.Select(x => x.TextIdentifier));

        JsonObjectData? parentJsonObjectData = null;
        JsonTextData? parentJsonTextData = null;
        for (var i = compiledParents.Count - 1; i >= 0; --i)
        {
            var currentParentCompiledJsonData = compiledParents[i];

            var currentParentJsonTextData = new JsonTextData(currentParentCompiledJsonData.TextIdentifier, currentParentCompiledJsonData.JsonText, parentJsonTextData);
            var currentParentJsonObjectData = new JsonObjectData(currentParentJsonTextData, currentParentCompiledJsonData.CompiledParsedValue, parentJsonObjectData);

            parentJsonObjectData = currentParentJsonObjectData;
            parentJsonTextData = currentParentJsonTextData;
        }

        var jsonTextData = new JsonTextData(jsonTextIdentifier, jsonText, parentJsonTextData);

        var jsonObjectData = new JsonObjectData(jsonTextData, rootParsedValue, parentJsonObjectData);

        Compile(jsonObjectData, compilationResult);
     
        var convertedCompilationResult = _compilationResultMapper.Map(jsonTextIdentifiers, compilationResult);

        _compilationResultLogger.LogCompilationResult(jsonTextData, convertedCompilationResult);
        return convertedCompilationResult;
    }

    /// <inheritdoc />
    public ICompilationResult Compile(IJsonTextData jsonTextData)
    {
        ThreadStaticLoggingContext.Context = _logger;
        ThreadStaticDateTimeOperationsContext.Context = _dateTimeOperations;

        var compilationResult = new CompilationResult();

        IJsonObjectData? jsonObjectData = Convert(jsonTextData, compilationResult.CompilationErrors);

        if (jsonObjectData == null)
            return compilationResult;

        Compile(jsonObjectData, compilationResult);

        var jsonTextIdentifiers = new List<string>(20);
        jsonTextIdentifiers.Add(jsonTextData.TextIdentifier);

        var parentJsonTextData = jsonTextData.ParentJsonTextData;
        while (parentJsonTextData != null)
        {
            jsonTextIdentifiers.Add(parentJsonTextData.TextIdentifier);
            parentJsonTextData = parentJsonTextData.ParentJsonTextData;
        }

        var convertedCompilationResult = _compilationResultMapper.Map(jsonTextIdentifiers, compilationResult);

        _compilationResultLogger.LogCompilationResult(jsonTextData, convertedCompilationResult);
        return compilationResult;
    }

    private IJsonObjectData? Convert(IJsonTextData jsonTextData, List<ICompilationErrorItem> compilationErrorItems)
    {
        if (!TryParse(jsonTextData.JsonText, jsonTextData.TextIdentifier, compilationErrorItems, out var rootParsedValue))
            return null;

        if (jsonTextData.ParentJsonTextData == null)
            return new JsonObjectData(jsonTextData, rootParsedValue);

        var parentJsonObjectData = Convert(jsonTextData.ParentJsonTextData, compilationErrorItems);
        return new JsonObjectData(jsonTextData, rootParsedValue, parentJsonObjectData);
    }

    private bool TryParse(string jsonText, string jsonTextIdentifier, List<ICompilationErrorItem> compilationErrorItems, [NotNullWhen(true)] out IRootParsedValue? rootParsedValue)
    {
        try
        {
            rootParsedValue = _jsonParser.Parse(jsonText, jsonTextIdentifier);
            return true;
        }
        catch (Exception e)
        {
            ThreadStaticLoggingContext.Context.Error($"Failed to parse json text file with [{nameof(IJsonTextData.TextIdentifier)}]=[{jsonTextIdentifier}].", e);

            JsonLineInfo? errorLineInfo = null;

            if (e is JsonReaderException jsonReaderException)
            {
                errorLineInfo = new JsonLineInfo(jsonReaderException.LineNumber, jsonReaderException.LinePosition);
            }

            rootParsedValue = null;
            compilationErrorItems.Add(new CompilationErrorItem(jsonTextIdentifier, e.Message, errorLineInfo));
            return false;
        }
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

            compilationResult.CompiledJsonFiles.Add(new CompiledJsonData(jsonObjectDataItem.JsonTextData.TextIdentifier, jsonObjectDataItem.JsonTextData.JsonText, compiledRootParsedValue));

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