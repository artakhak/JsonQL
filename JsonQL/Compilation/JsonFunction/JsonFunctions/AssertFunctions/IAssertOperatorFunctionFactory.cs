using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.AssertFunctions;

/// <summary>
/// Defines a factory interface for creating JSON assert operator functions.
/// This is used to construct functions that evaluate assertions within the JSON compilation and evaluation context.
/// </summary>
public interface IAssertOperatorFunctionFactory
{
    /// <summary>
    /// Creates an assert operator function based on the specified parameters. This function evaluates whether
    /// certain assertions hold true within the provided JSON context.
    /// </summary>
    /// <param name="functionName">The name of the function to be created.</param>
    /// <param name="assertedFunction">The JSON function to be asserted by the operator function.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function.</param>
    /// <param name="lineInfo">Optional line information related to the operator's location in the JSON document.</param>
    /// <returns>A JSON assert operator function.</returns>
    IJsonFunction CreateAssertOperatorFunction(string functionName, IJsonFunction assertedFunction, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo);
}

/// <inheritdoc />
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