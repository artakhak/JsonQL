// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using System.Text;
using JsonQL.Compilation;
using JsonQL.JsonObjects;
using JsonQL.JsonToObjectConversion;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Query;

/// <summary>
/// Provides methods for querying JSON data and converting it into specified types or structures.
/// </summary>
public interface IQueryManager
{
    /// <summary>
    /// Queries JSON in <paramref name="queriedJsonTextData"></paramref> and returns <see cref="IJsonValueQueryResult"/>.<br/>
    /// Query compilation errors will be in <see cref="IJsonValueQueryResult.CompilationErrors"/>.<br/>
    /// If the value of <see cref="IJsonValueQueryResult.ParsedValue"/> is not null, this value<br/>
    /// will contain the parsed JSON result of type <see cref="IParsedValue"/> which can be converted to one of the following sub-interfaces:<br/>
    /// <see cref="IParsedJson"/>, <see cref="IParsedSimpleValue"/>, <see cref="IParsedArrayValue"/> to get more details about the result.
    /// </summary>
    /// <remarks>
    /// Use this overload if there is no need to cache JSON texts in <paramref name="queriedJsonTextData"/>.
    /// Otherwise, to re-use compiled JSON texts, and save time in compilation, use the overloaded method
    /// <see cref="QueryJsonValue(string, IReadOnlyList{ICompiledJsonData})"/>.
    /// </remarks>
    /// <param name="query">Query text. Examples: <b>x.Array1</b>, <b>x.Array1[1, 2]</b>, <b>x.Array1.Where(x =&gt; x.Salary &gt; 10 &amp;&amp; index > 2).First(x => x.Company == 'XYZ')</b></param>
    /// <param name="queriedJsonTextData">Queried JSON text(s) data. In many cases we might query a single JSON text, in which case
    /// <see cref="IJsonTextData.ParentJsonTextData"/> will be null. However, we might query multiple JSON texts,
    /// by passing <paramref name="queriedJsonTextData"/> with non-null values for <see cref="IJsonTextData.ParentJsonTextData"/>,
    /// in which case the query will be executed against all JSON texts in the chain.
    /// </param>
    /// <returns>
    /// Return query results as <see cref="IJsonValueQueryResult"/>.
    /// The value is one of the following:<br/>
    /// <see cref="IParsedJson"/> for a result representing objects. Example: <b>{"EmployeeId": 15, "Age": 65}</b><br/>
    /// <see cref="IParsedArrayValue"/> for a result representing collections. Example: <b>[1, 3, 7]</b><br/>
    /// <see cref="IParsedSimpleValue"/> for results representing simple values like numbers, texts, booleans.
    /// Example: <b>15</b>, <b>"ABC"</b>, <b>true</b><br/>.
    /// </returns>
    IJsonValueQueryResult QueryJsonValue(string query, IJsonTextData queriedJsonTextData);
    
    /// <summary>
    /// Queries compiled JSON files in <paramref name="compiledJsonDataToQuery"/> and returns <see cref="IJsonValueQueryResult"/>.<br/>
    /// Query compilation errors and warnings will be in <see cref="IJsonValueQueryResult.CompilationErrors"/>.<br/>
    /// If the value of <see cref="IJsonValueQueryResult.ParsedValue"/> is not null, this value<br/>
    /// will contain the parsed JSON result of type <see cref="IParsedValue"/> which can be converted to one of the following sub-interfaces:<br/>
    /// <see cref="IParsedJson"/>, <see cref="IParsedSimpleValue"/>, <see cref="IParsedArrayValue"/> to get more details about the result.
    /// </summary>
    /// <remarks>
    /// Use this overload if the same JSON texts are queried multiple times.
    /// In these scenarios it is more efficient to compile the JSON texts once using <see cref="IJsonCompiler.Compile (string, string, IReadOnlyList{ICompiledJsonData})"/>
    /// and pass the compiled data to this overload.
    /// </remarks>
    /// <param name="query">
    /// Query text. Examples: <b>x.Array1</b>, <b>x.Array1[1, 2]</b>, <b>x.Array1.Where(x =&gt; x.Salary &gt; 10 &amp;&amp; index > 2).First(x => x.Company == 'XYZ')</b>
    /// </param>
    /// <param name="compiledJsonDataToQuery">Compiled JSON files to query. JSON objects referenced in <paramref name="query"/> might be in any of the files in <paramref name="compiledJsonDataToQuery"/> the first file where objects are found will be used.
    /// In other words, the order of compiled JSON files mutters.
    /// </param>
    /// <returns>
    /// Return query results as <see cref="IJsonValueQueryResult"/>.
    /// The value is one of the following:<br/>
    /// <see cref="IParsedJson"/> for a result representing objects. Example: <b>{"EmployeeId": 15, "Age": 65}</b><br/>
    /// <see cref="IParsedArrayValue"/> for a result representing collections. Example: <b>[1, 3, 7]</b><br/>
    /// <see cref="IParsedSimpleValue"/> for results representing simple values like numbers, texts, booleans.<br/>
    /// Example: <b>15</b>, <b>"ABC"</b>, <b>true</b><br/>.
    /// </returns>
    IJsonValueQueryResult QueryJsonValue(string query, IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery);

