// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

/// <summary>
/// Represents a JSON function that attempts to convert a given input value into a double/int-equivalent representation.
/// This function evaluates the input JSON value, processes potential type conversion, and handles related errors during the conversion operation.
/// </summary>
/// <remarks>
/// This class is intended to support the conversion of JSON values by leveraging its base functionality provided by <see cref="DoubleJsonFunctionAbstr"/>.
/// It also provides error handling mechanisms through an optional assertion function for conversion failures.
/// </remarks>
public class ConvertToIntJsonFunction : DoubleJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    /// <summary>
    /// Represents a JSON function for converting values to integers. This class extends the abstract class for double JSON functions
    /// and implements specific functionality for integer conversion.
    /// </summary>
    /// <remarks>
    /// The conversion process involves evaluating the provided JSON function and attempting to convert its output to an integer.
    /// An optional assertion function can be provided to handle scenarios when the conversion fails.
    /// </remarks>
    /// <param name="functionName">The name of the function being converted.</param>
    /// <param name="jsonFunction">The JSON function whose value will be converted.</param>
    /// <param name="assertIfConversionFailsJsonFunction">An optional function to assert or handle conversion failures.</param>
    /// <param name="jsonFunctionContext">The context in which the JSON function is being evaluated.</param>
    /// <param name="lineInfo">Optional line information for the function definition.</param>
    public ConvertToIntJsonFunction(string functionName, IJsonFunction jsonFunction,
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
                return new ParseResult<double?>(((int)doubleValue));
            }
            
            if (jsonComparable.Value is string stringValue &&
                     double.TryParse(stringValue, out var parsedValue))
            {
                return new ParseResult<double?>((int)parsedValue);
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