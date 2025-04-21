//using JsonQL.JsonObjects;

//namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

///// <inheritdoc />
//public class ContextValueFunction: JsonFunctionAbstr
//{
//    public ContextValueFunction(IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(
//        new FunctionMetadata(JsonFunctionNames.ContextValue, FunctionReturnAndParameterTypes.Any), jsonFunctionContext, lineInfo)
//    {
//    }

//    /// <inheritdoc />
//    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
//    {
//        if (contextData == null)
//            return new ParseResult<object?>((object?) null);

//        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(contextData.EvaluatedValue, null, out var jsonComparable))
//        {
//            return new ParseResult<object?>((object?)null);
//        }

//        return new ParseResult<object?>(jsonComparable.Value);
//    }
//}