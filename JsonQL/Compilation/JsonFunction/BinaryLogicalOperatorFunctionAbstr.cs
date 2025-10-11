// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueLookup;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// A function for binary boolean operators such as "&amp;", "||", etc.
/// </summary>
public abstract class BinaryLogicalOperatorFunctionAbstr : BooleanJsonFunctionAbstr
{
    /// <summary>
    /// Represents an abstract base class for binary logical operator functions such as "&&" and "||".
    /// </summary>
    /// <remarks>
    /// This class handles the evaluation of binary logical operations with two boolean operands.
    /// </remarks>
    protected BinaryLogicalOperatorFunctionAbstr(string operatorName,
        IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }

    /// <summary>
    /// Gets the first operand for the binary logical operation.
    /// </summary>
    /// <remarks>
    /// This property represents the first operand for binary logical operator functions,
    /// such as "&&" and "||". The operand is of type <see cref="IBooleanJsonFunction"/> and
    /// is evaluated as part of the logical operation.
    /// </remarks>
    protected IJsonFunction Operand1 { get; }

    /// <summary>
    /// Gets the second operand for the binary logical operation.
    /// </summary>
    /// <remarks>
    /// This property represents the second operand for binary logical operator functions,
    /// such as "&&" and "||". The operand is of type <see cref="IBooleanJsonFunction"/> and
    /// is evaluated as part of the logical operation.
    /// </remarks>
    protected IJsonFunction Operand2 { get; }

    /// <inheritdoc />
    public sealed override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var result1 = Operand1.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result1.Errors.Count > 0)
            return new ParseResult<bool?>(result1.Errors);

        var result2 = Operand2.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result2.Errors.Count > 0)
            return new ParseResult<bool?>(result2.Errors);

        return DoEvaluateBooleanValue(TryGetBooleanValue(result1), TryGetBooleanValue(result2));
    }

    private bool? TryGetBooleanValue(IParseResult<object?> parseResult)
    {
        if (parseResult.Value != null)
        {
            if (parseResult.Value is bool booleanValue)
                return booleanValue;

            if (parseResult.Value is ISingleItemJsonValuePathLookupResult singleItemJsonValuePathLookupResult)
            {
                if (singleItemJsonValuePathLookupResult.ParsedValue == null || 
                    !singleItemJsonValuePathLookupResult.IsValidPath || !singleItemJsonValuePathLookupResult.HasValue)
                    return null;

                if (singleItemJsonValuePathLookupResult.ParsedValue is IParsedSimpleValue parsedSimpleValue &&
                    !parsedSimpleValue.IsString && parsedSimpleValue.Value != null)
                {
                    switch (parsedSimpleValue.Value)
                    {
                        case "true":
                            return true;
                        case "false":
                            return false;
                    }
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Evaluates a binary logical operation given the evaluated boolean values of its two operands.
    /// </summary>
    /// <param name="evaluatedValueOfOperand1">The evaluated boolean value of the first operand. Can be null.</param>
    /// <param name="evaluatedValueOfOperand2">The evaluated boolean value of the second operand. Can be null.</param>
    /// <returns>The result of the binary logical operation as a parsed value, including potential parsing errors.</returns>
    protected abstract IParseResult<bool?> DoEvaluateBooleanValue(bool? evaluatedValueOfOperand1, bool? evaluatedValueOfOperand2);
}
