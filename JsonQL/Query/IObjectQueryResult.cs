using JsonQL.Compilation;

namespace JsonQL.Query;

/// <summary>
/// Represents the result of an object query operation, including the queried value and any associated errors or warnings.
/// </summary>
public interface IObjectQueryResult
{
    /// <summary>
    /// Gets the collection of errors and warnings associated with the query result.
    /// This property provides details regarding compilation errors, execution errors, and warnings
    /// encountered during the query processing.
    /// </summary>
    IQueryResultErrorsAndWarnings ErrorsAndWarnings { get; }
}

/// <summary>
/// Represents the result of an object query operation, including the queried value and any associated errors or warnings.
/// </summary>
public interface IObjectQueryResult<TQueryObject>: IObjectQueryResult
{
    /// <summary>
    /// Gets the value of the query result. If the query does not resolve to a single object, this property will return null.
    /// </summary>
    TQueryObject? Value { get; }
}

/// <inheritdoc />
internal class ObjectQueryResult<TQueryObject> : IObjectQueryResult<TQueryObject>
{
    /// <summary>
    /// Represents the result of a query for an object of the specified type.
    /// Encapsulates the queried value and any associated errors or warnings.
    /// </summary>
    public ObjectQueryResult(IReadOnlyList<ICompilationErrorItem> compilationErrors)
    {
        Value = default;
        ErrorsAndWarnings = new QueryResultErrorsAndWarnings(compilationErrors,
            EmptyErrors.EmptyConversionErrors, EmptyErrors.EmptyConversionErrors);
    }

    /// <summary>
    /// Represents the result of a query operation that returns a specific type of object.
    /// Encapsulates the queried object value along with any associated errors or warnings during the query or conversion process.
    /// </summary>
    public ObjectQueryResult(TQueryObject? value, IQueryResultErrorsAndWarnings queryResultErrorsAndWarnings)
    {
        Value = value;
        ErrorsAndWarnings = queryResultErrorsAndWarnings;
    }

    /// <summary>
    /// Represents the result of a query for an object of a specified type,
    /// containing the queried value and any associated errors or warnings.
    /// </summary>
    public ObjectQueryResult(TQueryObject? value)
    {
        Value = value;
        ErrorsAndWarnings = EmptyErrors.EmptyQueryResultErrorsAndWarnings;
    }

    /// <summary>
    /// Represents the result of a query for an object of the specified type.
    /// Encapsulates the queried value and any associated errors or warnings.
    /// </summary>
    public ObjectQueryResult(IQueryResultErrorsAndWarnings queryResultErrorsAndWarnings)
    {
        Value = default;
        ErrorsAndWarnings = queryResultErrorsAndWarnings;
    }

    /// <inheritdoc />
    public TQueryObject? Value { get; }

    /// <inheritdoc />
    public IQueryResultErrorsAndWarnings ErrorsAndWarnings { get; }
}

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