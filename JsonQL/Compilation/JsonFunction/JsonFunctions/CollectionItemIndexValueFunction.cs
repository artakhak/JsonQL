using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a function used within a JSON query language framework to retrieve and evaluate
/// the index position of a collection item as a double value.
/// This function is specifically used in the context of JSON evaluations where the index position
/// of a collection item may hold relevance.
/// </summary>
/// <remarks>
/// This class inherits from <c>DoubleJsonFunctionAbstr</c>, providing specialized behavior for
/// extracting a double value derived from the index information available within the given context data.
/// </remarks>
public class CollectionItemIndexValueFunction : DoubleJsonFunctionAbstr
{
    /// <summary>
    /// Represents a function that evaluates a collection item based on its index and retrieves its double value.
    /// This class extends <see cref="DoubleJsonFunctionAbstr"/> and is primarily designed for processing JSON data
    /// where the index-based value evaluation is required.
    /// </summary>
    /// <param name="functionName">The name of the function being invoked.</param>
    /// <param name="jsonFunctionContext">The context for evaluating function values in a JSON processing scenario.</param>
    /// <param name="lineInfo">Optional line information associated with the function's definition in the source context.</param>
    public CollectionItemIndexValueFunction(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
    }

    /// <inheritdoc />
    public override IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        if (contextData?.Index == null)
            return new ParseResult<double?>((double?)null);

        return new ParseResult<double?>((double?)contextData.Index);
    }
}