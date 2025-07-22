// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Base class for binary comparison operators such as "==", "!=", ">", ">=", "&lt;", "&lt;=", "LIKE" etc.
/// </summary>
public abstract class BinaryComparisonOperatorFunctionAbstr : BooleanJsonFunctionAbstr
{
    /// <summary>
    /// Represents an abstract base class for binary comparison operator functions.
    /// Provides the foundation for specific binary comparison operations
    /// by handling two operands and their evaluation context.
    /// </summary>
    protected BinaryComparisonOperatorFunctionAbstr(string operatorName,
        IJsonFunction operand1, IJsonFunction operand2, IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }

    /// <summary>
    /// Gets the first operand of the binary comparison operator function.
    /// Represents the left-hand side value in the comparison operation.
    /// </summary>
    protected IJsonFunction Operand1 { get; }

    /// <summary>
    /// Gets the second operand of the binary comparison operator function.
    /// Represents the right-hand side value in the comparison operation.
    /// </summary>
    protected IJsonFunction Operand2 { get; }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var result1 = Operand1.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result1.Errors.Count > 0)
            return new ParseResult<bool?>(result1.Errors);

        var result2 = Operand2.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (result2.Errors.Count > 0)
            return new ParseResult<bool?>(result2.Errors);

        JsonFunctionHelpers.TryConvertValueToJsonComparable(result1.Value, null, out var jsonComparable1);
        JsonFunctionHelpers.TryConvertValueToJsonComparable(result2.Value, null, out var jsonComparable2);

        try
        {
            return DoEvaluateBooleanValue(jsonComparable1, jsonComparable2);
        }
        catch (Exception e)
        {
            ThreadStaticLoggingContext.Context.Error(e);
            return new ParseResult<bool?>(CollectionExpressionHelpers.Create(new JsonObjectParseError($"Evaluation of operator [{this.FunctionName}] failed.", this.LineInfo)));
        }
    }

    /// <summary>
    /// Compares values in <param name="evaluatedValueOfOperand1"></param> and <param name="evaluatedValueOfOperand2"></param>.
    /// </summary>
    /// <param name="evaluatedValueOfOperand1">The first compared value.</param>
    /// <param name="evaluatedValueOfOperand2">The second compared value.</param>
    /// <returns></returns>
    protected abstract IParseResult<bool?> DoEvaluateBooleanValue(IJsonComparable? evaluatedValueOfOperand1, IJsonComparable? evaluatedValueOfOperand2);
}
