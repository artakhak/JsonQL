using System.Text;
using JsonQL.Compilation;
using JsonQL.JsonObjects;
using JsonQL.JsonToObjectConversion;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Query;

public class QueryManager : IQueryManager
{
    private delegate ICompilationResult CompileJsonQueryDelegate(string queryTextIdentifier, string queryJsonText);

    private readonly IJsonCompiler _jsonCompiler;
    private readonly IJsonParsedValueConversionManager _jsonParsedValueConversionManager;
    private readonly ILog _logger;

    private const string InvalidStateReachedError = "Failed to generate query result. Invalid state reached.";

    public QueryManager(IJsonCompiler jsonCompiler, IJsonParsedValueConversionManager jsonParsedValueConversionManager, ILog logger)
    {
        _jsonCompiler = jsonCompiler;
        _jsonParsedValueConversionManager = jsonParsedValueConversionManager;
        _logger = logger;
    }

    /// <inheritdoc />
    public IJsonValueQueryResult QueryJsonValue(string query, IJsonTextData queriedJsonTextData)
    {
        return ExecuteQuery(query, (queryTextIdentifier, queryJsonText) =>
        {
            var queryJsonTextData = new JsonTextData(queryTextIdentifier, queryJsonText, queriedJsonTextData);
            return _jsonCompiler.Compile(queryJsonTextData);
        });
    }

    /// <inheritdoc />
    public IObjectQueryResult<object?> QueryObject(string query, IJsonTextData queriedJsonTextData, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var executeQueryResult = QueryJsonValue(query, queriedJsonTextData);

        if (executeQueryResult.CompilationErrors.Count > 0 || executeQueryResult.ParsedValue == null)
        {
            return new ObjectQueryResult<object?>(executeQueryResult.CompilationErrors);
        }

        var conversionResult = _jsonParsedValueConversionManager.Convert(executeQueryResult.ParsedValue, typeToConvertTo, convertedValueNullability, jsonConversionSettingOverrides);

        return new ObjectQueryResult<object?>(conversionResult.Value,
            new QueryResultErrorsAndWarnings(executeQueryResult.CompilationErrors,
                conversionResult.ConversionErrorsAndWarnings.ConversionErrors, conversionResult.ConversionErrorsAndWarnings.ConversionWarnings));
    }

    /// <inheritdoc />
    public IJsonValueQueryResult QueryJsonValue(string query, IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery)
    {
        ThreadStaticLoggingContext.Context = _logger;
        return ExecuteQuery(query, (queryTextIdentifier, queryJsonText) => _jsonCompiler.Compile(queryJsonText, queryTextIdentifier, compiledJsonDataToQuery));
    }

    /// <inheritdoc />
    public IObjectQueryResult<object?> QueryObject(string query, IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability = null, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var executeQueryResult = QueryJsonValue(query, compiledJsonDataToQuery);

        if (executeQueryResult.CompilationErrors.Count > 0 || executeQueryResult.ParsedValue == null)
        {
            return new ObjectQueryResult<object?>(executeQueryResult.CompilationErrors);
        }

        var conversionResult = _jsonParsedValueConversionManager.Convert(executeQueryResult.ParsedValue, typeToConvertTo, convertedValueNullability, jsonConversionSettingOverrides);

        return new ObjectQueryResult<object?>(conversionResult.Value,
            new QueryResultErrorsAndWarnings(executeQueryResult.CompilationErrors,
                conversionResult.ConversionErrorsAndWarnings.ConversionErrors, conversionResult.ConversionErrorsAndWarnings.ConversionWarnings));
    }
    
    private IJsonValueQueryResult ExecuteQuery(string query, CompileJsonQueryDelegate compileJsonQuery) 
    {
        ThreadStaticLoggingContext.Context = _logger;

        var jsonTextStrBldr = new StringBuilder();

        const string openingBrace = "{";
        jsonTextStrBldr.AppendLine(openingBrace);
        jsonTextStrBldr.Append(Constants.QueryPrefix).Append(query).AppendLine(Constants.QuerySuffix);
        jsonTextStrBldr.Append("}");

        // The first line is for "["
        const int errorLineNumber = 1;

        var jsonText = jsonTextStrBldr.ToString();
        
        var compilationResult = compileJsonQuery(Constants.QueryTextIdentifier, jsonText);

        if (compilationResult.CompilationErrors.Count > 0)
            return new JsonValueQueryResult(compilationResult.CompilationErrors);

        var compiledJsonData = compilationResult.CompiledJsonFiles.FirstOrDefault(
            x => string.Equals(x.TextIdentifier, Constants.QueryTextIdentifier, StringComparison.OrdinalIgnoreCase));

        if (compiledJsonData == null)
        {
            return new JsonValueQueryResult(new List<ICompilationErrorItem>
            {
                new CompilationErrorItem(Constants.QueryTextIdentifier, "Failed to generate query result.", new JsonLineInfo(errorLineNumber + 1, 1))
            });
        }

        if (compiledJsonData.CompiledParsedValue is not IRootParsedJson rootParsedJson)
        {
            _logger.ErrorFormat("The value of [{0}] is expected to be of type [{1}]. Actual type is [{2}].",
                nameof(ICompiledJsonData.CompiledParsedValue.RootParsedValue),
                typeof(IRootParsedJson), compiledJsonData.CompiledParsedValue.RootParsedValue.GetType().FullName!);

            return new JsonValueQueryResult(new List<ICompilationErrorItem>
            {
                new CompilationErrorItem(Constants.QueryTextIdentifier, InvalidStateReachedError, new JsonLineInfo(errorLineNumber + 1, 1))
            });
        }

        if (!rootParsedJson.TryGetJsonKeyValue(Constants.QueryKey, out var jsonKeyValue))
        {
            return new JsonValueQueryResult(new List<ICompilationErrorItem>
            {
                new CompilationErrorItem(Constants.QueryTextIdentifier, InvalidStateReachedError, new JsonLineInfo(errorLineNumber + 1, 1))
            });
        }

        return new JsonValueQueryResult(jsonKeyValue.Value);
    }
}