    /// <summary>
    /// Queries JSON text in <paramref name="queriedJsonTextData"></paramref> and converts the query result to an instance of <paramref name="typeToConvertTo"/><br/>
    /// Query errors and warnings will be in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/>.<br/>
    /// If the value of <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null, this value<br/> will be an instance of <paramref name="typeToConvertTo"/>.<br/>
    /// </summary>
    /// <remarks>
    /// Use this overload if there is no need to cache JSON texts in <paramref name="queriedJsonTextData"/>.
    /// Otherwise, to re-use compiled JSON texts, and save time in compilation, use the overloaded method
    /// <see cref="QueryObject(string,IReadOnlyList{ICompiledJsonData},System.Type,System.Collections.Generic.IReadOnlyList{bool}?,JsonQL.JsonToObjectConversion.IJsonConversionSettingsOverrides?)"/>.
    /// </remarks>
    /// <param name="query">Query text. Examples: <b>x.Array1</b>, <b>x.Array1[1, 2]</b>, <b>x.Array1.Where(x =&gt; x.Salary &gt; 10 &amp;&amp; index > 2).First(x => x.Company == 'XYZ')</b>
    /// </param>
    /// <param name="queriedJsonTextData">Queried JSON text(s) data. In many cases we might query a single JSON text, in which case<br/>
    /// <see cref="IJsonTextData.ParentJsonTextData"/> will be null. However, we might query multiple JSON texts,<br/>
    /// by passing <paramref name="queriedJsonTextData"/> with non-null values for <see cref="IJsonTextData.ParentJsonTextData"/>,<br/>
    /// in which case the query will be executed against all JSON texts in the chain.
    /// </param>
    /// <param name="typeToConvertTo">Type to convert to. If the value <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null,<br/>
    /// it will be an instance of <paramref name="typeToConvertTo"/>.<br/>
    /// Errors and warnings might be reported (based on error reporting configuration) in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/> if non-nullable<br/>
    /// properties are not set, or non-nullable collection items are not set, by evaluating C# nullability syntax (e.g., Microsoft's nullability flag '?')<br/>
    /// to determine<br/> if the values are allowed to be null. 
    /// </param>
    /// <param name="convertedValueNullability">
    /// If the value is not null, specifies the nullability of the returned value and might result in errors/warnings<br/>
    /// being reported if the returned value is null, or any item in returned collection items is null.<br/>
    /// 'Non-Nullable value not set' errors will be reported only if <see cref="ConversionErrorType.ValueNotSet"/> or<br/>
    /// <see cref="ConversionErrorType.NonNullableCollectionItemValueNotSet"/> are not configured with <see cref="ErrorReportingType.Ignore"/>.<br/>
    /// Note, this value only affects the returned value. Property values in returned objects (including collection item nullability)  are checked<br/>
    /// by using Microsoft's nullability flag '?'.<br/>
    /// The following rules are used.<br/> 
    /// - If the returned value is not a collection, <paramref name="convertedValueNullability"/> should <br/>
    /// have a single value with <b>true</b> for nullable returned value and <b>false</b> otherwise.<br/>
    /// - If the returned type is a collection, such as <see cref="IReadOnlyList{T}"/>, then the first<br/>
    /// item specifies nullability of collection itself (<b>true</b> for nullable, <b>false</b> for non-nullable),<br/>
    /// the second value specifies the nullability of first level items, the third item<br/>
    /// specifies nullability of second level items (such as items in lists of lists), etc.<br/>
    /// For example if returned type is <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee&gt;&gt; Employees {get; set}</b>,<br/>
    /// then [true, false, true] will result in implementation assuming that the returned value can be nullable,<br/>
    /// lists <b>IReadOnlyList&lt;IEmployee&gt;</b> in returned value are not nullable, and <b>IEmployee</b> items in <b>IReadOnlyList&lt;IEmployee&gt;</b> are nullable.<br/>
    /// Null value of this parameter results in nullability checks not being enforced.
    /// </param> 
    /// <param name="jsonConversionSettingOverrides">Override conversion settings. If the value is null, the default settings will be used.
    /// The settings will be merged with default settings in <see cref="IJsonConversionSettings"/> which normally can be injected into the constructor of <see cref="IQueryManager"/> implementation.
    /// </param>
    /// <returns>Returns an instance of <see cref="IJsonValueQueryResult"/>.</returns>
    IObjectQueryResult<object?> QueryObject(string query, IJsonTextData queriedJsonTextData, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability = null, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null);

