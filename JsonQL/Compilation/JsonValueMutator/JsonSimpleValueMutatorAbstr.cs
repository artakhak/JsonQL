using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;
using OROptimizer.Diagnostics.Log;

namespace JsonQL.Compilation.JsonValueMutator;

/// <summary>
/// Represents an abstract class for mutating simple JSON values. This class provides mechanisms
/// for manipulating parsed simple values and translating them by generating string values,
/// while also handling potential parsing errors that may occur during the mutation process.
/// </summary>
public abstract class JsonSimpleValueMutatorAbstr : JsonValueMutatorAbstr
{
    /// <summary>
    /// An abstract base class for mutating simple JSON values. This class is designed to handle
    /// parsed simple JSON values, such as strings, numbers, and booleans, and provides a mechanism
    /// for generating mutated string representations of these values. Additionally, it supports
    /// the detection and handling of errors that may arise during the mutation process.
    /// </summary>
    /// <remarks>
    /// This class serves as a foundational component in handling modifications to simple JSON values.
    /// Deriving classes must implement the necessary logic for generating string representations
    /// of the mutated JSON values.
    /// </remarks>
    protected JsonSimpleValueMutatorAbstr(IParsedSimpleValue parsedSimpleValue, IJsonLineInfo? lineInfo) : base(lineInfo)
    {
        ParsedSimpleValue = parsedSimpleValue;
    }

    /// <inheritdoc />
    public override void Mutate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, List<IJsonObjectParseError> errors)
    {
        var parsedSimpleValue = MutatorHelpers.TryGetParsedSimpleValue(this.ParsedSimpleValue);

        if (parsedSimpleValue == null)
        {
            ThreadStaticLogging.Log.InfoFormat("The mutator [{0}] will not execute.", GetType().FullName!);
            return;
        }

        var textGeneratorExpressionResult = GenerateStringValue(rootParsedValue, compiledParentRootParsedValues);

        if (textGeneratorExpressionResult.Errors.Count > 0 || textGeneratorExpressionResult.Value == null)
        {
            if (textGeneratorExpressionResult.Errors.Count == 0)
            {
                errors.Add(new JsonObjectParseError("Failed to parse the expression.", LineInfo));
            }
            else
            {
                errors.AddRange(textGeneratorExpressionResult.Errors);
            }

            return;
        }

        parsedSimpleValue.Value = textGeneratorExpressionResult.Value;
    }

    /// <summary>
    /// Parsed simple value with <see cref="IParsedSimpleValue.Value"/> containing the text parsed to <see cref="IJsonSimpleValueExpressionToStringConverter"/>.
    /// </summary>
    protected IParsedSimpleValue ParsedSimpleValue { get; }

    /// <summary>
    /// Generates a mutated string value for a given root parsed value and its associated compiled parent root parsed values.
    /// This method is used as part of the mutation process for transforming simple JSON values while ensuring that any errors
    /// encountered during the mutation are tracked and returned.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value that serves as the base for generating the mutated string value.</param>
    /// <param name="compiledParentRootParsedValues">A read-only list of parent root parsed values that are used to assist in generating the mutated value.</param>
    /// <returns>
    /// An object implementing <see cref="IParseResult{T}"/> where the value represents the mutated string and the errors, if any,
    /// represent issues encountered during the mutation process.
    /// </returns>
    protected abstract IParseResult<string> GenerateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues);
}