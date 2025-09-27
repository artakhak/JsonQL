using JsonQL.Compilation;

namespace JsonQL.Query;

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
