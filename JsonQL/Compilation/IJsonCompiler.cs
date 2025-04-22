using JsonQL.Compilation.JsonValueMutator;
using JsonQL.JsonObjects;
using Newtonsoft.Json;
using OROptimizer.Diagnostics.Log;
using System.Diagnostics.CodeAnalysis;

namespace JsonQL.Compilation;

public interface IJsonCompiler
{
    /// <summary>
    /// Compiles json with expressions in <paramref name="jsonText"/> and returns a result and returns <see cref="ICompilationResult"/>.<br/>
    /// The parameter <paramref name="jsonText"/> is a json text that can have JsonQL expressions that might reference objects in the same json or any<br/>
    /// of the parents in <paramref name="compiledParents"/>. The objects referenced in JsonQL expressions in <paramref name="jsonText"/><br/>
    /// are looked up first in <paramref name="jsonText"/> and then in <paramref name="compiledParents"/>, in such a way that json files<br/>
    /// that appear earlier in list <paramref name="compiledParents"/> will be searched first.
    /// </summary>
    /// <param name="jsonText">Json text to compile.</param>
    /// <param name="jsonTextIdentifier">Json text unique identifier. Will be used in errors in result (in <see cref="ICompilationErrorItem.JsonTextIdentifier"/>).</param>
    /// <param name="compiledParents"></param>
    /// <returns></returns>
    ICompilationResult Compile(
        string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents);

    /// <summary>
    /// Compiles json with expressions in <paramref name="jsonTextData"/> and returns <see cref="ICompilationResult"/>.
    /// Json objects in <paramref name="jsonTextData"/> are compiled in the following order:
    /// The topmost parent (retrieved via generating list of objects using <see cref="IJsonTextData.ParentJsonTextData"/> properties) is
    /// compiled first. Then the next parent is complied by using json in parent json objects compiled earlier, to resolve any expressions that
    /// reference parent jsons (by looking up closest parent first). Eventually json in <paramref name="jsonTextData"/>.<see cref="IJsonTextData.JsonText"/>
    /// is compiled and the result is returned in result in <see cref="ICompilationResult.CompiledJsonFiles"/>.
    /// To look-up the parsed json in specific file, look up parsed json in <see cref="ICompilationResult.CompiledJsonFiles"/> by using
    /// <see cref="ICompiledJsonData.TextIdentifier"/>.
    /// The compiled files are sorted in such a way that the parent parsed json files appear before the child parsed json files.
    /// </summary>
    /// <param name="jsonTextData">Json text data that contains json data for all json files to compile.</param>
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

    public ICompilationResult Compile(string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents)
    {
        var compilationResult = new CompilationResult();
      
        if (!TryParse(jsonText, jsonTextIdentifier, compilationResult.CompilationErrors, out var rootParsedValue))
            return compilationResult;

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

        var compiledJsonData = compilationResult.CompiledJsonFiles.FirstOrDefault(x => x.TextIdentifier == jsonTextIdentifier);

        if (compiledJsonData == null)
            return compilationResult;

        _compilationResultLogger.LogCompilationResult(jsonTextData, compilationResult);
        return compilationResult;
    }

    /// <inheritdoc />
    public ICompilationResult Compile(IJsonTextData jsonTextData)
    {
        var compilationResult = new CompilationResult();

        IJsonObjectData? jsonObjectData = Convert(jsonTextData, compilationResult.CompilationErrors);

        if (jsonObjectData == null)
            return compilationResult;

        Compile(jsonObjectData, compilationResult);
        
        _compilationResultLogger.LogCompilationResult(jsonTextData, compilationResult);
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
            rootParsedValue = _jsonParser.Parse(jsonText);
            return true;
        }
        catch (Exception e)
        {
            LogHelper.Context.Log.Error($"Failed to parse json text file with [{nameof(IJsonTextData.TextIdentifier)}]=[{jsonTextIdentifier}].", e);

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