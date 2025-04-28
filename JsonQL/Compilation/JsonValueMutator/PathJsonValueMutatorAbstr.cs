using JsonQL.Compilation.JsonFunction.JsonFunctions;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Represents an abstract base class for mutators that modify JSON values based on a specific path.
/// This class provides the foundational implementation for handling JSON value mutations
/// that involve path evaluations and manipulation of parsed values.
/// </summary>
public abstract class PathJsonValueMutatorAbstr : JsonValueMutatorAbstr
{
    /// <summary>
    /// Represents an abstract base class for JSON value mutators that operate on specific paths within a JSON structure.
    /// This class provides the key mechanisms for path-based interaction with JSON values, enabling mutation
    /// of parsed values within the JSON structure.
    /// </summary>
    protected PathJsonValueMutatorAbstr(IParsedSimpleValue parsedSimpleValue, IJsonValuePathJsonFunction jsonValuePathJsonFunction) 
        : base(jsonValuePathJsonFunction.LineInfo)
    {
        JsonValuePathJsonFunction = jsonValuePathJsonFunction;
        ParsedSimpleValue = parsedSimpleValue;
    }

    /// <inheritdoc />
    public sealed override void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors)
    {
        var parsedSimpleValue = MutatorHelpers.TryGetParsedSimpleValue(this.ParsedSimpleValue);

        if (parsedSimpleValue == null)
        {
            ThreadStaticLogging.Log.InfoFormat("The mutator [{0}] will not execute.", GetType().FullName);
            return;
        }

        var pathLookupResult = JsonValuePathJsonFunction.Evaluate(rootParsedValue, compiledParentRootParsedValues, null);

        if (pathLookupResult.Errors.Count > 0 || pathLookupResult.Value == null)
        {
            if (pathLookupResult.Errors.Count == 0)
            {
                errors.Add(new JsonObjectParseError($"Failed to lookup a json object by path [{JsonValuePathJsonFunction.JsonValuePath}].", JsonValuePathJsonFunction.JsonValuePath.LineInfo));
            }
            else
            {
                errors.AddRange(pathLookupResult.Errors);
            }

            return;
        }

        var parsedValuesResult = pathLookupResult.Value.GetResultAsParsedValuesList(false, this.LineInfo);

        if (parsedValuesResult.Errors.Count > 0)
        {
            errors.AddRange(parsedValuesResult.Errors);
            return;
        }

        MutateValue(parsedSimpleValue, parsedValuesResult.Value?? Array.Empty<IParsedValue>(), errors);
    }

    /// <summary>
    /// Gets the current instance of <see cref="IParsedSimpleValue"/> being used by the mutator.
    /// This property provides access to the parsed simple value, which typically represents a
    /// basic JSON value such as a string, number, or boolean that the mutator utilizes or modifies
    /// during its operations.
    /// </summary>
    protected IParsedSimpleValue ParsedSimpleValue { get; }

    /// <summary>
    /// Gets the instance of <see cref="IJsonValuePathJsonFunction"/> that represents the JSON path evaluation
    /// functionality used by the mutator. This property is responsible for handling operations
    /// related to JSON path processing and value retrieval during mutation operations.
    /// </summary>
    protected IJsonValuePathJsonFunction JsonValuePathJsonFunction { get; }

    /// <summary>
    /// Mutates the JSON that owns <param name="currentParsedSimpleValueWithPath"></param>.
    /// </summary>
    /// <param name="currentParsedSimpleValueWithPath">Jason value that has the expression with a referenced path.</param>
    /// <param name="referencedParsedValues">JSON values looked up by referenced path <see cref="JsonValuePath"/>.
    /// The list will not be empty, since this class does the path validations before calling <see cref="MutateValue"/>.
    /// </param>
    /// <param name="errors">The implementation can add errors to this list.</param>
    protected abstract void MutateValue(
        IParsedSimpleValue currentParsedSimpleValueWithPath,
        IReadOnlyList<IParsedValue> referencedParsedValues,
        List<IJsonObjectParseError> errors);
}