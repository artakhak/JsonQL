using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

public class DefaultDoubleValueOperatorFunction : DefaultValueOperatorFunction, IDoubleJsonFunction
{
    public DefaultDoubleValueOperatorFunction(string operatorName, IDoubleJsonFunction mainValueJsonFunction, IDoubleJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    public IParseResult<double?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDouble(LineInfo);
    }
}