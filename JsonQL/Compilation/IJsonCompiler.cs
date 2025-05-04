// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using System.Diagnostics.CodeAnalysis;
using JsonQL.Compilation.JsonValueMutator;
using JsonQL.JsonObjects;
using Newtonsoft.Json;

namespace JsonQL.Compilation;

/// <summary>
/// Represents an interface for compiling JSON text into an executable or analyzable structure.
/// </summary>
public interface IJsonCompiler
{
    /// <summary>
    /// Compiles the JSON provided in <paramref name="jsonTextData"/> and produces a result that includes the compiled JSON and any errors encountered during the process.
    /// Compilation is performed hierarchically from parent JSON files to the specified JSON, resolving any references to parent JSON during the process.
    /// </summary>
    /// <remarks>Use this overload if JSON texts in <paramref name="jsonTextData"/> either have no parents,
    /// or if the parents in <see cref="IJsonTextData.ParentJsonTextData"/> are used only once.
    /// If JSON texts are used multiple times, use the over overloaded method instead.
    /// </remarks>
    /// <param name="jsonTextData">An object containing the JSON text and metadata for compilation, including references to parent JSON data.</param>
    /// <returns>
    /// An <see cref="ICompilationResult"/> containing the output of the compilation.<br/>
    /// The result includes the compiled JSON files in order, starting with parent JSON files followed by child JSON files,<br/>
    /// as well as any errors that occurred during the compilation.<br/>
    /// Since in the presence of compilation errors some files might not be in <see cref="ICompilationResult.CompiledJsonFiles"/>,<br/>
    /// a compiled file can be looked up by using <see cref="ICompiledJsonData.TextIdentifier"/>.<br/>
    /// Example: [var compiledFile=result.CompiledJsonFiles.FirstOrDefault(x=> x.TextIdentifier=="myJsonTextIdentifier")].
    /// </returns>
    ICompilationResult Compile(IJsonTextData jsonTextData);
    
    /// <summary>
    /// Compiles JSON with expressions in <paramref name="jsonText"/> and returns the results in <see cref="ICompilationResult"/>.
    /// JsonQL expressions in <paramref name="jsonText"/> are resolved by looking up objects in the following order:
    /// 1. Objects within <paramref name="jsonText"/> itself.
    /// 2. Objects in JSON files within <paramref name="compiledParents"/>, searched in the order they appear in the list.
    /// </summary>
    /// <remarks>
    /// Use this overload if the same JSON texts are compiled multiple times.
    /// In these scenarios it is more efficient to compile the JSON texts once using <see cref="Compile"/> method,
    /// and then re-use the compiled files.
    /// </remarks>
    /// <param name="jsonText">The JSON text to compile, which includes ma JsonQL expressions.</param>
    /// <param name="jsonTextIdentifier">A unique identifier for the JSON text. Used to tag errors in the compilation result.</param>
    /// <param name="compiledParents">A list of already compiled JSON data providing parent objects for resolving JsonQL expressions.</param>
    /// <returns>
    /// An <see cref="ICompilationResult"/> containing the output of the compilation.<br/>
    /// The result includes the compiled JSON files in order, starting with parent JSON files followed by child JSON files,<br/>
    /// as well as any errors that occurred during the compilation.<br/>
    /// Since in the presence of compilation errors some files might not be in <see cref="ICompilationResult.CompiledJsonFiles"/>,<br/>
    /// a compiled file can be looked up by using <see cref="ICompiledJsonData.TextIdentifier"/>.<br/>
    /// Example: [var compiledFile=result.CompiledJsonFiles.FirstOrDefault(x=> x.TextIdentifier=="myJsonTextIdentifier")].
    /// </returns>
    ICompilationResult Compile(
        string jsonText, string jsonTextIdentifier, IReadOnlyList<ICompiledJsonData> compiledParents);
}

/// <inheritdoc />
public class JsonCompiler : IJsonCompiler
{
    private readonly IJsonParser _jsonParser;
    private readonly IParsedJsonVisitor _parsedJsonVisitor;
    private readonly IJsonValueMutatorFunctionTemplatesParser _jsonValueMutatorFunctionTemplatesParser;
    private readonly IJsonValueMutatorFactory _jsonValueMutatorFactory;

    private readonly ICompilationResultLogger _compilationResultLogger;

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
        _compilationResultLogger = parameters.CompilationResultLogger;
        
        ThreadStaticLoggingContext.Context = parameters.Logger;
        ThreadStaticDateTimeOperationsContext.Context = parameters.DateTimeOperations;
    }

    /// <inheritdoc />
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