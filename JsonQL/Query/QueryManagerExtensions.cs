using JsonQL.Compilation;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Query;

public static class QueryManagerExtensions
{
    /// <summary>
    /// Queries json in <param name="jsonTextData"></param> and converts the query result to an instance of <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">Type parameter for a type to convert to. If the value <see cref="IObjectQueryResult{TQueryObject}.Value"/> is not null,
    /// it will be an instance of <typeparamref name="T"/>
    /// </typeparam>
    /// <param name="queryManager"></param>
    /// <param name="query">Query text. Examples: "x.Array1", "x.Array1[1, 2]", "x.Array1.Where(x => x.Salary > 10 && index > 2)"</param>
    /// <param name="jsonTextData">Queried json text(s) data. In many cases we might query a single json text, in which case
    /// <see cref="IJsonTextData.ParentJsonTextData"/> will be null. In some cases, some json texts might have query expressions that start with
    /// "parent", which are resolved from parent json in <see cref="IJsonTextData.ParentJsonTextData"/> (or parents of parent, if the object is not found in parent).
    /// </param>
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
    public static IObjectQueryResult<T> Query<T>(this IQueryManager queryManager, string query, IJsonTextData jsonTextData, IReadOnlyList<bool>? convertedValueNullability = null, IJsonConversionSettingsOverrides? jsonConversionSettingOverrides = null)
    {
        var queryResult = queryManager.QueryObject(query, jsonTextData, typeof(T), convertedValueNullability, jsonConversionSettingOverrides);

        if (queryResult.HasErrors() || queryResult.Value == null)
            return new ObjectQueryResult<T>(queryResult.ErrorsAndWarnings);

        if (queryResult.Value is not T convertedValue)
        {
            throw new ApplicationException($"Internal error. The query result is expected to be of type [{typeof(T)}]. Actual type is [{queryResult.Value.GetType()}]. This exception should never happen and if happens it is due to some bug.");
        }
        
        return new ObjectQueryResult<T>(convertedValue, queryResult.ErrorsAndWarnings);
    }
}