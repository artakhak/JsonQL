using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.SimpleTypes;

public interface IStringJsonFunction : IJsonFunction
{
    IParseResult<string?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}