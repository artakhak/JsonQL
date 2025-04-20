using JsonQL.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions.AssertFunctions;

public interface IAssertOperatorFunctionFactory
{
    IJsonFunction CreateAssertOperatorFunction(string functionName, IJsonFunction assertedFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo);
}

public class AssertOperatorFunctionFactory : IAssertOperatorFunctionFactory
{
    /// <inheritdoc />
    public IJsonFunction CreateAssertOperatorFunction(string functionName, IJsonFunction assertedFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo)
    {
        if (assertedFunction is IBooleanJsonFunction booleanJsonFunction)
            return new AssertOperatorBooleanFunction(functionName, booleanJsonFunction, jsonFunctionContext, lineInfo);

        if (assertedFunction is IDoubleJsonFunction doubleJsonFunction)
            return new AssertOperatorDoubleFunction(functionName, doubleJsonFunction, jsonFunctionContext, lineInfo);

        if (assertedFunction is IDateTimeJsonFunction dateTimeJsonFunction)
            return new AssertOperatorDateTimeFunction(functionName, dateTimeJsonFunction, jsonFunctionContext, lineInfo);

        if (assertedFunction is IStringJsonFunction stringJsonFunction)
            return new AssertOperatorStringFunction(functionName, stringJsonFunction, jsonFunctionContext, lineInfo);
        
        return new AssertOperatorFunction(functionName, assertedFunction, jsonFunctionContext, lineInfo);
    }
}