// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a JSON function that concatenates values provided as operands into a single string result.
/// </summary>
public class ConcatenateValuesJsonFunction : StringJsonFunctionAbstr
{
    private readonly IReadOnlyList<IJsonFunction> _operands;

    /// <summary>
    /// Represents a JSON function for concatenating multiple JSON values into a single unified string result.
    /// </summary>
    /// <remarks>
    /// Inherits from <see cref="StringJsonFunctionAbstr"/> to provide advanced functionality for
    /// combining multiple operands into a concatenated string within a JSON processing context.
    /// </remarks>
    public ConcatenateValuesJsonFunction(string functionName, IReadOnlyList<IJsonFunction> operands, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _operands = operands;
    }

    /// <inheritdoc />
    public override IParseResult<string?> EvaluateStringValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
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