namespace JsonQL;

// TODO: We can consolidate thread static context into an abstract class ThreadStaticContext in OROptimizer
// and implement it pretty similar to OROptimizer.AmbientContext<TContext, TContextDefaultImplementation>
// However for now this logic is used in a couple of places in this project, and we can do this later.
internal class ThreadStaticDateTimeOperations
{
    private static readonly IDateTimeOperations _defaultDateTimeOperations = new DateTimeOperations();

    [ThreadStatic]
    private static IDateTimeOperations? _dateTimeOperations;

    internal static IDateTimeOperations DateTimeOperations
    {
        get => _dateTimeOperations ?? _defaultDateTimeOperations;
        set => _dateTimeOperations = value;
    }
}