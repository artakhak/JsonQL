using JsonQL.Compilation.JsonValueMutator;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation;

/// <inheritdoc />
public class JsonCompilerParameters : IJsonCompilerParameters
{
    public JsonCompilerParameters(IJsonParser jsonParser, IParsedJsonVisitor parsedJsonVisitor,
        IJsonValueMutatorFunctionTemplatesParser jsonValueMutatorFunctionTemplatesParser,
        IJsonValueMutatorFactory jsonValueMutatorFactory, ICompilationResultMapper compilationResultMapper,
        ICompilationResultLogger compilationResultLogger,  IDateTimeOperations dateTimeOperations, ILog? logger)
    {
        JsonParser = jsonParser;
        ParsedJsonVisitor = parsedJsonVisitor;
        JsonValueMutatorFunctionTemplatesParser = jsonValueMutatorFunctionTemplatesParser;
        JsonValueMutatorFactory = jsonValueMutatorFactory;
        CompilationResultMapper = compilationResultMapper;
        CompilationResultLogger = compilationResultLogger;
        DateTimeOperations = dateTimeOperations;
        Logger = logger ?? new LogToConsole(LogLevel.Debug);
    }

    /// <inheritdoc />
    public IJsonParser JsonParser { get; }
    
    /// <inheritdoc />
    public IParsedJsonVisitor ParsedJsonVisitor { get; }
    
    /// <inheritdoc />
    public IJsonValueMutatorFunctionTemplatesParser JsonValueMutatorFunctionTemplatesParser { get; }
    
    /// <inheritdoc />
    public IJsonValueMutatorFactory JsonValueMutatorFactory { get; }
    
    /// <inheritdoc />
    public ICompilationResultLogger CompilationResultLogger { get; }
    
    /// <inheritdoc />
    public IDateTimeOperations DateTimeOperations { get; }

    /// <inheritdoc />
    public ICompilationResultMapper CompilationResultMapper { get; }

    /// <inheritdoc />
    public ILog Logger { get; }
}