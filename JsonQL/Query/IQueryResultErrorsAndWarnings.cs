using System.Collections.Generic;
using JsonQL.Compilation;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Query;

public interface IQueryResultErrorsAndWarnings
{
    IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
    IConversionErrors Errors { get; }
    IConversionErrors Warnings { get; }
}

public class QueryResultErrorsAndWarnings : IQueryResultErrorsAndWarnings
{
    public QueryResultErrorsAndWarnings(IReadOnlyList<ICompilationErrorItem> compilationErrors, IConversionErrors errors, IConversionErrors warnings)
    {
        CompilationErrors = compilationErrors;
        Errors = errors;
        Warnings = warnings;
    }

    public IReadOnlyList<ICompilationErrorItem> CompilationErrors { get; }
    public IConversionErrors Errors { get; }
    public IConversionErrors Warnings { get; }
}