    /// <summary>
    /// Queries compiled JSON files in <paramref name="compiledJsonDataToQuery"/> and converts the query result to an instance of <paramref name="typeToConvertTo"/><br/>
    /// Query errors and warnings will be in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/>.<br/>
    /// If the value of <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null, this value<br/> will be an instance of <paramref name="typeToConvertTo"/>.<br/>
    /// </summary>
    /// <remarks>
    /// Use this overload if the same JSON texts are queried multiple times.
    /// In these scenarios it is more efficient to compile the JSON texts once using <see cref="IJsonCompiler.Compile (string, string, IReadOnlyList{ICompiledJsonData})"/>
    /// and pass the compiled data to this overload.
    /// </remarks>
    /// <param name="query">Query text. Examples: <b>x.Array1</b>, <b>x.Array1[1, 2]</b>, <b>x.Array1.Where(x =&gt; x.Salary &gt; 10 &amp;&amp; index > 2).First(x => x.Company == 'XYZ')</b>
    /// </param>
    /// <param name="compiledJsonDataToQuery">Compiled JSON files to query. JSON objects referenced in <paramref name="query"/> might be in any of the files in <paramref name="compiledJsonDataToQuery"/> the first file where objects are found will be used.
    /// In other words, the order of compiled JSON files mutters.
    /// </param>
    /// <param name="typeToConvertTo">Type to convert to. If the value <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null,<br/>
    /// it will be an instance of <paramref name="typeToConvertTo"/>.<br/>
    /// Errors and warnings might be reported (based on error reporting configuration) in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/> if non-nullable<br/>
    /// properties are not set, or non-nullable collection items are not set, by evaluating C# nullability syntax (e.g., Microsoft's nullability flag '?')<br/>
    /// to determine<br/> if the values are allowed to be null. 
    /// </param>
    /// <param name="convertedValueNullability">
    /// If the value is not null, specifies the nullability of the returned value and might result in errors/warnings<br/>
    /// being reported if the returned value is null, or any item in returned collection items is null.<br/>
    /// 'Non-Nullable value not set' errors will be reported only if <see cref="ConversionErrorType.ValueNotSet"/> or<br/>
    /// <see cref="ConversionErrorType.NonNullableCollectionItemValueNotSet"/> are not configured with <see cref="ErrorReportingType.Ignore"/>.<br/>
    /// Note, this value only affects the returned value. Property values in returned objects (including collection item nullability)  are checked<br/>
    /// by using Microsoft's nullability flag '?'.<br/>
    /// The following rules are used.<br/> 
    /// - If the returned value is not a collection, <paramref name="convertedValueNullability"/> should <br/>
    /// have a single value with <b>true</b> for nullable returned value and <b>false</b> otherwise.<br/>
    /// - If the returned type is a collection, such as <see cref="IReadOnlyList{T}"/>, then the first<br/>
    /// item specifies nullability of collection itself (<b>true</b> for nullable, <b>false</b> for non-nullable),<br/>
    /// the second value specifies the nullability of first level items, the third item<br/>
    /// specifies nullability of second level items (such as items in lists of lists), etc.<br/>
    /// For example if returned type is <b>IEnumerable&lt;IReadOnlyList&lt;IEmployee&gt;&gt; Employees {get; set}</b>,<br/>
    /// then [true, false, true] will result in implementation assuming that the returned value can be nullable,<br/>
    /// lists <b>IReadOnlyList&lt;IEmployee&gt;</b> in returned value are not nullable, and <b>IEmployee</b> items in <b>IReadOnlyList&lt;IEmployee&gt;</b> are nullable.
    /// </param> 
    /// <param name="jsonConversionSettingOverrides">Override conversion settings. If the value is null, the default settings will be used.
    /// The settings will be merged with default settings in <see cref="IJsonConversionSettings"/> which normally can be injected into the constructor of <see cref="IQueryManager"/> implementation.
    /// </param>
    /// <returns>Returns an instance of <see cref="IJsonValueQueryResult"/>.</returns>
    IObjectQueryResult<object?> QueryObject(string query, IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery, Type typeToConvertTo, IReadOnlyList<bool>? convertedValueNullability = null, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null);
}

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
        return ExecuteQuery(query, (queryTextIdentifier, queryJsonText) => _jsonCompiler.Compile(queryTextIdentifier, queryJsonText, compiledJsonDataToQuery));
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
