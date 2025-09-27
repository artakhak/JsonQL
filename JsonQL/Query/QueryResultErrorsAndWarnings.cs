using JsonQL.Compilation;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Query;

/// <inheritdoc />
public class QueryResultErrorsAndWarnings : IQueryResultErrorsAndWarnings
{
    /// <summary>
    /// Represents the result of a query, containing information about compilation,
    /// conversion errors, and warnings encountered during the query execution process.
    /// </summary>
    /// <param name="compilationErrors">A collection of errors encountered during the query compilation process.</param>
    /// <param name="errors">An instance encapsulating conversion errors encountered during execution.</param>
    /// <param name="warnings">An instance encapsulating conversion warnings encountered during execution.</param>
    public QueryResultErrorsAndWarnings(IReadOnlyList<ICompilationErrorItem> compilationErrors, IConversionErrors errors, IConversionErrors warnings)
    {
        CompilationErrors = compilationErrors;
        ConversionErrors = errors;
        ConversionWarnings = warnings;
    }

    /// <inheritdoc />
    public IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
    
    /// <inheritdoc />
    public IConversionErrors ConversionErrors { get; }
    
    /// <inheritdoc />
    public IConversionErrors ConversionWarnings { get; }
}
