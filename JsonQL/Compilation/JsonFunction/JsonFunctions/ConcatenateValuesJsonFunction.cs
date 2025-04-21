using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

public class ConcatenateValuesJsonFunction : StringJsonFunctionAbstr
{
    private readonly IReadOnlyList<IJsonFunction> _operands;

    public ConcatenateValuesJsonFunction(string functionName, IReadOnlyList<IJsonFunction> operands, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _operands = operands;
    }

    /// <inheritdoc />
    protected override IParseResult<string?> GetStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        if (_operands.Count == 0)
            return new ParseResult<string?>(CollectionExpressionHelpers.Create(new JsonObjectParseError("Parameters are missing", this.LineInfo)));

        List<string> concatenatedValues = new List<string>(_operands.Count);

        foreach (var operand in _operands)
        {
            var evaluatedOperandValueResult = operand.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

            if (evaluatedOperandValueResult.Errors.Count > 0)
                return new ParseResult<string?>(evaluatedOperandValueResult.Errors);

            if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedOperandValueResult.Value, null, out var jsonComparable))
            {
                return new ParseResult<string?>((string?)null);
            }

            concatenatedValues.Add(jsonComparable.Value.ToString() ?? string.Empty);
        }

        return new ParseResult<string?>(string.Concat(concatenatedValues));
    }
}