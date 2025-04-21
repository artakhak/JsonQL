//using JsonQL.Compilation.JsonFunction.SimpleTypes;
//using JsonQL.JsonObjects;

//namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

//public interface IBooleanLambdaExpressionFunction : ILambdaExpressionFunction, IBooleanJsonFunction
//{

//}

//public class BooleanLambdaExpressionFunction : LambdaExpressionFunction, IBooleanLambdaExpressionFunction
//{
//    public BooleanLambdaExpressionFunction(IFunctionMetadata functionMetadata, List<ILambdaFunctionParameterJsonFunction> parameters, IJsonFunction expression, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionMetadata, parameters, expression, jsonFunctionContext, lineInfo)
//    {
//    }

//    public IParseResult<bool?> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
//    {
//        var evaluatedResult = this.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

//        if (evaluatedResult.Errors.Count > 0)
//            return new ParseResult<bool?>(evaluatedResult.Errors);

//        if (evaluatedResult.Value is not bool booleanValue)
//            return new ParseResult<bool?>([new JsonObjectParseError("Failed to evaluate the expression to a valid boolean value.", this.Expression.LineInfo)]);

//        return new ParseResult<bool?>(booleanValue);
//    }
//}
