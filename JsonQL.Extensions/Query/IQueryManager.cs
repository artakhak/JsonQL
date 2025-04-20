using JsonQL.Compilation;
using JsonQL.Extensions.JsonToObjectConversion;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JsonQL.Extensions.Query;

public interface IQueryManager
{
    /// <summary>
    /// Queries json in <param name="jsonTextData"></param> for single json object. If the query result does not find a single object, null value will be returned in<br/>
    /// <see cref="IObjectQueryResult{TQueryObject}.Value"/>. Example when null is returned is object does not exist, or it is a collection of multiple<br/>
    /// json objects selected by query.<br/>
    /// Note, single objects include values of type <see cref="IParsedJson"/>, <see cref="IParsedSimpleValue"/>, <see cref="IParsedArrayValue"/>.<br/>
    /// <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/> might contain compilation errors in <see cref="IQueryResultErrorsAndWarnings.CompilationErrors"/>.
    /// </summary>
    /// <param name="query">Query text. Examples: "x.Array1", "x.Array1[1, 2]", "x.Array1.Where(x => x.Salary > 10 && index > 2).First(x => x.Company == 'XYZ')"</param>
    /// <param name="jsonTextData">Queried json text(s) data. In many cases we might query a single json text, in which case
    /// <see cref="IJsonTextData.ParentJsonTextData"/> will be null. In some cases, some json texts might have query expressions that start with
    /// "parent", which are resolved from parent json in <see cref="IJsonTextData.ParentJsonTextData"/> (or parents of parent, if the object is not found in parent).
    /// </param>
    /// <returns>Returns query result as <see cref="IParsedValue"/>.
    /// The value is one of the following:<br/>
    /// -<see cref="IParsedJson"/> for a result representing objects. Example {"EmployeeId": 15, "Age": 65}<br/>
    /// <see cref="IParsedArrayValue"/> for a result representing collections. Example [1, 3, 7]<br/>
    /// <see cref="IParsedSimpleValue"/> for results representing simple values like numbers, texts, booleans.
    /// </returns>
    IJsonValueQueryResult QueryJsonValue(string query, IJsonTextData jsonTextData);

    /// <summary>
    /// Queries json in <param name="jsonTextData"></param> and converts the query result to an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <param name="query">Query text. Examples: "x.Array1", "x.Array1[1, 2]", "x.Array1.Where(x => x.Salary > 10 && index > 2)"</param>
    /// <param name="jsonTextData">Queried json text(s) data. In many cases we might query a single json text, in which case
    /// <see cref="IJsonTextData.ParentJsonTextData"/> will be null. In some cases, some json texts might have query expressions that start with
    /// "parent", which are resolved from parent json in <see cref="IJsonTextData.ParentJsonTextData"/> (or parents of parent, if the object is not found in parent).
    /// </param>
    /// <param name="typeToConvertTo">Type to convert to. If the value <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null,
    /// it will be an instance of <paramref name="typeToConvertTo"/></param>
    /// <param name="convertedValueNullability">
    /// If the value is not null, specifies the nullability of returned value and might result in errors/warnings<br/>
    /// being reported if the returned value is null, or any item in returned collection items are null.<br/>
    /// 'Non-Nullable value not set' errors will be reported only if <see cref="ConversionErrorType.NonNullablePropertyNotSet"/> or<br/>
    /// <see cref="ConversionErrorType.NonNullableCollectionItemValueNotSet"/> are not configured with <see cref="ErrorReportingType.Ignore"/>.<br/>
    /// Note, this value only affects the returned value. Property values in returned objects (including collection item nullability)  are checked<br/>
    /// by using microsoft's nullability flag '?'.<br/>
    /// The following rules are used.<br/> 
    /// -If the returned value is not a collection, <paramref name="convertedValueNullability"/> should <br/>
    /// have a single value with <b>true</b> for nullable returned value and <b>false</b> otherwise.<br/>
    /// -If the returned type is a collection, such as <see cref="IReadOnlyList{T}"/>, then the first<br/>
    /// item specifies nullability of collection itself (<b>true</b> for nullable, <b>false</b> for non-nullable),<br/>
    /// the second value specifies the nullability of first level items, the third item<br/>
    /// specifies nullability of second level items (such as items in lists of lists), etc.<br/>
    /// For example if returned type is <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee&gt;&gt; Employees {get; set}</b>,<br/>
    /// then [true, false, true] will result in implementation assuming that the returned value can be nullable,<br/>
    /// lists <b>IReadOnlyList&lt;IEmployee&gt;</b> in returned value are not nullable, and <b>IEmployee</b> items in <b>IReadOnlyList&lt;IEmployee&gt;</b> are nullable.
    /// </param> 
    /// <param name="jsonConversionSettingOverrides">Override conversion settings. If the value is null, the default settings will be used.
    /// The settings will be merged with default settings.
    /// </param>
    /// <returns></returns>
    IObjectQueryResult<object?> QueryObject(string query, IJsonTextData jsonTextData, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability = null, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null);
}

public class QueryManager : IQueryManager
{
    private readonly IJsonCompiler _jsonCompiler;
    private readonly IJsonParsedValueConversionManager _jsonParsedValueConversionManager;
    private readonly ILog _logger;

    private const string InvalidStateReachedError = "Failed to generate query result. Invalid state reached.";

    public QueryManager(IJsonCompiler jsonCompiler, IJsonParsedValueConversionManager jsonParsedValueConversionManager, ILog? logger)
    {
        _jsonCompiler = jsonCompiler;
        _jsonParsedValueConversionManager = jsonParsedValueConversionManager;
        _logger = logger ?? new LogToConsole(LogLevel.Debug);
    }

    /// <inheritdoc />
    public IJsonValueQueryResult QueryJsonValue(string query, IJsonTextData jsonTextData)
    {
        return ExecuteQuery(query, jsonTextData);
    }

    public IObjectQueryResult<object?> QueryObject(string query, IJsonTextData jsonTextData, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var executeQueryResult = ExecuteQuery(query, jsonTextData);

        if (executeQueryResult.CompilationErrors.Count > 0 || executeQueryResult.ParsedValue == null)
        {
            return new ObjectQueryResult<object?>(executeQueryResult.CompilationErrors);
        }

        var conversionResult = _jsonParsedValueConversionManager.Convert(executeQueryResult.ParsedValue, typeToConvertTo, convertedValueNullability, jsonConversionSettingOverrides);

        return new ObjectQueryResult<object?>(conversionResult.Value, 
            new QueryResultErrorsAndWarnings(executeQueryResult.CompilationErrors,
            conversionResult.ConversionErrorsAndWarnings.ConversionErrors, conversionResult.ConversionErrorsAndWarnings.ConversionWarnings));
    }

    private IJsonValueQueryResult ExecuteQuery(string query, IJsonTextData jsonTextData)
    {
        var jsonTextStrBldr = new StringBuilder();

        const string openingBrace = "{";

        jsonTextStrBldr.AppendLine(openingBrace);
        jsonTextStrBldr.Append(Constants.QueryPrefix).Append(query).AppendLine(Constants.QuerySuffix);
        jsonTextStrBldr.Append("}");

        // The first line is for "["
        const int errorLineNumber = 1;

        var jsonText = jsonTextStrBldr.ToString();

        var queryJsonTextData = new JsonTextData(Constants.QueryTextIdentifier, jsonText, jsonTextData);

        var compilationResult = _jsonCompiler.Compile(queryJsonTextData);

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

        if (compiledJsonData.CompiledParsedValue.RootParsedValue is not IRootParsedJson rootParsedJson)
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