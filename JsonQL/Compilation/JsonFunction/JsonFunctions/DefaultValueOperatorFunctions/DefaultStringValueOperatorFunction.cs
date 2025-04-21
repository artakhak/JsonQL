using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.DefaultValueOperatorFunctions;

public class DefaultStringValueOperatorFunction : DefaultValueOperatorFunction, IStringJsonFunction
{
    public DefaultStringValueOperatorFunction(string operatorName, IStringJsonFunction mainValueJsonFunction, IStringJsonFunction defaultValueJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, mainValueJsonFunction, defaultValueJsonFunction, jsonFunctionContext, lineInfo)
    {
    }

    public IParseResult<string?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToString(LineInfo);
    }
}