using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

public class DefaultBooleanValueOperatorFunction : DefaultValueOperatorFunction, IBooleanJsonFunction
{
    public DefaultBooleanValueOperatorFunction(string operatorName, IBooleanJsonFunction mainValueJsonFunction, IBooleanJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    public IParseResult<bool?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToBoolean(LineInfo);
    }
}