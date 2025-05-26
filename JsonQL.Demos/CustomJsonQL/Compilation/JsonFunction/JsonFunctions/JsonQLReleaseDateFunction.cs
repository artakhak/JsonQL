using JsonQL.Compilation.JsonFunction;
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Demos.CustomJsonQL.Compilation.JsonFunction.JsonFunctions;

public class JsonQLReleaseDateFunction: DateTimeJsonFunctionAbstr
{
    public JsonQLReleaseDateFunction(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public override IParseResult<DateTime?> EvaluateDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return new ParseResult<DateTime?>(new DateTime(2025, 6, 1, 0, 0, 0));
    }
}