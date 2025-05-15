using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that operates based on a JSON value path.
/// </summary>
public interface IJsonValuePathJsonFunction: IJsonFunction
{
    /// <summary>
    /// Gets the path information used for JSON value lookups within the function implementation.
    /// </summary>
    JsonValuePath JsonValuePath { get; }

    /// <summary>
    /// Evaluates the JSON value path function based on the provided parsed values and evaluation context.
    /// </summary>
    /// <param name="rootParsedValue">The root parsed value to evaluate.</param>
    /// <param name="compiledParentRootParsedValues">A collection of compiled parent root parsed values required for evaluation.</param>
    /// <param name="contextData">The optional context data used during the evaluation process.</param>
    /// <returns>A parse result containing the lookup result for the JSON value path.</returns>
    IParseResult<IJsonValuePathLookupResult> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData);
}

/// <summary>
/// Represents a JSON function implementation that evaluates expressions based on a specified JSON value path.
/// </summary>
public class JsonValuePathJsonFunction: JsonFunctionAbstr, IJsonValuePathJsonFunction
{
    private readonly IJsonValuePathLookup _jsonValuePathLookup;

    /// <summary>
    /// Represents a JSON function that evaluates JSON value paths using a provided lookup mechanism.
    /// </summary>
    /// <param name="functionName">The name of the function.</param>
    /// <param name="jsonValuePath">The JSON value path to be evaluated.</param>
    /// <param name="jsonValuePathLookup">The lookup mechanism for evaluating JSON value paths.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the function's value.</param>
    /// <param name="lineInfo">Optional information about the line where the function is located.</param>
    public JsonValuePathJsonFunction(string functionName, JsonValuePath jsonValuePath,
        IJsonValuePathLookup jsonValuePathLookup,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(
        functionName, jsonFunctionContext, lineInfo)
    {
        JsonValuePath = jsonValuePath;
        _jsonValuePathLookup = jsonValuePathLookup;
    }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return Evaluate(rootParsedValue, compiledParentRootParsedValues, contextData);
    }

    /// <inheritdoc />
    public JsonValuePath JsonValuePath { get; }

    /// <inheritdoc />
    public IParseResult<IJsonValuePathLookupResult> Evaluate(IRootParsedValue rootParsedValue, 
        IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        if (JsonValuePath.Path.Count > 0 && JsonValuePath.Path[0] is IJsonValuePropertyNamePathElement jsonValuePropertyNamePathElement)
        {
            var variableEvaluationResult = JsonFunctionValueEvaluationContext.VariablesManager.TryResolveVariableValue(jsonValuePropertyNamePathElement.Name);
            if (variableEvaluationResult != null)
            {
                if (variableEvaluationResult.Errors.Count > 0)
                    return new ParseResult<IJsonValuePathLookupResult>(variableEvaluationResult.Errors);

                if (variableEvaluationResult.Value is not IParsedValue parsedValue)
                    return new ParseResult<IJsonValuePathLookupResult>(CollectionExpressionHelpers.Create(
                        new JsonObjectParseError($"Invalid variable name [{jsonValuePropertyNamePathElement.Name}]", JsonValuePath.LineInfo)));

                if (JsonValuePath.Path.Count == 1)
                    return new ParseResult<IJsonValuePathLookupResult>(SingleItemJsonValuePathLookupResult.CreateForValidPath(parsedValue));
                
                return _jsonValuePathLookup.LookupJsonValue(parsedValue, this.JsonFunctionValueEvaluationContext, new JsonValuePath(JsonValuePath.Path.Skip(1).ToList()));
            }
        }

        return _jsonValuePathLookup.LookupJsonValue(rootParsedValue, compiledParentRootParsedValues, this.JsonFunctionValueEvaluationContext, JsonValuePath);
    }
}