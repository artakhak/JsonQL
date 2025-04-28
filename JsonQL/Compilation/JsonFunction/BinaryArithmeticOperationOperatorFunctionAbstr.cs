using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction;

/// <summary>
/// Base class for arithmetic operator operations for operators such as '+', '-', '/', '*', etc.
/// </summary>
public abstract class BinaryArithmeticOperationOperatorFunctionAbstr : JsonFunctionAbstr
{
    /// <summary>
    /// Abstract base class representing a binary arithmetic operation for operators such as '+', '-', '/', '*', etc.
    /// Inherits from <see cref="JsonFunctionAbstr" /> and provides common properties and operations
    /// for evaluating binary arithmetic expressions.
    /// </summary>
    protected BinaryArithmeticOperationOperatorFunctionAbstr(string operatorName,
        IJsonFunction operand1, IJsonFunction operand2,
        IJsonFunctionValueEvaluationContext jsonFunctionContext,
        IJsonLineInfo? lineInfo) : base(operatorName, jsonFunctionContext, lineInfo)
    {
        Operand1 = operand1;
        Operand2 = operand2;
    }

    /// <summary>
    /// Gets the first operand for the binary arithmetic operation.
    /// This represents the left-hand side of the operation and is evaluated during the execution of the operation.
    /// </summary>
    protected IJsonFunction Operand1 { get; }

    /// <summary>
    /// Gets the second operand for the binary arithmetic operation.
    /// This represents the right-hand side of the operation and is evaluated during the execution of the operation.
    /// </summary>
    protected IJsonFunction Operand2 { get; }

    /// <inheritdoc />
    protected override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    { 
        var evaluatedOperand1ValueResult = Operand1.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedOperand1ValueResult.Errors.Count > 0)
            return new ParseResult<object?>(evaluatedOperand1ValueResult.Errors);

        var evaluatedOperand2ValueResult = Operand2.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedOperand2ValueResult.Errors.Count > 0)
            return new ParseResult<object?>(evaluatedOperand1ValueResult.Errors);
            
        if (!JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedOperand1ValueResult.Value, null, out var jsonComparable1) ||
            !JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedOperand2ValueResult.Value, null, out var jsonComparable2))
        {
            return new ParseResult<object?>((object?) null);
        }

        return Calculate(jsonComparable1, jsonComparable2);
    }

    /// <summary>
    /// Performs the calculation based on the two provided JSON-comparable operand values.
    /// </summary>
    /// <param name="operand1Value">The first operand as an instance of <see cref="IJsonComparable"/>.</param>
    /// <param name="operand2Value">The second operand as an instance of <see cref="IJsonComparable"/>.</param>
    /// <returns>An instance of <see cref="IParseResult{TValue}"/> containing the result of the calculation or any errors encountered during the process.</returns>
    protected abstract IParseResult<object?> Calculate(IJsonComparable operand1Value, IJsonComparable operand2Value);
}