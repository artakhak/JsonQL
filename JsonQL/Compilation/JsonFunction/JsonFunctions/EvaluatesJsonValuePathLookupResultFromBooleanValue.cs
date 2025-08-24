// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents an evaluator that processes the result of a JSON value path lookup based on a boolean value.
/// </summary>
/// <remarks>
/// This class inherits from <c>EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr&lt;bool?&gt;</c>
/// and provides functionality to evaluate the parsed result of a JSON value using a boolean JSON function.
/// </remarks>
public class EvaluatesJsonValuePathLookupResultFromBooleanValue : EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr<bool?>
{
    private readonly IBooleanJsonFunction _booleanJsonFunction;

    public EvaluatesJsonValuePathLookupResultFromBooleanValue(IStringFormatter stringFormatter, string functionName, IBooleanJsonFunction booleanJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(stringFormatter, functionName, jsonFunctionContext, lineInfo)
    {
        _booleanJsonFunction = booleanJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<bool?> EvaluateParseResult(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this._booleanJsonFunction.EvaluateBooleanValue(rootParsedValue, compiledParentRootParsedValues, contextData);
    }

    /// <inheritdoc />
    protected override bool IsStringJsonValue => false;
}