using JsonQL.Compilation;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Query;

public static class EmptyErrors
{
    public static readonly IReadOnlyList<ICompilationErrorItem> EmptyCompilationErrors = Array.Empty<ICompilationErrorItem>();
    public static readonly IConversionErrors EmptyConversionErrors = new ConversionErrors();
    public static readonly IConversionErrorsAndWarnings EmptyConversionErrorsAndWarnings = new ConversionErrorsAndWarnings(EmptyConversionErrors, EmptyConversionErrors);
    public static readonly IQueryResultErrorsAndWarnings EmptyQueryResultErrorsAndWarnings =
        new QueryResultErrorsAndWarnings(EmptyCompilationErrors, EmptyConversionErrors, EmptyConversionErrors);
}