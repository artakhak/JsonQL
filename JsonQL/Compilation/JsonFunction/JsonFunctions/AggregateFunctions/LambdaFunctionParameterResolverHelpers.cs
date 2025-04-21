namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AggregateFunctions;

public static class LambdaFunctionParameterResolverHelpers
{
    public static IParseResult<object?>? TryEvaluateLambdaFunctionParameterValue<TJsonFunction>(ILambdaFunction<TJsonFunction>? lambdaFunctionData, string variableName, 
        IJsonFunctionEvaluationContextData? contextData) where TJsonFunction : IJsonFunction
    {
        if (lambdaFunctionData?.ParameterJsonFunction.Name != variableName)
            return null;

        if (contextData == null)
        {
            return new ParseResult<object?>(
                CollectionExpressionHelpers.Create(
                    new JsonObjectParseError("The context data was not set.", lambdaFunctionData.LambdaExpressionFunction.LineInfo)));
        }

        return new ParseResult<object?>(contextData.EvaluatedValue);
    }
}