using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an interface for JSON functions that evaluate to a result of type <see cref="double"/>.
/// </summary>
public interface IDoubleJsonFunction : IJsonFunction
{
    /// <summary>
    /// Evaluates the given root parsed value in the context of the provided parent root parsed values and evaluation context data,
    /// and returns a result of type <see cref="IParseResult{TValue}"/> containing a nullable double value.
    /// </summary>
    /// <param name="rootParsedValue">The root-level parsed value to be evaluated.</param>
    /// <param name="compiledParentRootParsedValues">A read-only list of parent parsed values that have been compiled.</param>
    /// <param name="contextData">The additional contextual data required for evaluation, if any.</param>
    /// <returns>An <see cref="IParseResult{TValue}"/> containing the result of the evaluation as a nullable double value.</returns>
    IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}