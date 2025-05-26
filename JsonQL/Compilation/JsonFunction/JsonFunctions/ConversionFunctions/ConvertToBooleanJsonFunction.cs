// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.JsonObjects;
using TypeCode = JsonQL.Compilation.JsonFunction.SimpleTypes.TypeCode;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions.ConversionFunctions;

/// <summary>
/// Represents a JSON function that converts a given input to a Boolean value.
/// </summary>
/// <remarks>
/// This class is responsible for evaluating a JSON input and attempting to convert it into a Boolean value.
/// It also manages errors encountered during the conversion process and can optionally assert if conversion fails.
/// Inherits from <see cref="BooleanJsonFunctionAbstr"/>.
/// </remarks>
public class ConvertToBooleanJsonFunction : BooleanJsonFunctionAbstr
{
    private readonly IJsonFunction _jsonFunction;
    private readonly IBooleanJsonFunction? _assertIfConversionFailsJsonFunction;

    /// <summary>
    /// Represents a function that converts a value to a boolean type in JSON processing, inheriting from BooleanJsonFunctionAbstr.
    /// </summary>
    /// <param name="functionName">The name of the function to be used in JSON processing.</param>
    /// <param name="jsonFunction">The JSON function used to perform the conversion.</param>
    /// <param name="assertIfConversionFailsJsonFunction">Optional boolean function to be invoked if the conversion fails.</param>
    /// <param name="jsonFunctionContext">The context for evaluating JSON function values.</param>
    /// <param name="lineInfo">Optional line information associated with the JSON function.</param>
    public ConvertToBooleanJsonFunction(string functionName, IJsonFunction jsonFunction, IBooleanJsonFunction? assertIfConversionFailsJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) :
        base(functionName, jsonFunctionContext, lineInfo)
    {
        _jsonFunction = jsonFunction;
        _assertIfConversionFailsJsonFunction = assertIfConversionFailsJsonFunction;
    }

    /// <inheritdoc />
    public override IParseResult<bool?> EvaluateBooleanValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        var evaluatedValueResult = _jsonFunction.EvaluateValue(rootParsedValue, compiledParentRootParsedValues, contextData);

        if (evaluatedValueResult.Errors.Count > 0)
            return new ParseResult<bool?>(evaluatedValueResult.Errors);
       
        if (JsonFunctionHelpers.TryConvertValueToJsonComparable(evaluatedValueResult.Value, null, out var jsonComparable))
        {
            if (jsonComparable.Value is bool booleanValue)
                return new ParseResult<bool?>(booleanValue);

            if (jsonComparable.Value is string stringValue)
            {
                switch (stringValue)
                {
                    case "true":
                    case "True":
                        return new ParseResult<bool?>(true);

                    case "false":
                    case "False":
                        return new ParseResult<bool?>(false);
                 
                }
            }
        }

        if (_assertIfConversionFailsJsonFunction == null)
            return new ParseResult<bool?>((bool?)null);

        return 
            ConversionJsonFunctionHelpers.GetParseResultForConversionError<bool?>(TypeCode.Boolean.ToString(),
            _assertIfConversionFailsJsonFunction, _assertIfConversionFailsJsonFunction.EvaluateBooleanValue(rootParsedValue, compiledParentRootParsedValues, contextData),
            this.LineInfo) ?? new ParseResult<bool?>((bool?)null);
    }
}