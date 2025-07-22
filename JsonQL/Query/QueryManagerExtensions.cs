// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Query;

/// <summary>
/// Provides extension methods for the <see cref="IQueryManager"/> interface to simplify query operations.
/// </summary>
public static class QueryManagerExtensions
{
    /// <summary>
    /// Queries JSON text in <paramref name="queriedJsonTextData"></paramref> and converts the query result to an instance of <typeparamref name="T"/><br/>
    /// Query errors and warnings will be in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/>.<br/>
    /// If the value of <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null, this value<br/> will be an instance of <typeparamref name="T"/>.<br/>
    /// </summary>
    /// <remarks>
    /// Use this overload if there is no need to cache JSON texts in <paramref name="queriedJsonTextData"/>.<br/>
    /// Otherwise, to re-use compiled JSON texts, and save time in compilation, use the overloaded method<br/>
    /// <see cref="QueryObject{T}(JsonQL.Query.IQueryManager,string,IReadOnlyList{ICompiledJsonData},System.Collections.Generic.IReadOnlyList{bool}?,JsonQL.JsonToObjectConversion.IJsonConversionSettingsOverrides?)"/>.
    /// </remarks>
    /// <typeparam name="T">
    /// Type to convert to. If the value <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null,<br/>
    /// it will be an instance of <typeparamref name="T"/>.<br/>
    /// Errors and warnings might be reported (based on error reporting configuration) in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/> if non-nullable<br/>
    /// properties are not set, or non-nullable collection items are not set, by evaluating C# nullability syntax (e.g., Microsoft's nullability flag '?')<br/>
    /// to determine<br/> if the values are allowed to be null. 
    /// </typeparam>
    /// <param name="queryManager">Extended <see cref="IQueryManager"/>.</param>
    /// <param name="query">Query text. Examples: <b>x.Array1</b>, <b>x.Array1[1, 2]</b>, <b>x.Array1.Where(x =&gt; x.Salary &gt; 10 &amp;&amp; index > 2).First(x => x.Company == 'XYZ')</b>
    /// </param>
    /// <param name="queriedJsonTextData">Queried JSON text(s) data. In many cases we might query a single JSON text, in which case<br/>
    /// <see cref="IJsonTextData.ParentJsonTextData"/> will be null. However, we might query multiple JSON texts,<br/>
    /// by passing <paramref name="queriedJsonTextData"/> with non-null values for <see cref="IJsonTextData.ParentJsonTextData"/>,<br/>
    /// in which case the query will be executed against all JSON texts in the chain.
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
    public static IObjectQueryResult<T> QueryObject<T>(this IQueryManager queryManager, string query, IJsonTextData queriedJsonTextData, IReadOnlyList<bool>? convertedValueNullability = null, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var queryResult = queryManager.QueryObject(query, queriedJsonTextData, typeof(T), convertedValueNullability, jsonConversionSettingOverrides);

        if (queryResult.Value == null)
            return new ObjectQueryResult<T>(queryResult.ErrorsAndWarnings);

        if (queryResult.Value is not T convertedValue)
            throw new ApplicationException($"Internal error. The query result is expected to be of type [{typeof(T)}]. Actual type is [{queryResult.Value.GetType()}]. This exception should never happen and if happens it is due to some bug.");

        return new ObjectQueryResult<T>(convertedValue, queryResult.ErrorsAndWarnings);
    }
    
    /// <summary>
    /// Queries compiled JSON files in <paramref name="compiledJsonDataToQuery"/> and converts the query result to an instance of <typeparamref name="T"/><br/>
    /// Query errors and warnings will be in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/>.<br/>
    /// If the value of <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null, this value<br/> will be an instance of <typeparamref name="T"/>.<br/>
    /// </summary>
    /// <remarks>
    /// Use this overload if the same JSON texts are queried multiple times.
    /// In these scenarios it is more efficient to compile the JSON texts once using <see cref="IJsonCompiler.Compile (string, string, IReadOnlyList{ICompiledJsonData})"/>
    /// and pass the compiled data to this overload.
    /// </remarks>
    /// <typeparam name="T">
    /// Type to convert to. If the value <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null,<br/>
    /// it will be an instance of <typeparamref name="T"/>.<br/>
    /// Errors and warnings might be reported (based on error reporting configuration) in <see cref="IObjectQueryResult{TQueryObject}.ErrorsAndWarnings"/> if non-nullable<br/>
    /// properties are not set, or non-nullable collection items are not set, by evaluating C# nullability syntax (e.g., Microsoft's nullability flag '?')<br/>
    /// to determine<br/> if the values are allowed to be null. 
    /// </typeparam>
    /// <param name="queryManager">Extended <see cref="IQueryManager"/>.</param>
    /// <param name="query">Query text. Examples: <b>x.Array1</b>, <b>x.Array1[1, 2]</b>, <b>x.Array1.Where(x =&gt; x.Salary &gt; 10 &amp;&amp; index > 2).First(x => x.Company == 'XYZ')</b>
    /// </param>
    /// <param name="compiledJsonDataToQuery">Compiled JSON files to query. JSON objects referenced in <paramref name="query"/> might be in any of the files in <paramref name="compiledJsonDataToQuery"/> the first file where objects are found will be used.<br/>
    /// In other words, the order of compiled JSON files mutters.
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
    /// <param name="jsonConversionSettingOverrides">Override conversion settings. If the value is null, the default settings will be used.<br/>
    /// The settings will be merged with default settings in <see cref="IJsonConversionSettings"/> which normally can be injected into the constructor of <see cref="IQueryManager"/> implementation.
    /// </param>
    /// <returns>Returns an instance of <see cref="IJsonValueQueryResult"/>.</returns>
    public static IObjectQueryResult<T> QueryObject<T>(this IQueryManager queryManager, string query, IReadOnlyList<ICompiledJsonData> compiledJsonDataToQuery, IReadOnlyList<bool>? convertedValueNullability = null, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var queryResult = queryManager.QueryObject(query, compiledJsonDataToQuery, typeof(T), convertedValueNullability, jsonConversionSettingOverrides);

        if (queryResult.Value == null)
            return new ObjectQueryResult<T>(queryResult.ErrorsAndWarnings);

        if (queryResult.Value is not T convertedValue)
        {
            throw new ApplicationException($"Internal error. The query result is expected to be of type [{typeof(T)}]. Actual type is [{queryResult.Value.GetType()}]. This exception should never happen and if happens it is due to some bug.");
        }
        
        return new ObjectQueryResult<T>(convertedValue, queryResult.ErrorsAndWarnings);
    }
}
