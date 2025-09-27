namespace JsonQL.Query;

/// <summary>
/// Provides extension methods for handling and analyzing the results of query operations represented by the <c>IObjectQueryResult</c> interface.
/// </summary>
public static class QueryResultExtensions
{
    /// <summary>
    /// Determines whether the query result contains compilation errors or conversion errors.
    /// </summary>
    /// <typeparam name="TQueryObject">The type of the object resulting from the query.</typeparam>
    /// <param name="objectQueryResult">The query result to evaluate for errors.</param>
    /// <returns>True if the query result contains compilation errors or conversion errors; otherwise, false.</returns>
    public static bool HasErrors<TQueryObject>(this IObjectQueryResult<TQueryObject> objectQueryResult)
    {
        return objectQueryResult.ErrorsAndWarnings.CompilationErrors.Count > 0 ||
               objectQueryResult.ErrorsAndWarnings.ConversionErrors.Errors.Count > 0;
    }
}