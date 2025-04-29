namespace JsonQL;

using OROptimizer;

internal class ThreadStaticLoggingContext : ThreadStaticAmbientContext<OROptimizer.Diagnostics.Log.ILog, OROptimizer.Diagnostics.Log.LogToConsole>
{

}