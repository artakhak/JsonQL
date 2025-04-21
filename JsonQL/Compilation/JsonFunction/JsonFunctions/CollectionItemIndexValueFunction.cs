using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class CollectionItemIndexValueFunction : DoubleJsonFunctionAbstr
{
    public CollectionItemIndexValueFunction(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : 
        base(functionName, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    protected override IParseResult<double?> GetDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        if (contextData?.Index == null)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>((double?)contextData.Index);
    }
}