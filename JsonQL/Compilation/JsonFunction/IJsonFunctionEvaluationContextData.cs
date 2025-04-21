using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Json function context data.
/// </summary>
public interface IJsonFunctionEvaluationContextData
{
    /// <summary>
    /// Evaluated value.
    /// </summary>
    IParsedValue EvaluatedValue { get; }

    /// <summary>
    /// Index of <see cref="EvaluatedValue"/> in evaluated collection.
    /// The collection might be collection of items in <see cref="IParsedArrayValue"/>,
    /// or might be a larger collection that contains collection or <see cref="IParsedArrayValue"/> (or collection of collections, etc.).
    /// </summary>
    int? Index { get; }
}

public class JsonFunctionEvaluationContextData : IJsonFunctionEvaluationContextData
{
    public JsonFunctionEvaluationContextData(IParsedValue evaluatedValue, int? arrayIndex)
    {
        EvaluatedValue = evaluatedValue;
        Index = arrayIndex;
    }

    public IParsedValue EvaluatedValue { get; }
    public int? Index { get; }
}