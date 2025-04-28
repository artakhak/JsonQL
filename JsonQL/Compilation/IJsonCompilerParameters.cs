using JsonQL.Compilation.JsonValueMutator;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation;

/// <summary>
/// Represents a set of parameters required to construct and configure a JSON compiler.
/// </summary>
public interface IJsonCompilerParameters
{
    /// <summary>
    /// Gets the JSON parser used to process JSON text into a parsed representation.
    /// </summary>
    /// <remarks>
    /// This property represents an implementation of <see cref="IJsonParser"/>,
    /// which provides functionality to parse JSON strings and convert them into
    /// an intermediary format that can be visited or manipulated further.
    /// </remarks>
    IJsonParser JsonParser { get; }

    /// <summary>
    /// Gets the parsed JSON visitor used to traverse and process parsed JSON structures.
    /// </summary>
    /// <remarks>
    /// This property provides an implementation of <see cref="IParsedJsonVisitor"/>, enabling traversal
    /// of a parsed JSON representation and processing of its values using custom visit logic.
    /// </remarks>
    IParsedJsonVisitor ParsedJsonVisitor { get; }

    /// <summary>
    /// Gets the parser responsible for interpreting and parsing JSON value
    /// mutator function templates from JSON object data.
    /// </summary>
    /// <remarks>
    /// This property provides an implementation of <see cref="IJsonValueMutatorFunctionTemplatesParser"/>,
    /// which is used to parse JSON objects representing value mutator function templates
    /// and convert them into a structured format for further processing.
    /// </remarks>
    IJsonValueMutatorFunctionTemplatesParser JsonValueMutatorFunctionTemplatesParser { get; }

    /// <summary>
    /// Provides a mechanism to create instances of JSON value mutators based on input JSON data and parsed expressions.
    /// </summary>
    /// <remarks>
    /// This factory enables the creation of JSON value mutators, which are responsible for modifying or enhancing
    /// JSON data based on specific logic or operations. The factory utilizes parsed JSON objects, simple values,
    /// and a collection of parsed expression data as input for creating the mutators.
    /// </remarks>
    IJsonValueMutatorFactory JsonValueMutatorFactory { get; }

    /// <summary>
    /// Gets the logger responsible for handling the results of the compilation process.
    /// </summary>
    /// <remarks>
    /// This property returns an instance of <see cref="ICompilationResultLogger"/>,
    /// which provides functionality to log information regarding the outcomes
    /// of JSON compilation, such as success or failure details.
    /// </remarks>
    ICompilationResultLogger CompilationResultLogger { get; }

    /// <summary>
    /// Gets the date and time operations used for parsing, formatting, and converting date and time values.
    /// </summary>
    /// <remarks>
    /// This property provides access to an implementation of <see cref="IDateTimeOperations"/>,
    /// which defines methods to parse date strings, format date values, and convert date-time objects.
    /// </remarks>
    IDateTimeOperations DateTimeOperations { get; }

    /// <summary>
    /// Gets the logger used for recording diagnostics and runtime information during application execution.
    /// </summary>
    /// <remarks>
    /// This property exposes an instance of <see cref="OROptimizer.Diagnostics.Log.ILog"/> that provides
    /// methods for logging messages of varying severity, such as debug, error, info, warning, and fatal.
    /// It also supports context properties for enriched logging scenarios.
    /// </remarks>
    ILog Logger { get; }
}

/// <inheritdoc />
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
    public ILog Logger { get; }
}