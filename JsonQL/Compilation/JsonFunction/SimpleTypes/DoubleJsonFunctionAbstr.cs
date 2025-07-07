// Copyright (c) JsonQL Project. All rights reserved.
// Licensed under the MIT License. See LICENSE in the solution root for license information.
using JsonQL.JsonObjects;

namespace JsonQL.Compilation.JsonFunction.SimpleTypes;

/// <summary>
/// Represents an abstract base class for JSON functions that evaluate to a double or nullable double value.
/// </summary>
/// <remarks>
/// This class provides a base implementation for JSON functions that handle numeric computations or operations.
/// Subclasses are expected to implement the abstract method <c>GetDoubleValue</c>, which defines the specific logic
/// for computing the double value based on the input parameters.
/// </remarks>
public abstract class DoubleJsonFunctionAbstr : JsonFunctionAbstr, IDoubleJsonFunction
{
    /// <summary>
    /// Represents an abstract base class for JSON functions that operate on or return double values.
    /// </summary>
    /// <remarks>
    /// This class serves as a foundation for implementing specific types of JSON functions that deal with double precision numerical values.
    /// Derived classes are expected to define specific behavior in the context of JSON evaluation.
    /// </remarks>
    protected DoubleJsonFunctionAbstr(string functionName, IJsonFunctionValueEvaluationContext jsonFunctionContext, IJsonLineInfo? lineInfo) : base(functionName, jsonFunctionContext, lineInfo)
    {
    }
    
    /// <inheritdoc />
    protected sealed override IParseResult<object?> DoEvaluateValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData)
    {
        return this.EvaluateDoubleValue(rootParsedValue, compiledParentRootParsedValues, contextData).ConvertToObject();
    }

    /// <inheritdoc />
    public abstract IParseResult<double?> EvaluateDoubleValue(IRootParsedValue rootParsedValue, IReadOnlyList<IRootParsedValue> compiledParentRootParsedValues, IJsonFunctionEvaluationContextData? contextData);
}