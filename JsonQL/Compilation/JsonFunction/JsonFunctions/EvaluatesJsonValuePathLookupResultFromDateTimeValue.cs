// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents an implementation that evaluates a JSON value path lookup result from a DateTime value.
/// </summary>
/// <remarks>
/// This class is designed to handle JSON functions where the resulting value is expected to
/// be a DateTime or a nullable DateTime value. It utilizes a specific JSON function implementation
/// for DateTime values, allowing custom evaluation based on the JSON function's requirements.
/// </remarks>
public class EvaluatesJsonValuePathLookupResultFromDateTimeValue : EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr<DateTime?>
{
    private readonly IDateTimeJsonFunction _dateTimeJsonFunction;

    public EvaluatesJsonValuePathLookupResultFromDateTimeValue(IStringFormatter stringFormatter, string functionName, IDateTimeJsonFunction dateTimeJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(stringFormatter, functionName, jsonFunctionContext, lineInfo)
    {
        _dateTimeJsonFunction = dateTimeJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<DateTime?> EvaluateParseResult(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues,
        IJsonFunctionEvaluationContextData? contextData)
    {
        return this._dateTimeJsonFunction.EvaluateDateTimeValue(rootParsedValue, compiledParentRootParsedValues, contextData);
    }

    /// <inheritdoc />
    protected override bool IsStringJsonValue => true;
}