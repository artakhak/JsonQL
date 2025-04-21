namespace JsonQL.Compilation.JsonFunction;

public interface IResolvesVariableValue
{
    IParseResult<object?>? TryEvaluateVariableValue(string variableName, IJsonFunctionEvaluationContextData? contextData);
}