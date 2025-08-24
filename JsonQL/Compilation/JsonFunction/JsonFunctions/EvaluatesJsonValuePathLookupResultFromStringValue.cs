// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents the evaluation of a JSON value path lookup result for a string value.
/// This class derives from <see cref="EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr{T}"/>
/// to handle string-based JSON evaluation results.
/// </summary>
public class EvaluatesJsonValuePathLookupResultFromStringValue : EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr<string?>
{
    private readonly IStringJsonFunction _stringJsonFunction;
    public EvaluatesJsonValuePathLookupResultFromStringValue(IStringFormatter stringFormatter, string functionName, IStringJsonFunction stringJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(stringFormatter, functionName, jsonFunctionContext, lineInfo)
    {
        _stringJsonFunction = stringJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<string?> EvaluateParseResult(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this._stringJsonFunction.EvaluateStringValue(rootParsedValue, compiledParentRootParsedValues, contextData);
    }

    /// <inheritdoc />
    protected override bool IsStringJsonValue => true;
}