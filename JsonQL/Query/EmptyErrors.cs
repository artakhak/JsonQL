using JsonQL.Compilation;
using JsonQL.JsonToObjectConversion;

namespace JsonQL.Query;

/// <summary>
/// Provides static instances of empty error and warning collections for use across the framework.
/// </summary>
/// <remarks>
/// This class contains pre-initialized empty instances for compilation errors, conversion errors, and warnings.
/// It facilitates the reuse of empty representations to avoid redundant object creation.
/// </remarks>
public static class EmptyErrors
{
    /// <summary>
    /// A static, read-only instance that represents an empty collection of compilation errors.
    /// </summary>
    /// <remarks>
    /// This empty collection is of type <see cref="IReadOnlyList{T}"/> where T is <see cref="ICompilationErrorItem"/>.
    /// It is used to signify the absence of compilation errors within the framework, avoiding unnecessary memory allocation.
    /// Commonly utilized for scenarios where error-free states need to be represented.
    /// </remarks>
    public static readonly IReadOnlyList<ICompilationErrorItem> EmptyCompilationErrors = Array.Empty<ICompilationErrorItem>();

    /// <summary>
    /// A static, read-only instance that represents an empty collection of conversion errors.
    /// </summary>
    /// <remarks>
    /// This instance is of type <see cref="IConversionErrors"/> and is initialized as an empty implementation.
    /// It is utilized to indicate the absence of conversion errors in contexts where such representations are needed,
    /// reducing memory consumption by avoiding the creation of multiple empty objects.
    /// Commonly used in scenarios where conversion error-free states are to be communicated within the framework.
    /// </remarks>
    public static readonly IConversionErrors EmptyConversionErrors = new ConversionErrors();

    /// <summary>
    /// A static, read-only instance that represents an empty collection of conversion errors and warnings.
    /// </summary>
    /// <remarks>
    /// This instance implements <see cref="IConversionErrorsAndWarnings"/> and contains pre-initialized, empty
    /// objects for both <see cref="IConversionErrors"/> (`ConversionErrors`) and <see cref="IConversionErrors"/>
    /// (`ConversionWarnings`). It is used to signify the absence of conversion errors or warnings in JSON-to-object
    /// conversion scenarios, promoting efficient memory usage and consistent representation of error-free states.
    /// Commonly leveraged in cases where no conversion issues are identified during processing.
    /// </remarks>
    public static readonly IConversionErrorsAndWarnings EmptyConversionErrorsAndWarnings = new ConversionErrorsAndWarnings(EmptyConversionErrors, EmptyConversionErrors);

    /// <summary>
    /// A static, read-only instance that represents an empty set of query result errors and warnings.
    /// </summary>
    /// <remarks>
    /// This instance is of type <see cref="IQueryResultErrorsAndWarnings"/> and is used to signify the absence of both errors and warnings
    /// in query results. It is part of a set of predefined empty error representations within the framework to minimize memory usage and
    /// avoid allocating unnecessary resources.
    /// Commonly utilized for scenarios where query execution does not produce any errors or warnings.
    /// </remarks>
    public static readonly IQueryResultErrorsAndWarnings EmptyQueryResultErrorsAndWarnings =
        new QueryResultErrorsAndWarnings(EmptyCompilationErrors, EmptyConversionErrors, EmptyConversionErrors);
}