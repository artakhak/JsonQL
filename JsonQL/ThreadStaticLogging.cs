using OROptimizer.Diagnostics.Log;

namespace JsonQL;

internal static class ThreadStaticLogging
{
    private static readonly LogToConsole _defaultLogger = new();

    [ThreadStatic]
    private static ILog? _log;

    internal static ILog Log
    {
        get => _log ?? _defaultLogger;
        set => _log = value;
    }
}