using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

public class DefaultDateTimeValueOperatorFunction : DefaultValueOperatorFunction, IDateTimeJsonFunction
{
    public DefaultDateTimeValueOperatorFunction(string operatorName, IDateTimeJsonFunction mainValueJsonFunction, IDateTimeJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    public IParseResult<DateTime?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToDateTime(LineInfo);
    }
}