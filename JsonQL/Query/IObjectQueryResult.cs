using JsonQL.Compilation;

namespace JsonQL.Query;

public interface IObjectQueryResult<TQueryObject>
{
    TQueryObject? Value { get; }
    IQueryResultErrorsAndWarnings ErrorsAndWarnings { get; }
}

/// <inheritdoc />
internal class ObjectQueryResult<TQueryObject> : IObjectQueryResult<TQueryObject>
{
    public ObjectQueryResult(IReadOnlyList<ICompilationErrorItem> compilationErrors)
    {
        Value = default;
        ErrorsAndWarnings = new QueryResultErrorsAndWarnings(compilationErrors,
            EmptyErrors.EmptyConversionErrors, EmptyErrors.EmptyConversionErrors);
    }

    public ObjectQueryResult(TQueryObject? value, IQueryResultErrorsAndWarnings queryResultErrorsAndWarnings)
    {
        Value = value;
        ErrorsAndWarnings = queryResultErrorsAndWarnings;
    }

    public ObjectQueryResult(TQueryObject? value)
    {
        Value = value;
        ErrorsAndWarnings = EmptyErrors.EmptyQueryResultErrorsAndWarnings;
    }

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

public static class QueryResultExtensions
{
    public static bool HasErrorsOrWarnings<TQueryObject>(this IObjectQueryResult<TQueryObject> objectQueryResult)
    {
        return objectQueryResult.ErrorsAndWarnings.Warnings.Errors.Count > 0 ||
               objectQueryResult.HasErrors();
    }

    public static bool HasErrors<TQueryObject>(this IObjectQueryResult<TQueryObject> objectQueryResult)
    {
        return objectQueryResult.ErrorsAndWarnings.CompilationErrors.Count > 0 ||
               objectQueryResult.ErrorsAndWarnings.Errors.Errors.Count > 0;
    }
}