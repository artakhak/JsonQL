using JsonQL.Compilation.JsonValueMutator;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation;

public interface IJsonCompilerParameters
{
    IJsonParser JsonParser { get; }
    IParsedJsonVisitor ParsedJsonVisitor { get; }
    IJsonValueMutatorFunctionTemplatesParser JsonValueMutatorFunctionTemplatesParser { get; }
    IJsonValueMutatorFactory JsonValueMutatorFactory { get; }
  
    ICompilationResultLogger CompilationResultLogger { get; }
    IDateTimeOperations DateTimeOperations { get; }
    ILog Logger { get; }
}

public class JsonCompilerParameters : IJsonCompilerParameters
{
    public JsonCompilerParameters(IJsonParser jsonParser, IParsedJsonVisitor parsedJsonVisitor,
        IJsonValueMutatorFunctionTemplatesParser jsonValueMutatorFunctionTemplatesParser,
        IJsonValueMutatorFactory jsonValueMutatorFactory,
        ICompilationResultLogger compilationResultLogger,  IDateTimeOperations dateTimeOperations, ILog? logger)
    {
        JsonParser = jsonParser;
        ParsedJsonVisitor = parsedJsonVisitor;
        JsonValueMutatorFunctionTemplatesParser = jsonValueMutatorFunctionTemplatesParser;
        JsonValueMutatorFactory = jsonValueMutatorFactory;
        CompilationResultLogger = compilationResultLogger;
        DateTimeOperations = dateTimeOperations;
        

        Logger = logger ?? new LogToConsole(LogLevel.Debug);
    }

    public IJsonParser JsonParser { get; }
    public IParsedJsonVisitor ParsedJsonVisitor { get; }
    public IJsonValueMutatorFunctionTemplatesParser JsonValueMutatorFunctionTemplatesParser { get; }
    public IJsonValueMutatorFactory JsonValueMutatorFactory { get; }
    public ICompilationResultLogger CompilationResultLogger { get; }
    public IDateTimeOperations DateTimeOperations { get; }
    public ILog Logger { get; }
}