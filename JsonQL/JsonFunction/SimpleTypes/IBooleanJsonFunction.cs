using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.SimpleTypes;

public interface IBooleanJsonFunction : IJsonFunction
{
    IParseResult<bool?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}