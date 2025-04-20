using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.JsonFunction.JsonFunctions;

public interface IJsonValuePathJsonFunction: IJsonFunction
{
    JsonValuePath JsonValuePath { get; }
    IParseResult<IJsonValuePathLookupResult> Evaluate(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData);
}

public class JsonValuePathJsonFunction: JsonFunctionAbstr, IJsonValuePathJsonFunction
{
    private readonly IJsonValuePathLookup _jsonValuePathLookup;

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
            var variableEvaluationResult = JsonFunctionValueEvaluationContext.VariablesManager.TryEvaluateVariableValue(jsonValuePropertyNamePathElement.Name, contextData);
            
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