// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.Compilation.JsonFunction.SimpleTypes;
using JsonQL.Compilation.JsonValueTextGenerator;
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.JsonFunctions;

/// <summary>
/// Represents a class that evaluates JSON value path lookup results from a double value.
/// </summary>
/// <remarks>
/// This class inherits from <c>EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr&lt;double?&gt;</c>,
/// which provides the base functionality for evaluating JSON value path lookup results from simple value types.
/// </remarks>
public class EvaluatesJsonValuePathLookupResultFromDoubleValue : EvaluatesJsonValuePathLookupResultFromSimpleValueAbstr<double?>
{
    private readonly IDoubleJsonFunction _doubleJsonFunction;
    public EvaluatesJsonValuePathLookupResultFromDoubleValue(IStringFormatter stringFormatter, string functionName, IDoubleJsonFunction doubleJsonFunction,
        IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(stringFormatter, functionName, jsonFunctionContext, lineInfo)
    {
        _doubleJsonFunction = doubleJsonFunction;
    }

    /// <inheritdoc />
    protected override IParseResult<double?> EvaluateParseResult(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this._doubleJsonFunction.EvaluateDoubleValue(rootParsedValue, compiledParentRootParsedValues, contextData);
    }

    /// <inheritdoc />
    protected override bool IsStringJsonValue => false;
}