using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.SimpleTypes;

public interface IDoubleJsonFunction : IJsonFunction
{
    IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}