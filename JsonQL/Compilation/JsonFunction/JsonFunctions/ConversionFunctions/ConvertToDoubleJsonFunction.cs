// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

/// <summary>
/// Represents a JSON function that attempts to convert a given value to a double.
/// This class inherits from <see cref="DoubleJsonFunctionAbstr"/> and provides the implementation for
/// handling the conversion logic and error handling when converting JSON values to a double.
/// </summary>
public class ConvertToDoubleJsonFunction : DoubleJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    /// <summary>
    /// Represents a JSON function that converts a specified input to a double value.
    /// </summary>
    /// <remarks>
    /// This class extends <see cref="DoubleJsonFunctionAbstr"/> and includes additional functionality,
    /// such as handling assertions when the conversion fails.
    /// </remarks>
    /// <param name="functionName">The name of the function being invoked.</param>
    /// <param name="jsonFunction">The JSON function providing the input value to be converted.</param>
    /// <param name="assertIfConversionFailsJsonFunction">An optional JSON function to assert conditions during failed conversion.</param>
    /// <param name="jsonFunctionContext">The context for evaluating the JSON function values.</param>
    /// <param name="lineInfo">Optional information about the source code line where the function is defined, for error reporting or debugging purposes.</param>
    public ConvertToDoubleJsonFunction(string functionName, IJsonFunction jsonFunction,
        IBooleanJsonFunction? assertIfConversionFailsJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
        _assertIfConversionFailsJsonFunction = assertIfConversionFailsJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<double?> GetDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedValueResult.Errors.Count > 0)
            return new ParseResult<double?>(evaluatedValueResult.Errors);

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedValueResult.Value, null, out var jsonComparable))
        {
            if (jsonComparable.Value is double doubleValue)
            {
                return new ParseResult<double?>(doubleValue);
            }

            if (jsonComparable.Value is string stringValue &&
                double.TryParse(stringValue, out doubleValue))
            {
                return new ParseResult<double?>(doubleValue);
            }
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<double?>((double?)null);

        return ConversionJsonFunctionHelpers.GetParseResultForConversionError<double?>(TypeCode.Double.ToString(),
                   _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.Evaluate(rootParsedValue,
                       compiledParentRootParsedValues, contextData), this.LineInfo) ??
               new ParseResult<double?>((double?)null);
    }
}