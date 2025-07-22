// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.

using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

/// <summary>
/// Represents a JSON function implementation for converting input values to a <see cref="DateTime"/> format.
/// </summary>
/// <remarks>
/// This class is used to handle the conversion of JSON-compatible input values into <see cref="DateTime"/> objects.
/// It includes functionality to assert behavior when the conversion fails, if such assertions are provided.
/// </remarks>
public class ConvertToDateJsonFunction : DateTimeJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    /// <summary>
    /// A JSON function that converts a specified value to a date format.
    /// </summary>
    /// <param name="functionName">The name of the JSON function.</param>
    /// <param name="jsonFunction">A JSON function used to provide the value to be converted.</param>
    /// <param name="assertIfConversionFailsJsonFunction">Optional JSON function to assert if the conversion fails.</param>
    /// <param name="jsonFunctionContext">The evaluation context for the JSON function.</param>
    /// <param name="lineInfo">Optional line information for debugging and tracking.</param>
    public ConvertToDateJsonFunction(string functionName, IJsonFunction jsonFunction,
        IBooleanJsonFunction? assertIfConversionFailsJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
        _assertIfConversionFailsJsonFunction = assertIfConversionFailsJsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<DateTime?> EvaluateDateTimeValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedValueResult.Errors.Count > 0)
            return new ParseResult<DateTime?>(evaluatedValueResult.Errors);

        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedValueResult.Value, null, out var jsonComparable))
        {
            DateTime? parsedDate;
            if (jsonComparable.Value is DateTime date)
            {
                parsedDate = date;
            }
            else if (jsonComparable.Value is not string stringValue || !ThreadStaticDateTimeOperationsContext.Context.TryParse(stringValue, out parsedDate))
            {
                parsedDate = null;
            }

            if (parsedDate != null)
                return new ParseResult<DateTime?>(ThreadStaticDateTimeOperationsContext.Context.ConvertToDate(parsedDate.Value));
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<DateTime?>((DateTime?)null);

        return ConversionJsonFunctionHelpers.GetParseResultForConversionError<DateTime?> (TypeCode.DateTime.ToString(),
                _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.EvaluateBooleanValue(rootParsedValue, 
                    compiledParentRootParsedValues, contextData), this.LineInfo) ?? 
               new ParseResult<DateTime?>((DateTime?)null);
    }
}
