using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an interface for JSON functions that work with strings as their primary data type.
/// </summary>
public interface IStringJsonFunction : IJsonFunction
{
    /// <summary>
    /// Evaluates the JSON function using the provided root parsed value, a list of compiled parent parsed values,
    /// and the contextual data for evaluation.
    /// </summary>
    /// <param name="rootParsedValue">
    /// The root parsed value that serves as the primary input for the evaluation.
    /// </param>
    /// <param name="compiledParentRootParsedValues">
    /// A read-only list of compiled parent parsed values that may contribute to the evaluation context.
    /// </param>
    /// <param name="contextData">
    /// The contextual data to provide additional information required during evaluation,
    /// or null if no context data is available.
    /// </param>
    /// <returns>
    /// A result object containing the evaluated value as a string, or null if evaluation could not produce a value,
    /// along with any errors encountered during the process.
    /// </returns>
    IParseResult<string?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}